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
        public Vector3 Offset { get; set; }

        public Dictionary<int, Vector3> Path { get; set; }

        public String ItemName { get; set; }

        public Item()
        {
            Path = new Dictionary<int, Vector3>();
        }
    }
}
