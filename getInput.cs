using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class getInput : MonoBehaviour
{
    //public Spawner myspawner;
    public int playerScore = 0;
    private Transform objectTransform;
    Quaternion objectRotation;
    Vector3 objectPosition;
    string swipeDirection = "none";

    public AudioSource audioSource;
    public int possiblePoints = 0;

    private Scorer score;
    public string comment= "";

    void Start()
    {
        //audioSource = GameObject.Find("AudioObject").GetComponent<AudioSource>();
        score = new Scorer();
        Debug.Log("Created Scorer");
        comment="";
    }

    void OnTriggerEnter (Collider other)
    {
        comment=score.getComment();
        possiblePoints++;
        //Debug.Log($"Object Entered");
        objectTransform = other.transform;
        objectRotation = objectTransform.rotation;
        objectPosition = objectTransform.position;

        //changed this to -1 so objects with 0 rotation dont get matched to null
        int temp_direction = -1;
        if (objectTransform.rotation == Quaternion.Euler(0, 0, 0))
        {
            temp_direction =2;
        } else if(objectTransform.rotation ==Quaternion.Euler(0, 0, 90))
        {
            temp_direction = 4;
        } else if(objectTransform.rotation ==Quaternion.Euler(0, 0, 270))
        {
            temp_direction = 3;
        } else if(objectTransform.rotation ==Quaternion.Euler(0, 0, 180))
        {
            temp_direction = 1;
        } 
        Debug.Log(DateTime.Now);
        score.addObject(DateTime.Now.AddSeconds(0.25), temp_direction);
        

        //Debug.Log("Object entered the trigger");
        //Debug.Log("Score: " + playerScore);
    
        // Get object orientation
        //transform.rotation = other.transform.Quanternion.identity();

        //objectTransform = other.GetComponent<Transform>();    // this gets the prefabs default transform
        //objectTransform = other.transform;
        //objectTransform.GetPositionAndRotation(out objectPosition, out objectRotation);
        // objectRotation = objectTransform.localRotation;  // this gets the prefabs default rotation
        //objectRotation = objectTransform.rotation;
        //objectPosition = objectTransform.position;
    }

    void OnTriggerStay (Collider other)
    {
        //Debug.Log("Object is within trigger");
        comment=score.getComment();
        // Get user input
        //Debug.Log(InputReader.currentActionMap);
        //string swipeDirection = "left"; // not always true
        // string swipeDirection = Input.GetKey();
        
        //score.update();

        //score.update();

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("You swiped left!");
            swipeDirection = "left";
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("You swiped right!");
            swipeDirection = "right";
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("You swiped up!");
            swipeDirection = "up";
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("You swiped down!");
            swipeDirection = "down";
        }

        // Update the player score display
        //Debug.Log("Player Score: " + playerScore);
    }

    void OnTriggerExit (Collider other)
    {
        score.Update();
        playerScore = score.score;
        comment=score.getComment();
        //Debug.Log("Object has exited the trigger box region.");
        if (other.CompareTag("targetPrefab"))
        {
            Destroy(other.gameObject);
            //targets.RemoveAt(targets.size() - 1);
        }

       


        // Check if user input matches current object objectRotation
        // 90 ==> swipe right, 270 ==> swipe left
        // 180 ==> up, 0 ==> down
        // if they match, increment playerScore
        // if (objectRotation == Quaternion.Euler(0, 0, 270))
        // {
        //     //Debug.Log("Target rotation: left");
        //     if (swipeDirection == "left")
        //     {
                
        //         playerScore++;
        //         audioSource.Play();
        //     }
        // }
        // else if (objectRotation == Quaternion.Euler(0, 0, 90))
        // {
        //     //Debug.Log("Target rotation: right");
        //     if (swipeDirection == "right")
        //     {
                
        //         playerScore++;
        //         audioSource.Play();
        //     }
        // }
        // else if (objectRotation == Quaternion.Euler(0, 0, 180))
        // {
        //     //Debug.Log("Target rotation: up");
        //     if (swipeDirection == "up")
        //     {
                
        //         playerScore++;
        //         audioSource.Play();
        //     }
        // }
        // else if (objectRotation == Quaternion.Euler(0, 0, 0))
        // {
        //     //Debug.Log("Target rotation: down");
        //     if (swipeDirection == "down")
        //     {
                
        //         playerScore++;
        //         audioSource.Play();
        //     }
        // }

        // Reset the swipeDirection field
        swipeDirection = "none";

        // Print the current score
        //Debug.Log("Player Score: " + playerScore);

    }
}
