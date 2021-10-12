using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Time_Events : MonoBehaviour
{
    DateTime DT, UpdateDT;
    //TODO: Revamp with eventclass

    public Events Script;

    void Start()
    {
        DT = System.DateTime.Now;
        UpdateDT = new DateTime(DT.Year, DT.Month, DT.Day, DT.Hour, DT.Minute - 1, 0);
    }

    void Update()
    {
        DT = System.DateTime.Now;
        if (DT >= UpdateDT)
        {
            UpdateDT += new TimeSpan(0, 1, 0);
            EveryMin(UpdateDT - new TimeSpan(0, 1, 0));
        }
    }

    void EveryMin(DateTime D8)
    {
        Script.CallEvent("Min", D8 as object);
        D8.ToString("MM/dd/yyyy\nHH:MM");
    }
}
