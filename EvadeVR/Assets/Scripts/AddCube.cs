using System;
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

	private List<GameObject> cubes  = new List<GameObject>();

	private float nextActionTime = 0.0f;
	private int step = 0;
	//private int maxSteps = 0;
	private float cubeWidth = 0; 
	//private float cubeCenterDistance = 0;

	private Vector3 startVector;
	private Vector3 scaleVector;

	//private Dictionary<int, Vector3> itemDict = new Dictionary<int, Vector3> ();

	TimeStep timestep;
	List<Dictionary<int, Vector3>> dictList = new List<Dictionary<int, Vector3>>();

	// Use this for initialization
	void Start () {
		//Parse Data from csv files 

		// ! WINDOWS VS MAC PATH NAMES!!!
		parseCSV(Application.dataPath+"/Resources");

		//Get the Obejcts and calculate Stuff
		cubeWidth = theCube.transform.localScale.x;

		//Random Factor 10, because 10 (Dafuq?!)
		startX = - (((areaGameObject.transform.localScale.x * areaGameObject.transform.parent.localScale.x * 10) - cubeWidth) / 2);
		startZ = - (((areaGameObject.transform.localScale.z * areaGameObject.transform.parent.localScale.z * 10) - cubeWidth) / 2);
		startY = cubeWidth / 2;

		//Get the Prototype Cube and Calculate Values
		scaleVector = new Vector3 (cubeWidth, cubeWidth, cubeWidth);
		startVector = new Vector3 (startX, 0, startZ);

		//Instantiate 1 cube for every trace
		for (int i = 0; i < dictList.Count; i++) {
			GameObject cubeInstance; 
			cubeInstance = Instantiate(theCube);
			cubeInstance.GetComponent<Renderer> ().material.color = Color.red;
			cubes.Add (cubeInstance);
		}
	
		//Cell 0 / 0
	}


	// Update is called once per frame
	void Update () {
		if (Time.time > nextActionTime ) {
			nextActionTime += stepDuration;

			step++;

			cubeInstance.GetComponent<Renderer> ().material.color = Color.red;
			Debug.Log (i);
			if (dictList[0].ContainsKey (step)) {
				//				Debug.Log (dictList [0] [step]);
				//				Debug.Log (startVector);
				//				Debug.Log (Vector3.Scale(dictList[0] [step], scaleVector));
				//				Debug.Log (Vector3.Scale(dictList[0] [step], scaleVector) + startVector);

				cubeInstance.transform.position = Vector3.Scale(dictList[0] [step], scaleVector) + startVector;
			} else {
				cubeInstance.GetComponent<Renderer> ().material.color = Color.blue;
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

				itemDict.Add(time, new Vector3() { x = float.Parse(line[0]), y = 0, z = float.Parse(line[1]) });
			}
			dictList.Add(itemDict);
		}	
	}

}
