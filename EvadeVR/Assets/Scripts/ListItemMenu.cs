﻿using Assets.Scripts;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ListItemMenu : MonoBehaviour {
    
    public GameObject Canvas;
    public GameObject Panel;
    public GameObject ButtonItem;
    public ViveController viveController;

    List<Item> items;
    Text text;
    GameObject listItem;

    // Use this for initialization
    void Start () {
        viveController.MenuClickedHandler += ViveController_MenuClickedHandler;
        viveController.PointerIn += ViveController_PointerIn;
        viveController.PointerOut += ViveController_PointerOut;
        viveController.TriggerClicked += ViveController_TriggerClicked;
    }

    private void ViveController_TriggerClicked(object sender, ClickedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ViveController_PointerOut(object sender, PointerEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ViveController_PointerIn(object sender, PointerEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ViveController_MenuClickedHandler(object sender, ClickedEventArgs e)
    {
        Canvas.SetActive(true);
        instantiateText();
    }

    // Update is called once per frame
    void Update()
    {
        items = GetComponent<AddCube>().Items;

        if (Input.GetKeyDown(KeyCode.M))
        {
            Canvas.SetActive(true);
            instantiateText();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            Canvas.SetActive(false);
            destroyButtons();
        }
    }

    public void instantiateText()
    {
        for (int i = 0; i < items.Count; i++)
        {
            listItem = Instantiate(ButtonItem);
            listItem.transform.SetParent(Panel.transform, false);

            listItem.GetComponent<Button>().onClick.AddListener(OnClick);

            listItem.transform.GetChild(0).GetComponent<Text>().text = "Item" + i;
            
        }
    }
    
    public void OnClick()
    {
        Debug.Log("clicked!");
    }

    public void destroyButtons()
    {
        Destroy(Panel);
    }
    
}
