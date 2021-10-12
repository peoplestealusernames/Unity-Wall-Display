using UnityEngine;

public class Server : MonoBehaviour
{
    public Events Script;
    public HttpsReq HttpsHandle; //TODO: better implimentation

    public string URI = ""; //TODO: move to config game object for setting

    Connection Ser;

    void Start()
    {
        DataSaver URIFile = new DataSaver("C:/Pass/Mainframe/URI.txt");
        //TODO: Save file location in config game object
        if (URI != "")
        {
            URIFile.Save(URI);
            Debug.Log("Saved URI to file");
        }
        else
        {
            string Stri = URIFile.Load();
            if (Stri == "There is no save data!")
            {
                Debug.LogError("No save data set: URI");
            }
            URI = URIFile.Load();
        }

        HttpsHandle.GetRequest(URI, Connect);
    }

    void Connect(string Res)
    {
        Host URI = new Host(Res);
        Ser = new Connection(URI);
        Ser.Events.on("setup", Connected);
        Ser.Events.on("data", Data);
    }

    void Data(object rawmsg)
    {
        string msg = rawmsg as string;
        Debug.Log(msg);

        Request Req = new Request(msg);
        Debug.Log(Req.data);
        //TODO: swap to a DataBase event system like in node
        if (Req.path == "SleepScreen")
            if (Req.data == "true")
            {
                Script.CallOnMain("SleepOn", true as object);
            }
            else
            {
                Script.CallOnMain("SleepOff", false as object);
            }
    }

    void Connected(object N)
    {
        Debug.Log("Setup");
        Request Req = new Request();
        Req.method = "LISTEN";
        Req.path = "SleepScreen";
        Ser.write(Req.Save());
    }
}