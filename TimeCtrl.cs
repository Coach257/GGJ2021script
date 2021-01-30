using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCtrl : MonoBehaviour
{
    static private float time;


    static public float GetTime()
    {
        return time;
    }

    static public float GetYear()
    {
        if(time < 3 * 60)
        {
            return getMid(0, 3 * 60, time, 0, 6);
        }
        else if(time < 6 * 60)
        {
            return getMid(3 * 60, 6 * 30, time, 6, 22);
        }
        else if (time < 9 * 60)
        {
            return getMid(6 * 60, 9 * 60, time, 22, 60);
        }
        else
        {
            return getMid(9 * 60, 12 * 60, time, 60, 100);
        }
    }

    static private float getMid(float sx,float ex, float mx,float sy,float ey)
    {
        return (ey - sy) / (ex - sx) * (mx - sx) + sy;
    }

    void Start()
    {
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
    }
}
