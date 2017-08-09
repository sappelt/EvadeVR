using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public SteamVR_Camera floorCamera;
    public SteamVR_Camera birdCamera;
    public SteamVR_Camera railCamera;
 
    void Start()
    {
        floorCamera.enabled = true;
        birdCamera.enabled = false;
        railCamera.enabled = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            floorCamera.enabled = true;
            birdCamera.enabled = false;
            railCamera.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            floorCamera.enabled = false;
            birdCamera.enabled = true;
            railCamera.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            floorCamera.enabled = false;
            birdCamera.enabled = false;
            railCamera.enabled = true;
        }
    }
    
}
