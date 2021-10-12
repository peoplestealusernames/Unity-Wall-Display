using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeUpdator : MonoBehaviour
{
    //public Text DateTime;
    public Text D8, HM, APM, Sec, D8S;
    public DateTime DT;

    void Update()
    {
        //TODO: swap dates to daily
        //TODO: swap mins to update every min
        DT = System.DateTime.Now;
        //DateTime.text = DT.ToString("MM/dd/yyyy\nHH:MM\nss");
        D8.text = DT.ToString("MM/dd/yyyy");
        HM.text = DT.ToString("hh:mm");
        APM.text = DT.ToString("tt");
        Sec.text = DT.ToString("ss");
        D8S.text = DT.ToString("ddd MMM dd");
    }
}
