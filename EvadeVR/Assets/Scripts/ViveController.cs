using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {
    private SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;

    public delegate void ClickedHandler(object sender, ClickedEventArgs e);
    public delegate void PointerInHandler(object sender, PointerEventArgs e);
    public delegate void PointerOutHandler(object sender, PointerEventArgs e);
    public delegate void MenuHandler(object sender, ClickedEventArgs e);

    public event ClickedHandler TriggerClicked;
    public event ClickedHandler PadClicked;
    public event PointerInHandler PointerIn;
    public event PointerOutHandler PointerOut;
    public event MenuHandler MenuClickedHandler;

    bool triggerPressed = false;

    public void Start()
    {
    }

    private void OnEnable()
    {
        laserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();

        laserPointer.PointerIn += LaserPointer_PointerIn;
        laserPointer.PointerOut += LaserPointer_PointerOut;

        trackedController = gameObject.GetComponent<SteamVR_TrackedController>();
        trackedController.TriggerClicked += TrackedController_TriggerClicked;
        trackedController.PadClicked += TrackedController_PadClicked;

        trackedController.MenuButtonClicked += TrackedController_MenuButtonClicked;
    }

    private void TrackedController_MenuButtonClicked(object sender, ClickedEventArgs e)
    {
        MenuClickedHandler(sender, e);
    }

    private void TrackedController_PadClicked(object sender, ClickedEventArgs e)
    {
        if(!triggerPressed) { 
            PadClicked(sender, e);
            triggerPressed = true;
        }
        else
        {
            triggerPressed = false;
        }
    }

    private void TrackedController_TriggerClicked(object sender, ClickedEventArgs e)
    {
        TriggerClicked(sender, e);
    }

    private void LaserPointer_PointerOut(object sender, PointerEventArgs e)
    {
        PointerOut(sender, e);
    }

    private void LaserPointer_PointerIn(object sender, PointerEventArgs e)
    {
        PointerIn(sender, e);
    }
}
