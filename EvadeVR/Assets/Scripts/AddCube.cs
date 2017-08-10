using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AddCube : MonoBehaviour {

    public GameObject timeStepText;
	public GameObject itemGameObject;
    public List<GameObject> machineGameObjects;
    public GameObject viveLeftController;
    public GameObject areaGameObject;
	public GameObject gridCellGameObject;

    public float stepDuration = 0.2f;
	public int rows = 12;
	public int columns = 12;
    private int step = 0;

	private bool drawPersistentHeatmap = true;
	private bool drawTemporalHeatmap = false;
    
	bool isPaused = false;
    float progress;
    float startTime = 0;
    float gameTime = 0;


    ViveController viveController;
    Item currentSelectedItem;
	GameArea gameArea;
	Heatmap heatmap;
	HeatField heatfield;
	List<Item> items = new List<Item>();

	GameObject[,] map;
	//Dictionary <Vector2, HeatField> grid = new Dictionary<Vector2, HeatField> ();

    public List<Item> Items = new List<Item>();
    HashSet<Machine> machines = new HashSet<Machine>();
  
	// Use this for initialization
	void Start ()
    {
        InitViveController();
        LoadData();
        gameArea = new GameArea(areaGameObject);
        itemGameObject.transform.localScale = new Vector3(gameArea.CubeSize, (float)(gameArea.FieldSize * 0.05), gameArea.CubeSize);
	
		map = new GameObject[columns,rows];

		//Initiate the Grid Background
		float cellSize = (float)(gameArea.FieldSize);
		int gridCounter = 0;

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				GameObject cellInstance;
				cellInstance = Instantiate (gridCellGameObject);
				cellInstance.transform.Rotate(90, 0, 0);
				cellInstance.GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 0.8f);
//				cellInstance.GetComponent<Renderer> ().material.color = Color.clear;

				cellInstance.transform.position = Vector3.Scale ((new Vector3 (i, 0.2f, j)), gameArea.MovementVector)
					+ gameArea.StartVector;
				cellInstance.transform.localScale = new Vector3 (cellSize, cellSize, cellSize);
				cellInstance.name = "GridCell: " + "(" + i + "," + j + ")";

				map [i, j] = cellInstance;
				//grid.Add (new Vector2 ((float)i, (float)j), new HeatField (cellInstance, 0));

				gridCounter++;
			}
			gridCounter++;
		}
		this.heatmap = new Heatmap (map);
			

		//End Initiate Background

        //Draw Machines/Sources/Sinks
        //TODO: Read from file (not hardcoded shit) and shorten obviously
//        int[][] machines =
//        {
//            new int[] {7,6},
//            new int[] {4,11},
//            new int[] {6,0},
//            new int[] {5,3},
//            new int[] {10,10},
//            new int[] {3,3},
//        };
//
//		float machineSize = (float)(gameArea.FieldSize * (1 / 1800.0));
//        foreach (int[] machine in machines)
//        {
//            GameObject machineInstace;
//            machineInstace = Instantiate(machineGameObjects);
//
//            //machineInstace.GetComponent<Renderer>().material.color = Color.red;
//            machineInstace.transform.position = Vector3.Scale((new Vector3(machine[0], 1, machine[1])), gameArea.MovementVector) 
//                + gameArea.StartVector;
//            machineInstace.transform.localScale = new Vector3(machineSize, machineSize, machineSize);
//            machineInstace.name = "Machine: " + "(" + machine[0] + "," + machine[1] + ")";
//        }

        CreateMachines();
        CreateItems();
       
    }

    private void CreateItems()
    {
        //Instantiate 1 cube for every trace
        for (int i = 0; i < Items.Count; i++)
        {
            GameObject cubeInstance;
            cubeInstance = Instantiate(itemGameObject);
            //cubeInstance.GetComponent<Renderer> ().material.color = UnityEngine.Random.ColorHSV(0.5f, 0.6f, 0.2f, 1f, 1f, 1f);
            //cubeInstance.GetComponent<Renderer> ().material.color = new Color((i*50)%255, 255, 150);
            cubeInstance.transform.position = new Vector3(0, -10000, 0);
            cubeInstance.name = "Trace: " + i;
            ItemDetailsHandler detailsHandler = cubeInstance.AddComponent<ItemDetailsHandler>();
            detailsHandler.Item = Items[i];
            detailsHandler.ItemClicked += DetailsHandler_ItemClicked;

            float randomOffset = UnityEngine.Random.Range(0, gameArea.FieldSize - (gameArea.CubeSize));

            Items[i].Cube = cubeInstance;
            Items[i].Offset = new Vector3(randomOffset, 0, randomOffset);
        }

    }

    private void CreateMachines()
    {
        float machineSize = gameArea.FieldSize;

        foreach (Machine machine in machines)
        {
            GameObject machineInstace;

            //Different machine model for different machine type
            int modelIndex = machine.Type % machineGameObjects.Count;
            machineInstace = Instantiate(machineGameObjects[modelIndex]);

            //machineInstace.GetComponent<Renderer>().material.color = Color.red;
            machineInstace.transform.position = Vector3.Scale((new Vector3(machine.Position.x, 1, machine.Position.z)), 
                gameArea.MovementVector) + gameArea.StartVector;
            //machineInstace.transform.localScale = new Vector3(machineSize, machineSize, machineSize);
            machineInstace.name = machine.Name;
        }
    }

    private void DetailsHandler_ItemClicked(object sender, ItemClickedEventArgs e)
    {
        Debug.Log("Clicked the item: " + e.Item.ItemName);
        ShowItemDetails(e.Item);
    }

    internal void ShowItemDetails(int itemId)
    {
        if(Items.Count > itemId)
        {
            ShowItemDetails(Items[itemId]);
        }
    }

    public void ShowItemDetails(Item item)
    {
        currentSelectedItem = item;
    }

    private void LoadData()
    {
        // ! WINDOWS VS MAC PATH NAMES!!!
        Items = CsvParser.ParseItems(Application.dataPath + "/Resources");
        machines = CsvParser.ReadMachineVars(Application.dataPath + "/Resources");
    }

    private void InitViveController()
    {
        viveController = viveLeftController.GetComponent<ViveController>();
        viveController.PointerIn += ViveController_PointerIn;
        viveController.PointerOut += ViveController_PointerOut;
        viveController.TriggerClicked += ViveController_TriggerClicked;
        viveController.PadClicked += ViveController_PadClicked;
    }

    private void ViveController_PadClicked(object sender, ClickedEventArgs e)
    {
        TogglePause();
    }

    private void ViveController_TriggerClicked(object sender, ClickedEventArgs e)
    {
        Debug.Log("Trigger clicked");
    }

    private void ViveController_PointerOut(object sender, PointerEventArgs e)
    {
        Debug.Log("Pointer Out");
    }

    private void ViveController_PointerIn(object sender, PointerEventArgs e)
    {
        Debug.Log("Pointer In");
    }


    public void TogglePause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.P))
        {
            TogglePause();
        }
		if (Input.GetKeyUp(KeyCode.S))
		{
			ResetTime();
		}

		if (Input.GetKeyUp(KeyCode.H)) {
			this.toggleHeatmapVisibility ();
		}
		if (Input.GetKeyUp(KeyCode.G)) {
			this.toggleHeatmapStyle ();
		}

				
        gameTime = Time.time - startTime;
        int lastStep = step;
		step = (int)Math.Floor(gameTime / stepDuration);
        bool isFullStep = (lastStep != step);
        progress = Math.Abs(step - (gameTime / stepDuration));
        
        setTimeStepOnCanvas();
		
		//*HEATMAP*
		int heatmapStep = (int)Math.Ceiling(Time.time / stepDuration);
		if (drawPersistentHeatmap == false) {
			heatmap.reset();
		}
		//*HEATMAP*
        
		Items.ForEach(item =>
        {
            if (item.Path.ContainsKey(step))
            {
                int nextStep = GetNextStep(item, step);
                if(nextStep == -1)
                    return;

                Vector3 nextPosition = item.Path[step] + (item.Path[nextStep] - item.Path[step]) * progress;
                Vector3 itemPosition = Vector3.Scale(nextPosition, gameArea.MovementVector)
                    + gameArea.StartVector + item.Offset;
                //If we completed one step fully, we add a path cube
			
				Vector2 temp = new Vector2(item.Path[heatmapStep].x, item.Path[heatmapStep].z);

                if (isFullStep)
                {
                    itemPosition.y = 1;

					//CreatePathCube(itemPosition, item);

                    int itemFirstStep = item.Path.Keys.First();
                    //Create a path for the first waypoint
                    if(itemFirstStep == step-1)
                    {
                        Vector3 firstPathVector = Vector3.Scale(item.Path[itemFirstStep], gameArea.MovementVector)
                    + gameArea.StartVector + item.Offset;

                        CreatePathCube(firstPathVector, item);
                    }

					//*HEATMAP*
					if (drawPersistentHeatmap == true) {
						heatmap.update("persistent", temp);
					}
					//*Persistent HEATMAP*
					
					if (drawPersistentHeatmap == false) {
						heatmap.update("temporal", temp);
					}
					//*HEATMAP*
                }
               	
				item.Cube.transform.position = itemPosition;

			} else {
				if (drawPersistentHeatmap == false) {
					for(int i = heatmapStep-1; i >= 0; i--) {
						if(item.Path.ContainsKey(i)) {
							Vector2 temp = new Vector2(item.Path[i].x, item.Path[i].z);
							heatmap.update("temporal", temp);
							//							grid[temp].addUpVisited();
							break;
						}
					}
				}
			}

            //If one item is selected, hide the others
            if (currentSelectedItem != null && currentSelectedItem.Equals(item))
            {
                HighlightPath(item.PathGameObjects);
            }
            else {
                //HideCube(item.Cube);
                //HidePathCubes(item);
                UnhighlightPath(item.PathGameObjects);
            }
//			
//			if (item.Path.ContainsKey(step)) {
//				item.Path[step];gogo
//				Debug.Log(step);
//			}
        });

		//*HEATMAP*
		//Draws a persistent heatmap (factor maybe depending on stepduration)
		if (drawPersistentHeatmap == false ) {
			heatmap.draw ();
		}
		//*HEATMAP*

	}

    private int GetNextStep(Item item, int step)
    {
       int lastKey = item.Path.Keys.Last();
        
        for(int i = step+1; i < lastKey; i++)
        {
            if (item.Path.ContainsKey(i))
                return i;
        }
        return -1;
    }

    public void ResetTime()
    {
        //Jump to timestep 0
        TravelInTime(0);
    }

    public void TravelInTime(int step)
    {
        startTime = Time.time - step * stepDuration;

        if (Time.time - startTime < 0)
        {
            startTime = 0;
        }
        HideItems(step);
    }

    private void HideItems(int step)
    {
        Items.ForEach(item =>
        {
            //Hide all cubes and paths
            HideCube(item.Cube);
            item.PathGameObjects.ForEach(pathCube => Destroy(pathCube));
            item.PathGameObjects.Clear();

            //Add Cubes and Paths again (that have a position < step)
            for(int time = 0; time < step; time++)
            {
                if(item.Path.ContainsKey(time))
                {
                    Vector3 position = Vector3.Scale(item.Path[time], gameArea.MovementVector)
                    + gameArea.StartVector + item.Offset;

                    item.Cube.transform.position = position;
                    CreatePathCube(position, item);
                }
            }
        });
    }

    //update text on Canvas for each timestep
    public void setTimeStepOnCanvas()
    {
        String timeStep = (step + progress).ToString("0.00");
        Text text = timeStepText.GetComponent<Text>();
        text.text = timeStep;
    }


    private void HighlightPath(List<GameObject> pathGameObjects)
    {
        pathGameObjects.ForEach(pathCube => {
            UpdateCubeColor(pathCube, Color.red);
            SetCubeY(pathCube, 1.1f);
            }
        );
    }

    private void UpdateCubeColor(GameObject pathCube, Color color)
    {
        pathCube.GetComponent<Renderer>().material.color = color;
    }

    private void HideCube(GameObject cube)
    {
        SetCubeY(cube, -10000);
    }

    private void SetCubeY(GameObject cube, float y)
    {
        Vector3 currentPosition = cube.transform.position;
        currentPosition.y = y;
        cube.transform.position = currentPosition;
    }

    private void HidePathCubes(Item item)
    {
        item.PathGameObjects.ForEach(cube => HideCube(cube));
    }

    private void UnhighlightPath(List<GameObject> pathGameObjects)
    {
        pathGameObjects.ForEach(pathCube => {
            UpdateCubeColor(pathCube, Color.white);
            SetCubeY(pathCube, 1);
            });
    }

    private void CreatePathCube(Vector3 position, Item item)
    {
        GameObject pathCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pathCube.transform.position = position;
        pathCube.transform.localScale = new Vector3(gameArea.CubeSize, (float)(gameArea.FieldSize * 0.005), gameArea.CubeSize);

        UpdateCubeColor(pathCube, Color.white);

        item.PathGameObjects.Add(pathCube);
    }

	private void toggleHeatmapVisibility() {
		this.heatmap.toggleVisibility ();
	}
	private void toggleHeatmapStyle() {
		this.drawPersistentHeatmap = !this.drawPersistentHeatmap;
	}

}
