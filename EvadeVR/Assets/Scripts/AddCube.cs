using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class AddCube : MonoBehaviour {

	public GameObject theCube;

	public float stepDuration = 0.2f;

	public int rows = 12;
	public int columns = 12;

	//private int areaWidth = 1000;
	//private int areaWHeight = 1000;

	private float startX = 0;
	private float startZ = 0;
	private float startY = 0;

	public GameObject areaGameObject;

	private List<GameObject> cubes = new List<GameObject>();

	private float nextActionTime = 0.0f;
	private int step = 0;
	//private int maxSteps = 0;
	private float cubeWidth = 0; 
	//private float cubeCenterDistance = 0;

	private Vector3 startVector;
	private Vector3 scaleVector;

	//private Dictionary<int, Vector3> itemDict = new Dictionary<int, Vector3> ();

	List<Dictionary<int, Vector3>> dictList = new List<Dictionary<int, Vector3>>();

	// Use this for initialization
	void Start () {
		//Parse Data from csv files 

		// ! WINDOWS VS MAC PATH NAMES!!!
		parseCSV(Application.dataPath+"/Resources");

        //Get the Obejcts and calculate Stuff
        float areaWidth = areaGameObject.GetComponent<Renderer>().bounds.size.x;
        cubeWidth = areaGameObject.GetComponent<Renderer>().bounds.size.x / 12;
        theCube.transform.localScale = new Vector3(cubeWidth, (float)(cubeWidth * 0.1), cubeWidth);

        //Random Factor 10, because 10 (Dafuq?!)
        
        startX = areaGameObject.transform.position.x - (areaWidth / 2) + cubeWidth / 2;
        startZ = areaGameObject.transform.position.z - (areaWidth / 2) + cubeWidth / 2;
		startY = cubeWidth / 2;

		//Get the Prototype Cube and Calculate Values
		scaleVector = new Vector3 (cubeWidth, cubeWidth/10, cubeWidth);
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
		foreach (int[] machine in machines) {
			GameObject cubeInstance;
			cubeInstance = Instantiate(theCube);

			cubeInstance.GetComponent<Renderer> ().material.color = Color.red;
			cubeInstance.transform.position = Vector3.Scale((new Vector3(machine[0], 1, machine[1])), scaleVector) + startVector;
			cubeInstance.name = "Machine: " + "(" + machine[0] + "," + machine[1] + ")";
		}
		foreach (int[] source in sources) {
			GameObject cubeInstance; 
			cubeInstance = Instantiate(theCube);
			cubeInstance.GetComponent<Renderer> ().material.color = Color.white;
			cubeInstance.transform.position = Vector3.Scale((new Vector3(source[0], 1, source[1])), scaleVector) + startVector;
			cubeInstance.name = "Source: " + "(" + source[0] + "," + source[1] + ")";
		}
		foreach (int[] sink in sinks) {
			GameObject cubeInstance; 
			cubeInstance = Instantiate(theCube);
			cubeInstance.GetComponent<Renderer> ().material.color = Color.black;
			cubeInstance.transform.position = Vector3.Scale((new Vector3(sink[0], 1, sink[1])), scaleVector) + startVector;
			cubeInstance.name = "Sink: " + "(" + sink[0] + "," + sink[1] + ")";
		}
	
		//Instantiate 1 cube for every trace
		for (int i = 0; i < dictList.Count; i++) {
			GameObject cubeInstance; 
			cubeInstance = Instantiate(theCube);
<<<<<<< HEAD
			cubeInstance.GetComponent<Renderer> ().material.color = UnityEngine.Random.ColorHSV(0.5f, 0.6f, 0.2f, 1f, 1f, 1f);
=======
			//cubeInstance.GetComponent<Renderer> ().material.color = new Color((i*50)%255, 255, 150);
>>>>>>> 6e60c904043896f910ce8ad8e87954c455275b08
            cubeInstance.transform.position = new Vector3(0, -10000, 0);
            cubeInstance.name = "Trace: " + i;
			cubes.Add (cubeInstance);
		}
	
		//Cell 0 / 0
	}


	// Update is called once per frame
	void Update () {
		if (Time.time > nextActionTime && 0 == 0 ) {
            nextActionTime += stepDuration;

			step++;

            int cubeIndex = 0;
            foreach(Dictionary<int, Vector3> trace in dictList)
            {
                if(trace.ContainsKey(step))
                {
<<<<<<< HEAD
                    cubes[cubeIndex].transform.position = Vector3.Scale(trace[step], scaleVector) + startVector;
                }
                else
                {
//                    cubes[cubeIndex].GetComponent<Renderer>().material.color = Color.blue;
=======
                    //cubes[cubeIndex].GetComponent<Renderer>().material.color = Color.red;
                    cubes[cubeIndex].transform.localPosition = Vector3.Scale(trace[step], scaleVector) + startVector;
                }
                else
                {
                    //cubes[cubeIndex].GetComponent<Renderer>().material.color = Color.blue;
>>>>>>> 6e60c904043896f910ce8ad8e87954c455275b08
                }

                cubeIndex++;
            }

		}
	}
		
	public void parseCSV(string directoryPath) {
		string[] files = Directory.GetFiles(directoryPath);
		Dictionary<int, Vector3> itemDict;
		foreach (string file in files)
		{
			if (!file.EndsWith(".csv"))
				continue;

			itemDict = new Dictionary<int, Vector3>();

			string sFileContents = new StreamReader(File.OpenRead(file)).ReadToEnd();

			string[] sFileLines = sFileContents.Split('\n');

			for (int i = 1; i < sFileLines.Length - 1; i++)
			{
				String[] line = sFileLines[i].Split(' ');
				int time = int.Parse(line[2]);

				itemDict.Add(time, new Vector3() { x = float.Parse(line[0]), y = 1, z = float.Parse(line[1]) });
			}
			dictList.Add(itemDict);
		}	
	}

}
