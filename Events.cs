using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class EventCaller
{
    public string Name;
    public object Arg;

    public EventCaller(string Name, object Arg)
    {
        this.Name = Name;
        this.Arg = Arg;
    }
}

public class Events : MonoBehaviour
{
    //TODO: Remove and add to time_events
    private Dictionary<string, List<System.Action<object>>> EventsTab =
    new Dictionary<string, List<System.Action<object>>>();

    List<EventCaller> ToCall = new List<EventCaller>();

    public void CallOnMain(string Name, object Arg)
    {
        lock (ToCall)
        {
            ToCall.Add(new EventCaller(Name, Arg));
        }
    }

    void Update()
    {
        lock (ToCall)
        {
            foreach (EventCaller e in ToCall)
            {
                CallEvent(e.Name, e.Arg);
            }
            ToCall = new List<EventCaller>();
        }
    }

    public void AddEvent(string Name, System.Action<object> FNC)
    {
        if (EventsTab.ContainsKey(Name))
        {
            EventsTab[Name].Add(FNC);
        }
        else
        {
            EventsTab[Name] = new List<System.Action<object>>();
            EventsTab[Name].Add(FNC);
        }
    }

    public void AddEventDateTime(DateTime D8, System.Action<object> FNC)
    {
        string Name = D8.ToString("MM/dd/yyyy\nHH:MM");
        AddEvent(Name, FNC);
    }

    public void CallEvent(string Name, object Arg)
    {
        if (EventsTab.ContainsKey(Name))
        {
            for (int i = 0; i < EventsTab[Name].Count; i++)
            {
                EventsTab[Name][i](Arg);
            }
        }
    }

    public void Start()
    {
        /*AddEvent("Test", Debug.Log);
        CallEvent("Test", "Aye" as object);*/
    }
}
