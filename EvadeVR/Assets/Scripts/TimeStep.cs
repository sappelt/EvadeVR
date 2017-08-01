using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStep : MonoBehaviour {
    private int x;
    private int y;
    private int time;

    public TimeStep(int x, int y, int time)
        {
            this.x = x;
            this.y = y;
            this.time = time;
        }
    }
