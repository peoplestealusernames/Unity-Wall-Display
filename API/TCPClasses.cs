using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Request
{
    public string path;
    public string method;
    public string data = "";
    public bool save = false;

    public Request() { }
    public Request(string savedData)
    {
        this.Load(savedData);
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
    public void Load(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}

public class Host
{
    public string ip;
    public int port;

    public Host() { }
    public Host(string savedData)
    {
        this.Load(savedData);
    }


    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
    public void Load(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}