using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Item
    {
        public GameObject Cube { get; set; }
        public List<GameObject> PathGameObjects { get; set; }

        public Vector3 Offset { get; set; }

        public SortedDictionary<int, Vector3> Path { get; set; }

        public String ItemName { get; set; }
        public HashSet<String> Machines { get; set; }

        public Item()
        {
            Path = new SortedDictionary<int, Vector3>();
            PathGameObjects = new List<GameObject>();
            Machines = new HashSet<string>();
        }
    }
}
