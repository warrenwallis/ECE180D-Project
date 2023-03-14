using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scorer : MonoBehaviour
{
    private Matcher match;
    public int total { get; set; }
    public int score { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        match = new Matcher();
        total = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score += match.missMatch[1];
    }

    public void addObject(DateTime time, int direction) {
        match.addObject(time, direction);
        total++;
    }
}
