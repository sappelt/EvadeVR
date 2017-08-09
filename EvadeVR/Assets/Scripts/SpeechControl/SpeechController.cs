using Assets.Scripts.SpeechControl;
using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechController : MonoBehaviour {

    public SocketIOComponent socket;
    public AddCube GameArea;

    // Use this for initialization
    void Start () {
        socket.On("command", CommandReceived);
    }

    public void CommandReceived(SocketIOEvent e)
    {
        if(e.name == "command" && e.data.HasField("commandType"))
        {
            String commandTypeString = e.data["commandType"].str;
            if (Enum.IsDefined(typeof(Command), commandTypeString))
            {
                Command command = (Command)Enum.Parse(typeof(Command), commandTypeString);
                switch(command)
                {
                    case Command.Pause:
                        GameArea.TogglePause();
                        break;
                    case Command.Resume:
                        GameArea.TogglePause();
                        break;
                    case Command.Reset:
                        GameArea.ResetTime();
                        break;
                    case Command.ToggleHeatmap:
                        throw new NotImplementedException();
                    case Command.SwitchCamera:
                        throw new NotImplementedException();
                    case Command.ShowItemDetails:
                        int itemId = 0;
                        if(Int32.TryParse(e.data["itemId"].str, out itemId))
                        {
                            GameArea.ShowItemDetails(itemId);
                        }
                        break;
                    case Command.JumpToTime:
                        int timeStep = 0;
                        if (Int32.TryParse(e.data["step"].str, out timeStep))
                        {
                            GameArea.TravelInTime(timeStep);
                        }
                        break;
                    case Command.ShowItemMenu:
                        throw new NotImplementedException();
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
