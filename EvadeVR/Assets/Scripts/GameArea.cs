using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameArea
    {
        public float AreaWidth { get; private set; }
        public float FieldSize { get; private set; }
        public float CubeSize { get; private set; }
        public Vector3 StartCoordinates { get; private set; }
        public Vector3 StartVector { get; private set; }
        public Vector3 MovementVector { get; private set; }
        public GameObject AreaGameObject { get; private set; }

        public GameArea(GameObject areaGameObject)
        {
            AreaGameObject = areaGameObject;
            //Get the Obejcts and calculate Stuff
            AreaWidth = areaGameObject.GetComponent<Renderer>().bounds.size.x - 15;
            FieldSize = AreaWidth / 12;
            CubeSize = FieldSize / 2;
            
            //Init start coordinates
            float startX = areaGameObject.transform.position.x - (AreaWidth / 2) + FieldSize / 2;
            float startZ = areaGameObject.transform.position.z - (AreaWidth / 2);
            float startY = FieldSize / 2;

            //Get the Prototype Cube and Calculate Values
            MovementVector = new Vector3(FieldSize, FieldSize / 10, FieldSize);
            StartVector = new Vector3(startX, 0, startZ);
        }
    }
}
