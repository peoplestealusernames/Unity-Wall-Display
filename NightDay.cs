using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightDay : MonoBehaviour
{
    public Events Script;

    public List<GameObject> Day;
    public List<GameObject> Night;

    void Start()
    {
        Script.AddEvent("SleepOn", On);
        Script.AddEvent("SleepOff", Off);
    }

    void On(object Nil)
    {
        Change(Day, Night);
    }

    void Off(object Nil)
    {
        Change(Night, Day);
    }

    void Change(List<GameObject> On, List<GameObject> Off)
    {
        for (int i = 0; i < On.Count; i++)
            On[i].SetActive(true);

        for (int i = 0; i < Off.Count; i++)
            Off[i].SetActive(false);
    }
}
