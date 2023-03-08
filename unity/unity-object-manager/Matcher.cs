using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Globals
{
    public const double spawnRate = 0.12;
}

public class Constructs
{
    public List<DateTime> Time { get; set; }
    public int Direction { get; set; }

    public Constructs(DateTime time, int direction)
    {
        Time = new List<DateTime>()
        {
            time.AddSeconds(Globals.spawnRate/2),
            time.AddSeconds(-Globals.spawnRate/2)
        };
        Direction = direction;
    }
}

public class Matcher : MonoBehaviour
{
    private List<Constructs> objects;

    // Start is called before the first frame update
    void Start()
    {
        // 0 = up, 1 = down, 2 = left, 3 = right
        objects = new List<Constructs>()
        {
            new Constructs(DateTime.Parse("16:36:33.09"), 1),
            new Constructs(DateTime.Parse("16:36:34.30"), 3)
        };

        Debug.Log("HERE");

        // put in update later
        var lines = System.IO.File.ReadAllLines(@"/Users/warren_wallis/Documents/GitHub/ChristmasGifts2022/My project/Assets/data.txt");
        var line_data = new List<Constructs>();

        foreach (var line in lines)
        {
            string[] data = line.Split(',');
            string time = data[0].Split(' ')[1];
            
            if (data[1] != "null")
                line_data.Add(new Constructs(DateTime.Parse(time.Substring(0, time.Length-1)), Int32.Parse(data[1])));
        }

        Debug.Log("OBJECTS");
        foreach (var o in objects)
            Debug.Log($"Min Time: {o.Time[0].ToString("hh:mm:ss.fff")}, Max Time: {o.Time[1].ToString("hh:mm:ss.fff")}, Direction: {o.Direction}");

        Debug.Log("LINE DATA");
        foreach (var o in line_data)
            Debug.Log($"Min Time: {o.Time[0].ToString("hh:mm:ss.fff")}, Max Time: {o.Time[1].ToString("hh:mm:ss.fff")}, Direction: {o.Direction}");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
