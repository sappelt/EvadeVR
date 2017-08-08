using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class CsvParser
    {
        public static List<Item> ParseItems(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath);
            List<Item> items = new List<Item>();
            Item currentItem;
            int itemIndex = 0;

            // Read trace files
            foreach (string file in files)
            {
                if (!file.Contains("trace") || !file.EndsWith(".csv"))
                    continue;

                currentItem = new Item();
                currentItem.ItemName = "Item " + itemIndex;

                string sFileContents = new StreamReader(File.OpenRead(file)).ReadToEnd();

                string[] sFileLines = sFileContents.Split('\n');
                for (int i = 1; i < sFileLines.Length - 1; i++)
                {
                    String[] line = sFileLines[i].Split(' ');
                    int time = int.Parse(line[2]);

                    currentItem.Path.Add(time, new Vector3() { x = float.Parse(line[0]), y = 1, z = float.Parse(line[1]) });
                }
                items.Add(currentItem);

                itemIndex++;
            }

            // Read Item Vars
            ReadItemVars(items, directoryPath);

            return items;
        }

        private static void ReadItemVars(List<Item> items, String directoryPath)
        {
            string sFileContents = new StreamReader(File.OpenRead(Path.Combine(directoryPath, "item_vars.csv"))).ReadToEnd();
            string[] sFileLines = sFileContents.Split('\n');
            for (int i = 1; i < sFileLines.Length - 1; i++)
            {

                String[] line = sFileLines[i].Split(';');

                int itemIndex = Int32.Parse(line[1]);
                //These are the comma separated machines
                String[] machines = line[2].Split(',');
                foreach(string machine in machines)
                {
                    items[itemIndex].Machines.Add(machine.Replace('[', ' ').Replace(']', ' ').Trim());
                }
            }
        }

        public static HashSet<Machine> ReadMachineVars(String directoryPath)
        {
            HashSet<Machine> machines = new HashSet<Machine>();
            string sFileContents = new StreamReader(File.OpenRead(Path.Combine(directoryPath, "factory_map10.csv"))).ReadToEnd();
            string[] sFileLines = sFileContents.Split('\n');
            for (int i = 1; i < sFileLines.Length - 1; i++)
            {

                String[] machinePositions = sFileLines[i].Split(';');

                foreach(String machinePosition in machinePositions)
                {
                    String[] position = machinePosition.Split(',');
                    float x = float.Parse(position[0].Replace('(', ' '). Replace(')', ' ').Trim());
                    float y = float.Parse(position[1].Replace('(', ' ').Replace(')', ' ').Trim());
                    machines.Add(new Machine() { Position = new Vector3(x, 1, y), Name = "Machine " + (i - 1).ToString() });
                }
                
            }

            return machines;
        }
    }
}
