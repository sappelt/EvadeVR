using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInfoController : MonoBehaviour {

    public GameObject itemInfo;

    private SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;

    private GameObject lastClickedObject = null;
    private GameObject pointedObject = null;

    public void Start()
    {
        itemInfo.SetActive(false);
    }

    private void OnEnable()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();

        laserPointer.PointerIn += LaserPointer_PointerIn;
        laserPointer.PointerOut += LaserPointer_PointerOut;

        trackedController = GetComponent<SteamVR_TrackedController>();
        trackedController.TriggerClicked += TrackedController_TriggerClicked;
    }

    private void TrackedController_TriggerClicked(object sender, ClickedEventArgs e)
    {
        if(lastClickedObject != null)
        {
            lastClickedObject.GetComponent<Renderer>().material.color = Color.white;
        }

        if(pointedObject != null) {
            pointedObject.GetComponent<Renderer>().material.color = Color.red;
            itemInfo.SetActive(true);
            itemInfo.transform.parent = pointedObject.transform;
            itemInfo.transform.localPosition = new Vector3(2, 0, 0);
            lastClickedObject = pointedObject;
        }
        else
        {
            itemInfo.SetActive(false);
        }


    }

    private void LaserPointer_PointerOut(object sender, PointerEventArgs e)
    {
        pointedObject = null;
    }

    private void LaserPointer_PointerIn(object sender, PointerEventArgs e)
    {
        pointedObject = e.target.gameObject;
    }


}
