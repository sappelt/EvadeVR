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

	public float stepDuration = 0.2f;

	public int rows = 12;
	public int columns = 12;

	//private int areaWidth = 1000;
	//private int areaWHeight = 1000;

	private float startX = 0;
	private float startZ = 0;
	private float startY = 0;

	public GameObject areaGameObject;

	private int step = 0;
	//private int maxSteps = 0;
	private float fieldSize = 0; 
	//private float cubeCenterDistance = 0;

	private Vector3 startVector;
	private Vector3 scaleVector;

	//private Dictionary<int, Vector3> itemDict = new Dictionary<int, Vector3> ();

	List<Item> items = new List<Item>();

    bool isPaused = false;
    ViveController viveController;

	// Use this for initialization
	void Start () {
        viveController = viveLeftController.GetComponent<ViveController>();
        viveController.PointerIn += ViveController_PointerIn;
        viveController.PointerOut += ViveController_PointerOut;
        viveController.TriggerClicked += ViveController_TriggerClicked;
        viveController.PadClicked += ViveController_PadClicked;

		//Parse Data from csv files 

		// ! WINDOWS VS MAC PATH NAMES!!!
		items = CsvParser.ParseCSV(Application.dataPath+"/Resources");

        //Get the Obejcts and calculate Stuff
        float areaWidth = areaGameObject.GetComponent<Renderer>().bounds.size.x;
        fieldSize = areaGameObject.GetComponent<Renderer>().bounds.size.x / 12;
        float cubeWidth = fieldSize / 2;
        itemGameObject.transform.localScale = new Vector3(cubeWidth, (float)(fieldSize * 0.05), cubeWidth);

        //Random Factor 10, because 10 (Dafuq?!)
        
        startX = areaGameObject.transform.position.x - (areaWidth / 2) + fieldSize / 2;
        startZ = areaGameObject.transform.position.z - (areaWidth / 2) + fieldSize / 2;
		startY = fieldSize / 2;

		//Get the Prototype Cube and Calculate Values
		scaleVector = new Vector3 (fieldSize, fieldSize/10, fieldSize);
		startVector = new Vector3 (startX, 0, startZ);

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
        float machineSize = (float)(fieldSize * (1 / 1800.0));
        foreach (int[] machine in machines)
        {
            GameObject machineInstace;
            machineInstace = Instantiate(machineGameObject);

            //machineInstace.GetComponent<Renderer>().material.color = Color.red;
            machineInstace.transform.position = Vector3.Scale((new Vector3(machine[0], 1, machine[1])), scaleVector) + startVector;
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
        for (int i = 0; i < items.Count; i++) {
			GameObject cubeInstance; 
			cubeInstance = Instantiate(itemGameObject);
			//cubeInstance.GetComponent<Renderer> ().material.color = UnityEngine.Random.ColorHSV(0.5f, 0.6f, 0.2f, 1f, 1f, 1f);
			//cubeInstance.GetComponent<Renderer> ().material.color = new Color((i*50)%255, 255, 150);
            cubeInstance.transform.position = new Vector3(0, -10000, 0);
            cubeInstance.name = "Trace: " + i;

            float randomOffset = UnityEngine.Random.Range(0, fieldSize-(cubeWidth));

            items[i].Cube = cubeInstance;
            items[i].Offset = new Vector3(randomOffset, 0, randomOffset);
		}
	
		//Cell 0 / 0
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

		step = (int)Math.Floor(Time.time / stepDuration);
        float progress = Math.Abs(step - (Time.time / stepDuration));

        int cubeIndex = 0;
        items.ForEach(item =>
        {
            if (item.Path.ContainsKey(step) && item.Path.ContainsKey(step + 1))
            {
                Vector3 nextPosition = item.Path[step] + (item.Path[step + 1] - item.Path[step]) * progress;

                item.Cube.transform.position = Vector3.Scale(nextPosition, scaleVector)
                    + startVector + item.Offset;
            }

            cubeIndex++;
        });
	}
		
	

}
