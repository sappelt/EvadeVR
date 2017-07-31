using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCube : MonoBehaviour {

    public GameObject myCube;

	// Use this for initialization
	void Start () {
        GameObject cube = Instantiate(myCube);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
