using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main : MonoBehaviour {
    TimeStep timestep;
    List<TimeStep> timeSteps = new List<TimeStep>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<TimeStep> parseCSV(string path)
    {
        try
        {
            using (StreamReader readFile = new StreamReader(path))
            {
                string line;
                string[] row;

                while ((line = readFile.ReadLine()) != null)
                {
                    row = line.Split(' ');
                    TimeStep timestep = new TimeStep(int.Parse(row[0]), int.Parse(row[1]), int.Parse(row[2]));
                    timeSteps.Add(timestep);
                }
            }
        }
        catch (Exception e)
        {
            //@TODO Exception
        }

        return timeSteps;
    }

}
