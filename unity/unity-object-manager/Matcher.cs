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
    public bool Consider { get; set;}


    public Constructs(DateTime time, int direction)
    {
        Time = new List<DateTime>()
        {
            time.AddSeconds(-Globals.spawnRate/2), // min time
            time.AddSeconds(Globals.spawnRate/2) // max time
        };
        Direction = direction;
        Consider = true;
    }
}

public class Matcher : MonoBehaviour
{
    private List<Constructs> objects;
    private bool testMode;
    private string outputPath;
    
    public List<int> missMatch { get; set; };

    // Start is called before the first frame update
    void Start()
    {
        testMode = false; // set to True to test module
        outputPath = "/Users/warren_wallis/Documents/GitHub/ChristmasGifts2022/My project/Assets/data.txt"; // change this to the correct output file
        objects = new List<Constructs>(){ new Constructs(DateTime.Parse("16:36:33.09"), 1) }; // create a seed to keep member variable alive
        missMatch = new List<int>(){ 0, 0 };

        // 0 = null, 1 = up, 2 = down, 3 = left, 4 = right
        if (testMode)
        {
            objects = new List<Constructs>()
            {
                new Constructs(DateTime.Parse("16:36:33.09"), 1),
                new Constructs(DateTime.Parse("16:36:33.69"), 0),
                new Constructs(DateTime.Parse("16:36:34.30"), 3)
            };

            moduleTest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!testMode)
        {
            List<int> missMatch = Match();
            DeleteConsideredObjects();
        }
    }

    void moduleTest()
    {
        Debug.Log("Curerent Objects");
        printObjects(objects);
        
        List<int> missMatch = Match();

        Debug.Log($"MISSED: {missMatch[0]}, MATCHED: {missMatch[1]}");

        DeleteConsideredObjects();

        Debug.Log("OBJECTS AFTER DELETE");
        printObjects(objects);
    }

    void printObjects(List<Constructs> objects) 
    {
        foreach (var o in objects)
            Debug.Log($"Min Time: {o.Time[0].ToString("hh:mm:ss.fff")}, Max Time: {o.Time[1].ToString("hh:mm:ss.fff")}, Direction: {o.Direction}");
    }

    // use to add objects from another file
    public void addObject(DateTime time, int direction)
    {
        objects.Add(new Constructs(time, direction));
    }

    // deletes all objects that have been considered
    void DeleteConsideredObjects() 
    {
        objects.RemoveAll(o => !o.Consider);
    }

    // matches objects with streamed data
    List<int> Match()
    {
        var line_data = createList();
        int object_index = 1, missed = 0, matched = 0;

        foreach (var line in line_data)
        {
            if (testMode)
            {
                Debug.Log($"Current Index: {object_index}");
                Debug.Log($"Line Data: {line.Time[0]}, {line.Time[1]}, {line.Direction}");
            }
            
            if (object_index >= objects.Count)
                break;
            if ((
                    (objects[object_index].Time[0] == line.Time[0] && objects[object_index].Time[1] == line.Time[1]) || // data.min == obj.min <= data.max == obj.max
                    (line.Time[0] <= objects[object_index].Time[1] && objects[object_index].Time[1] <= line.Time[1]) || // data.min <= obj.max <= data.max
                    (line.Time[0] <= objects[object_index].Time[0] && objects[object_index].Time[0] <= line.Time[1]) // data.min <= obj.min <= data.max
                ) && objects[object_index].Direction == line.Direction) 
            {
                if (testMode)
                    Debug.Log("Matched");

                objects[object_index].Consider = false;
                object_index++;
                matched++;
            } 
            else if (line.Time[1] < objects[object_index].Time[0]) // stream is behind time of current object
            {
                if (testMode)
                    Debug.Log("Continuing");

                continue;
            }
            else// set current object to be deleted because current stream is past the time of current object
            {
                if (testMode)
                    Debug.Log("Not Matched");

                objects[object_index].Consider = false;
                object_index++;
                missed++;
            }
        }

        return new List<int>(){ missed, matched };
    }

    // gets data stream and parses it to be used by Match Function
    List<Constructs> createList()
    {
        var lines = System.IO.File.ReadAllLines(@outputPath);
        var line_data = new List<Constructs>();

        foreach (var line in lines)
        {
            string[] data = line.Split(',');
            string time = data[0].Split(' ')[1]; // get time
            
            if (data[1] != "null")
                line_data.Add(new Constructs(DateTime.Parse(time.Substring(0, time.Length-1)), Int32.Parse(data[1])));
            else
                line_data.Add(new Constructs(DateTime.Parse(time.Substring(0, time.Length-1)), 0));
        }

        return line_data;
    }
}
