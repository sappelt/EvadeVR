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
                String[] line = sFileLines[i].Split(',');
            }
        }
    }
}
