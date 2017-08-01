using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main : MonoBehaviour
{
    TimeStep timestep;
    List<Dictionary<int, Vector3>> dictList = new List<Dictionary<int, Vector3>>();

    // Use this for initialization
    void Start()
    {
        parseCSV(Application.dataPath+"\\Resources");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void parseCSV(string directoryPath)
    {
        string[] files = Directory.GetFiles(directoryPath);
        Dictionary<int, Vector3> itemDict;
        foreach (string file in files)
        {
            if (!file.EndsWith(".csv"))
                continue;

            itemDict = new Dictionary<int, Vector3>();

            Debug.Log(file);
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
