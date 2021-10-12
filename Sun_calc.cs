using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Sun_calc : MonoBehaviour
{
    public Vector2 Loc = new Vector2(0, 0);//TODO: Set in config file

    public RectTransform SunTran;
    public Text Change, Day, Night;

    public int Hour = 0; //TODO: remove testing

    double Sunrise, Sunset;
    DateTime DT;
    double Scale = 155;

    //TODO: Calc each day
    //TODO: after sunset calc next sunrise
    //TODO: improve accuracy

    void Start()
    {
        DataSaver LocFile = new DataSaver("C:/Pass/Mainframe/Location.txt");
        //TODO: Save file location in config game object
        if (Loc.x != 0 && Loc.y != 0)
        {
            LocFile.Save(JsonUtility.ToJson(Loc));
            Debug.Log("Saved location to file");
        }
        else
        {
            string Stri = LocFile.Load();
            if (Stri == "There is no save data!")
            {
                Debug.LogError("No save data set location");
            }
            JsonUtility.FromJsonOverwrite(LocFile.Load(), Loc);
        }

        DT = System.DateTime.Now;
        (Sunrise, Sunset) = Calc(DT, 35.854030, -86.363530); //TODO: Save in file
        double H, M, S;
        string APM;

        (H, M, S) = DecToHMS(Sunrise);
        if (H > 12) { H -= 12; APM = "PM"; } else { APM = "AM"; }
        Day.text = String.Format("{2}\n{0:00}:{1:00}", H, M, APM);

        (H, M, S) = DecToHMS(Sunset);
        if (H > 12) { H -= 12; APM = "PM"; } else { APM = "AM"; }
        Night.text = String.Format("{0:00}:{1:00}\n{2}", H, M, APM);
    }

    private float nextActionTime = 0.0f;
    public float UpdateFreq = 30f;

    void Update()
    {
        DT = System.DateTime.Now;
        UpdateText();

        if (Time.time >= nextActionTime)
        {
            nextActionTime = Time.time + UpdateFreq;
            double SunPos = CalcSun();
            UpdateSun(SunPos);
        }
    }

    double CalcSun()
    {
        double DayTime = Sunset - Sunrise;
        double NightTime = 1 - DayTime;

        double Now = HMSToDec(DT.Hour, DT.Minute, DT.Second);

        double SunPos = (Now - Sunrise) / DayTime;

        if (Now > Sunset)
        {
            SunPos = 1 + ((Now - Sunset) / NightTime);
        }
        if (Now < Sunrise)
        {
            SunPos = 1 + ((Now + 1 - Sunset) / NightTime);
        }
        return SunPos;
    }

    void UpdateText()
    {
        double Now = HMSToDec(DT.Hour, DT.Minute, DT.Second);
        double Delta;
        String Stri = "Sunrise";

        if (Now < Sunrise)
        {
            Delta = Sunrise - Now;
        }
        else if (Now > Sunset)
        {
            Delta = Sunrise + (1 - Now);
        }
        else
        {
            Stri = "Sunset";
            Delta = Sunset - Now;
        }
        (double H, double M, double S) = DecToHMS(Delta);
        Change.text = String.Format("{3} in\n{0:00}:{1:00}:{2:00}", H, M, S, Stri);
    }
    void UpdateSun(double SunPos)
    {
        double SunRad = RADIANS(SunPos * 180);

        double x = Scale * -Math.Cos(SunRad);
        double y = Scale * Math.Sin(SunRad);
        Vector2 p = new Vector2((float)x, (float)y);
        SunTran.localPosition = p;
    }

    /*void Load()
    {
        if (File.Exists(Application.persistentDataPath
                           + "/Config.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/Config.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            TimeZone = data.TimeZone;
            Lat = data.Lat;
            Long = data.Long;
            Debug.Log("Game data loaded!");
        }
    }*/

    /*void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/MySaveData.dat");
        SaveData data = new SaveData();
        data.savedInt = intToSave;
        data.savedFloat = floatToSave;
        data.savedBool = boolToSave;
        bf.Serialize(file, data);
        file.Close();
    }*/

    double JulianDay(DateTime DT)
    {
        return DT.ToOADate() + 2415018.5;
    }

    double JulianCent(double JulianDays)
    {
        return (JulianDays - 2451545) / 36525;
    }

    (double, double) Calc(DateTime Date, double Lat, double Long)
    {
        var utcNow = DT.ToUniversalTime();
        var TZ1 = DT - utcNow;
        double TZ = TZ1.Hours;

        double G = JulianCent(JulianDay(Date));
        double I = (280.46646 + G * (36000.76983 + G * 0.0003032) % 360);
        double J = 357.52911 + G * (35999.05029 - 0.0001537 * G);
        double K = 0.016708634 - G * (0.000042037 + 0.0000001267 * G);
        double L = Math.Sin(RADIANS(J)) * (1.914602 - G * (0.004817 + 0.000014 * G))
          + Math.Sin(RADIANS(2 * J)) * (0.019993 - 0.000101 * G) + Math.Sin(RADIANS(3 * J)) * 0.000289;
        double M = I + L;
        double N = J + L;
        double O = (1.000001018 * (1 - K * K)) / (1 + K * Math.Cos(RADIANS(N)));
        double P = M - 0.00569 - 0.00478 * Math.Sin(RADIANS(125.04 - 1934.136 * G));
        double Q = 23 + (26 + ((21.448 - G * (46.815 + G * (0.00059 - G * 0.001813)))) / 60) / 60;
        double R = Q + 0.00256 * Math.Cos(RADIANS(125.04 - 1934.136 * G));
        double S = DEGREES(Math.Atan2(Math.Cos(RADIANS(P)), Math.Cos(RADIANS(R)) * Math.Sin(RADIANS(P))));
        double T = DEGREES(Math.Asin(Math.Sin(RADIANS(R)) * Math.Sin(RADIANS(P))));
        double U = Math.Tan(RADIANS(R / 2)) * Math.Tan(RADIANS(R / 2));
        double V = 4 * DEGREES(U * Math.Sin(2 * RADIANS(I)) - 2 * K * Math.Sin(RADIANS(J)) + 4 * K * U *
        Math.Sin(RADIANS(J)) * Math.Cos(2 * RADIANS(I)) - 0.5 * U * U * Math.Sin(4 * RADIANS(I)) - 1.25 * K *
        K * Math.Sin(2 * RADIANS(J)));
        double W = DEGREES(Math.Acos(Math.Cos(RADIANS(90.833)) / (Math.Cos(RADIANS(Lat)) * Math.Cos(RADIANS(T))) -
        Math.Tan(RADIANS(Lat)) * Math.Tan(RADIANS(T))));
        double X = (720 - 4 * Long - V + TZ * 60) / 1440;

        double Sunrise = (X * 1440 - W * 4) / 1440;
        double Sunset = (X * 1440 + W * 4) / 1440;
        double LightDur = 8 * W;

        return (Sunrise, Sunset);
    }

    (int, int, int) DecToHMS(double Dec)
    {
        int H = (int)Math.Floor(Dec * 24);
        Dec -= (double)H / 24;
        int M = (int)Math.Floor(Dec * 60 * 24);
        Dec -= (double)M / (60 * 24);
        int S = (int)Math.Floor(Dec * 60 * 60 * 24);

        return (H, M, S);
    }

    double HMSToDec(int H, int M, int S)
    {
        return (double)H / 24 + (double)M / (24 * 60) + (double)S / (24 * 60 * 60);
    }

    double RADIANS(double angle)
    {
        return angle * (Math.PI / 180);
    }

    double DEGREES(double rad)
    {
        return rad * (180 / Math.PI);
    }


}
