using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Globals
{
    public const double spawnRate = 0.13;   // should be larger than minimum time between any two notes
}

public class Constructs
{
    public List<DateTime> Time { get; set; }
    public string Reason { get; set;}
    public string DataAgainst { get; set; } // the datapoint that made the construct invalid
    public int Direction { get; set; }
    public int Count { get; set; }
    public bool Consider { get; set;}


    public Constructs(DateTime time, int direction, int count = -1)
    {
        Time = new List<DateTime>()
        {
            time.AddSeconds(-Globals.spawnRate/2), // min time
            time.AddSeconds(Globals.spawnRate/2) // max time
        };
        Reason = "";
        DataAgainst = "";
        Direction = direction;
        Count = count;
        Consider = true;
    }
}

public class Matcher
{
    private List<Constructs> objects;
    private bool testMode;
    private string outputPath;


    //Add this to store data index
    public List<int> matchedDataIndexes;
    //add this for a too slow comment
    public string comment { get; set; }
    
    public List<int> missMatch { get; set; }

    // Start is called before the first frame update
    public Matcher()
    {
        testMode = false; // set to True to test module
        outputPath = "C:/Users/mvand/Documents/ECE180DA/ECE180D-Project/MediaPipe-Script/gesturefile.txt"; // change this to the correct output file
        objects = new List<Constructs>(){ new Constructs(DateTime.Parse("16:36:33.09"), 1) }; // create a seed to keep member variable alive
        missMatch = new List<int>(){ 0, 0 };
        matchedDataIndexes = new List<int>();
        comment ="";

        // 0 = null, 1 = up, 2 = down, 3 = left, 4 = right
        if (testMode)
        {
            objects = new List<Constructs>()
            {
                new Constructs(DateTime.Parse("16:36:33.09"), 1, 0),
                new Constructs(DateTime.Parse("16:36:33.69"), 0, 1),
                new Constructs(DateTime.Parse("16:36:34.30"), 3, 2)
            };

            moduleTest();
        }
    }

    //added this to keep only 10 indexes
    public void addIndex(int ind)
    {
        if(matchedDataIndexes.Count<10)
        {
            matchedDataIndexes.Add(ind);
        }
        else
        {
            matchedDataIndexes.Remove(matchedDataIndexes[0]);
            matchedDataIndexes.Add(ind);
        }
    }


    // Update is called once per frame
    public void Update()
    {
        if (!testMode)
        {
            missMatch = Match();
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

    public void printObjects(List<Constructs> objects) 
    {
        foreach (var o in objects)
            Debug.Log($"Min Time: {o.Time[0].ToString("hh:mm:ss.fff")}, Max Time: {o.Time[1].ToString("hh:mm:ss.fff")}, Direction: {o.Direction}");
    }

    // use to add objects from another file
    public bool addObject(DateTime time, int direction, int count)
    {
        Constructs curr = new Constructs(time, direction);

        if ((objects[objects.Count - 1].Time[0] == curr.Time[0] && objects[objects.Count - 1].Time[1] == curr.Time[1]) || // data.min == obj.min <= data.max == obj.max
            (curr.Time[0] <= objects[objects.Count - 1].Time[1] && objects[objects.Count - 1].Time[1] <= curr.Time[1]) || // data.min <= obj.max <= data.max
            (curr.Time[0] <= objects[objects.Count - 1].Time[0] && objects[objects.Count - 1].Time[0] <= curr.Time[1])) // data.min <= obj.min <= data.max
        {
            return false;
        }

        objects.Add(curr);
        Debug.Log("Added Object");
        return true;
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
        Debug.Log("ObjectsList");
        printObjects(objects);
        Debug.Log("MediaPipe");
        printObjects(line_data);
        bool match =false;
        comment ="";
        foreach (var line in line_data)
        {
            match=false;
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

                objects[object_index].Reason = "Matched";
                objects[object_index].DataAgainst = $"Min: {line.Time[0]}, Max: {line.Time[1]}, Count: {line.Count}";
                objects[object_index].Consider = false;
                object_index++;
                matched++;
                match=true;
                
            }
            if (object_index >= objects.Count)
                break;
            if(objects[object_index].Direction == line.Direction&&!match)
            {
                DateTime gestureTime=line.Time[0].AddSeconds(Globals.spawnRate/2);
                DateTime targetTime=objects[object_index].Time[0].AddSeconds(Globals.spawnRate/2);
                DateTime min = targetTime.AddSeconds(-0.5);
                DateTime max = targetTime.AddSeconds(0.5);
                if(gestureTime>= min && gestureTime <=max)
                {
                    objects[object_index].Reason = "Matched";
                    objects[object_index].DataAgainst = $"Min: {line.Time[0]}, Max: {line.Time[1]}, Count: {line.Count}";
                    objects[object_index].Consider = false;
                    object_index++;
                    matched++;
                    match=true;



                    if(gestureTime<targetTime)
                    {
                        //Debug.Log("Too Early!");
                        //comment ="Too Early!";
                    }
                    else if(gestureTime>targetTime)
                    {
                        //Debug.Log("Too Late");
                        //omment ="Too Slow!";
                    }
                }
            }
            else if (line.Time[1] < objects[object_index].Time[0]) // stream is behind time of current object
            {
                if (testMode)
                    Debug.Log("Continuing");
                Debug.Log("Continuing");
                continue;
            }
            else// set current object to be deleted because current stream is past the time of current object
            {
                if (testMode)
                    Debug.Log("Not Matched");

                Debug.Log("Deleted Object");
                objects[object_index].Reason = "Not Matched";
                objects[object_index].DataAgainst = $"Min: {line.Time[0]}, Max: {line.Time[1]}, Count: {line.Count}";
                objects[object_index].Consider = false;
                object_index++;
                missed++;
            }

        //     DateTime gestureTime=line.Time[0].AddSeconds(Globals.spawnRate/2);
        //     if (object_index >= objects.Count)
        //         break;
        //     if(gestureTime>=objects[object_index].Time[0]&&gestureTime<=objects[object_index].Time[1]&&!(matchedDataIndexes.Contains(objects[object_index].Count))&&line.Direction==objects[object_index].Direction)
        //     {
        //         objects[object_index].Reason = "Matched";
        //         objects[object_index].DataAgainst = $"Min: {line.Time[0]}, Max: {line.Time[1]}, Count: {line.Count}";
        //         objects[object_index].Consider = false;
        //         object_index++;
        //         matched++;

        //         addIndex(objects[object_index].Count);
        //     }
        //     else if(gestureTime<=objects[object_index].Time[0]|| (gestureTime>=objects[object_index].Time[0]&&gestureTime<=objects[object_index].Time[1]))
        //     {
        //          if (testMode)
        //             Debug.Log("Continuing");
        //         Debug.Log("Continuing");
        //         continue;
        //     }
        //     else
        //     {
        //         Debug.Log($"GestureDectionTime: {gestureTime}, ObjectMin:{objects[object_index].Time[0]}, ObjectMax:{objects[object_index].Time[1]}");
        //         Debug.Log("Deleted Object");
        //         objects[object_index].Reason = "Not Matched";
        //         objects[object_index].DataAgainst = $"Min: {line.Time[0]}, Max: {line.Time[1]}, Count: {line.Count}";
        //         objects[object_index].Consider = false;
        //         object_index++;
        //         missed++;
        //     }
        // }
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
            
            //we should't add nulls, they are mostly pointless?
            if (data[1] != "null")
                line_data.Add(new Constructs(DateTime.Parse(time.Substring(0, time.Length-1)), Int32.Parse(data[1]), Int32.Parse(data[4])));
            else
                line_data.Add(new Constructs(DateTime.Parse(time.Substring(0, time.Length-1)), 0,Int32.Parse(data[4])));
        }

        return line_data;
    }
}

public class Scorer
{
    private Matcher match;
    public int total { get; set; }
    public int score { get; set; }
    public int count { get; set; }

    // Start is called before the first frame update
    public Scorer()
    {
        match = new Matcher();
        total = 0;
        score = 0;
        count = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        match.Update();
        score += match.missMatch[1];
    }

    public string getComment()
    {
        return match.comment;
    }
    public bool addObject(DateTime time, int direction) {
        bool output = match.addObject(time, direction, count);
        total++;
        count++;

        return output;
    }
}