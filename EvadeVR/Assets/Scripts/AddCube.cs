using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class AddCube : MonoBehaviour {

	public GameObject itemGameObject;
    public GameObject machineGameObject;
    public GameObject viveLeftController;
    public GameObject areaGameObject;

    public float stepDuration = 0.2f;
	public int rows = 12;
	public int columns = 12;
    private int step = 0;
    bool isPaused = false;
    ViveController viveController;
    Item currentSelectedItem;
    GameArea gameArea;

    List<Item> items = new List<Item>();

   
    

	// Use this for initialization
	void Start ()
    {
        InitViveController();
        LoadData();
        gameArea = new GameArea(areaGameObject);
        itemGameObject.transform.localScale = new Vector3(gameArea.CubeSize, (float)(gameArea.FieldSize * 0.05), gameArea.CubeSize);
       

        //Draw Machines/Sources/Sinks
        //TODO: Read from file (not hardcoded shit) and shorten obviously
        int[][] machines =
        {
            new int[] {7,6},
            new int[] {4,11},
            new int[] {6,0},
            new int[] {5,3},
            new int[] {10,10},
            new int[] {3,3},
        };
        int[][] sources =
        {
            new int[] {0,1},
            new int[] {0,4},
            new int[] {0,7},
            new int[] {0,10}
        };
        int[][] sinks =
        {
            new int[] {11,1},
            new int[] {11,4},
            new int[] {11,7},
            new int[] {11,10}
        };
        float machineSize = (float)(gameArea.FieldSize * (1 / 1800.0));
        foreach (int[] machine in machines)
        {
            GameObject machineInstace;
            machineInstace = Instantiate(machineGameObject);

            //machineInstace.GetComponent<Renderer>().material.color = Color.red;
            machineInstace.transform.position = Vector3.Scale((new Vector3(machine[0], 1, machine[1])), gameArea.MovementVector) 
                + gameArea.StartVector;
            machineInstace.transform.localScale = new Vector3(machineSize, machineSize, machineSize);
            machineInstace.name = "Machine: " + "(" + machine[0] + "," + machine[1] + ")";
        }
        //foreach (int[] source in sources)
        //{
        //    GameObject cubeInstance;
        //    cubeInstance = Instantiate(itemGameObject);
        //    cubeInstance.GetComponent<Renderer>().material.color = Color.white;
        //    cubeInstance.transform.position = Vector3.Scale((new Vector3(source[0], 1, source[1])), scaleVector) + startVector;
        //    cubeInstance.name = "Source: " + "(" + source[0] + "," + source[1] + ")";
        //}
        //foreach (int[] sink in sinks)
        //{
        //    GameObject cubeInstance;
        //    cubeInstance = Instantiate(itemGameObject);
        //    cubeInstance.GetComponent<Renderer>().material.color = Color.black;
        //    cubeInstance.transform.position = Vector3.Scale((new Vector3(sink[0], 1, sink[1])), scaleVector) + startVector;
        //    cubeInstance.name = "Sink: " + "(" + sink[0] + "," + sink[1] + ")";
        //}


        //Instantiate 1 cube for every trace
        for (int i = 0; i < items.Count; i++)
        {
            GameObject cubeInstance;
            cubeInstance = Instantiate(itemGameObject);
            //cubeInstance.GetComponent<Renderer> ().material.color = UnityEngine.Random.ColorHSV(0.5f, 0.6f, 0.2f, 1f, 1f, 1f);
            //cubeInstance.GetComponent<Renderer> ().material.color = new Color((i*50)%255, 255, 150);
            cubeInstance.transform.position = new Vector3(0, -10000, 0);
            cubeInstance.name = "Trace: " + i;

            float randomOffset = UnityEngine.Random.Range(0, gameArea.FieldSize - (gameArea.CubeSize));

            items[i].Cube = cubeInstance;
            items[i].Offset = new Vector3(randomOffset, 0, randomOffset);

            if (i == 5)
            {
                currentSelectedItem = items[i];
            }
        }

        //Cell 0 / 0
    }

    private void LoadData()
    {
        // ! WINDOWS VS MAC PATH NAMES!!!
        items = CsvParser.ParseCSV(Application.dataPath + "/Resources");
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


    private void TogglePause()
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

        int lastStep = step;
		step = (int)Math.Floor(Time.time / stepDuration);
        bool isFullStep = (lastStep != step);
        float progress = Math.Abs(step - (Time.time / stepDuration));

        items.ForEach(item =>
        {
            if (item.Path.ContainsKey(step) && item.Path.ContainsKey(step + 1))
            {
                Vector3 nextPosition = item.Path[step] + (item.Path[step + 1] - item.Path[step]) * progress;
                Vector3 itemPosition = Vector3.Scale(nextPosition, gameArea.MovementVector)
                    + gameArea.StartVector + item.Offset;

                //If we completed one step fully, we add a path cube
                if (isFullStep)
                {
                    CreatePathCube(itemPosition, item);
                }

                item.Cube.transform.position = itemPosition;
            }

            //If one item is selected, hide the others
            if (currentSelectedItem != null && currentSelectedItem.Equals(item))
            {
                //HighlightPath(item.PathGameObjects);
            }
            else
            {
                //HideCube(item.Cube);
                //HidePathCubes(item);
            }
        });
	}

    private void HighlightPath(List<GameObject> pathGameObjects)
    {
        pathGameObjects.ForEach(pathCube => {
            UpdateCubeColor(pathCube, Color.red);
            SetCubeY(pathCube, 5);
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

    private void CreatePathCube(Vector3 position, Item item)
    {
        GameObject pathCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pathCube.transform.position = position;
        pathCube.transform.localScale = new Vector3(gameArea.CubeSize, (float)(gameArea.FieldSize * 0.005), gameArea.CubeSize);

        UpdateCubeColor(pathCube, Color.white);

        item.PathGameObjects.Add(pathCube);
    }
}
