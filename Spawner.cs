/// Spawner
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;    // for reading input from .txt files for song sequencing, gesture/speech recognition, and localization
using MenuLogicNamespace;   // to access Menu



// Declare this class elsewhere
public class Song
{
    public Song(string nm, int rate, float delay)
    {
        m_name = nm;
        bpm = rate;
        initialSpawnerDelay = delay;
        int winners_score = 0;  //for now
        if (winners_score >= highscore)
        {
            highscore = winners_score;
        } 

    }
    public Song(string nm, int rate)
    {
        m_name = nm;
        bpm = rate;
        initialSpawnerDelay = 0.0f;
    }
    public void loadScript(ref Song curSong)
    {
        //public getInput script;
        // Load song script into targets container and the time between notes into the timeBetweenEachNote container
        if (curSong.name() == "Crab Rave")
        {
            //startDelay = 0.2f;
            if (curSong.getDifficulty() == 1)
            {
                // CURRENT DIFFICULTY
                startDelay = 5.68f - 2.3f - 0.45f - 1.9f + 4f - 5.57f - 0.3f + 2f + 0.51f;
                
                Debug.Log("Diifculty " + curSong.getDifficulty() + " Start Delay " + startDelay);
            } else if (curSong.getDifficulty() == 2)
            {
                startDelay = 5.68f;
            } else
            {
                startDelay = 5.58f;  // for debugging
            }
            //startDelay = 5.68f; // 5.67f - 4.13f + 5.36f + 5.25f; // - 2.27f;
            // Hard-code the sequence for Crab Rave here or read with infile
            // char1 = position r/m/l
            // char2 = rotation u/d/l/r
            // char3 = time (ms) between last target spawn and current target spawn
            // code for new implementation + old
            /*
            //            new implementation       old implementation
            //     0 time: 5                        0
            //single note: 6........................1
            //  two notes: 7       
            //three notes: 8
            // four notes: 9........................5
            //   1/2 note: 1........................2
            //   1/4 note: 2........................3
            // 1/8th note: 3........................4
            // 1/16th note:4   
            //    triplet: 0
            */


            // 1 + 2 + 3 + 4 + 
            // for obstacle : lo_ or ro_
            //"ll6,rr6,ll6,rr6," + //ll6,rr6" + "ll6,rr6," + //ll6,rr6,ll6,rr6" +
            // string sequenceKey = "rd1,ld1,rd2,ru2,rd1," + "ld1,rd1,ld2,lu2,ld1," + "rr1,ll1,rd2,ru2,rd1," + "ll1,rr1,lld2,lu2,ld1," + 
            //     "ll1,mr1,ll2,lr2,ll1," + "rr1,ml1,rr2,rl2,rr1," + "ll1,rr1,ll2,lr2,ll1," + "rr1,ll1,rr2,rl2,rr1," +
            //     "rd1,ld1,lu2,ld2,lu1," + "ld1,rd1,ru2,rd2,ru1," + "ru1,lu1,ld2,lu2,ld1," + "lu1,ru1,rd2,ru2,ru1," + 
            //     "md1,mu1,md2,mu2,md1," + "ld1,lu1,ld2,lu2,ld1," + "rd1,ru1,rd2,ru2,rd1," +
            //     "md1,mu1,md2,mu2,md1," + "ld1,lu1,ld2,lu2,ld1," + "rd1,ru1,rd2,ru2,rd1," +
            //     "mu1,md1,md2,mu2,md1," + "lu1,ld1,ld2,lu2,ld1," +
            //     "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" + "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" +
            //     "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" + "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" +
            //     "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" + "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" +
            //     "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" + "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" +
            //     "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" + "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" + "ml1,rr2,ml3,rr2,ml2,ll7," +
            //     "md7,md7,md7,md7," + "md7,md7,md7,md7," +
            //     "rd1,ld1,rd2,ru2,rd1," + "ld1,rd1,ld2,lu2,ld1," + "rr1,ll1,rd2,ru2,rd1," + "ll1,rr1,lld2,lu2,ld1," +
            //     "ll1,mr1,ll2,lr2,ll1," + "rr1,ml1,rr2,rl2,rr1," + "ll1,rr1,ll2,lr2,ll1," + "rr1,ll1,rr2,rl2,rr1," +
            //     "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" + "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" +
            //     "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" + "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" +
            //     "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" + "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" +
            //     "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" + "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" +
            //     "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" + "ml1,rr2,ml3,rr2,ml2,ll2," + "mr1,ll2,mr3,ll2,mr2,rl2" +
            //     "mr1,ll2,mr3,ll2,mr2,rr2," + "ml1,rr2,ml3,rr2,ml2,lr2" + "rd2,mu2,ll2,rr2,rd2,ru3,ll2,rr2," + "ld2,mu2,rr2,ll2,ld2,lu3,rr2,ll2," +
            //     "md7,md7,md7,md7," +
            //     "###,";
            //test sequence
            string sequenceKey = "ml7,lr7,mu7,ld7,ll7,rl7,ml7,mr7,lu7,rd7,ll7,rl7,ml7,lr7,ru7,rd7,ml7,ll7,ml7,lr7,mu7,ld7,ll7,rl7,ml7,mr7,lu7,rd7,ll7,rl7,ml7,lr7,ru7,rd7,ml7,ll7"+"###,";

            curSong.setKey(sequenceKey);
        }
        else if (curSong.name() == "Matangi")
        {
            startDelay = 8f;
            string sequenceKey = "ll5,rr9,ll9,rr9,mu9,md9,mu9,md9,ll9,mr3,rr3,rl4,rr4,ml3,ll3,lr4,ll4,lr4,mr4" + "###,";
            curSong.setKey(sequenceKey);
        }
        else if (curSong.name() == "Other Song")
        {
            string sequenceKey = "mu0,md3,mu3,md3";
            curSong.setKey(sequenceKey);
        }
    }
    public void setKey(string seq)
    {
        seqKey = seq.ToLower();
    }
    public string getKey()
    {
        return seqKey;
    }
    public float getDelay()
    {
        return startDelay;
    }
    public float beat()
    {
        int i = 240;
        int ll = 80;
        if ((ll < bpm) && (bpm < i))
        {
            return (60f / bpm);
        }
        else { return 128.0f; }
    }
    public string name() { return m_name; }
    public void setDifficulty(int d)
    {
        m_difficulty = d;
    }
    public int getDifficulty()
    {
        return m_difficulty;
    }
    private string m_name;
    public int bpm;
    private float initialSpawnerDelay;
    //private int highscore;
    private string seqKey;  // encoded data for spawner
    public float startDelay;
    int m_difficulty;
    public static int highscore;
    public static string players_name;
    public static string opponents_name;
    public static int opponents_score;
    public int winners_score;
}

/*
public class Menu
{
    public void PausePlay()
    {
        // Check for user speech commands:
        // on_message("MusicNinja"..."
        // string msg = message.payload(2:-1);
        string msg = "start";   // for now;
        msg.ToLower();
        string confirm = "";

        // Move below code to Menu or UI 
        if (msg == "quit")
        {
            // pause the current game
            // display("Are you sure?);
            // display("Yes/No");
            // get user speech
            confirm = "yes"; // for now
            confirm.ToLower();
            if (confirm == "yes")
            {
                // Go to the Menu screen
            }
            else if (confirm == "no")
            {
                // resume
            }
        }
        else if (msg == "pause")
        {
            // pause the current game
            Time.timeScale = 0;
            Debug.Log("Set timescale to 0.");
            // display("Resume?")
            // display("Yes/No?)
            // get user speech
            confirm = "yes"; // for now
        }
        if (confirm == "yes")
        {
            // resume the game
            Time.timeScale = 1;
            //Debug.Log("Set timescale to 1.");
        }
        else if (confirm == "no")
        {
            // quit the game
        }
        else if (msg == "start")
        {

        }
    }
    public void displaySongList()
    {
        // write to on-screen display
        Debug.Log("Displaying Song List."); // for now
    }
    public void setSongChoice()
    {
        // get user speech
        songChoice = "Crab Rave";   // for now
    }
    public void setDifficulty()
    {
        // get user difficulty from speech recognition
        difficulty = "easy";   // for now
    }
    public int updateHighScores(Song song, int score)
    {
        return 0;
        // Write to local game file that stores an array of 10 x number of songs in song list
        // each song records the top 10 scores in a .txt file whenever a game is finished
    }
    private Song[] songList;
    //private List<Song> songList;
    private string songChoice;
    private string difficulty;
}
*/

public class Spawner : MonoBehaviour
{
    public float offset;
    //float startDelay = 7.1f;   // for now
    public GameObject prefab;
    public GameObject obstacle;
    public Transform[] points;
    //public float beat = (60f / 125f) * 2f;
    public float beat;  // = (60f / curSong.bpm()) * 2f;
    private float timer;
    public float speed;
    public float timeSinceLastSpawn = 0;
    bool delayInProgress = true;
    public int numTargetsSpawned = 0;

    // Declare containers for targets and their corresponding intervals
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> activeTargets = new List<GameObject>();
    public List<GameObject> obstacles = new List<GameObject>();
    public List<float> timeBetweenEachNote = new List<float>();
    public List<Vector3> startPoints = new List<Vector3>();
    public List<Vector3> endPoints = new List<Vector3>();

    public Vector3 startPosition = new Vector3(0, 1.6f, 50.0f);
    public Vector3 startPositionLeft = new Vector3(-0.8f, 1.6f, 50.0f);
    public Vector3 startPositionRight = new Vector3(0.8f, 1.6f, 50.0f);
    public Vector3 finalPosition = new Vector3(0, 1.6f, 1.9f);
    public Vector3 finalPositionLeft = new Vector3(-0.8f, 1.6f, 1.9f);
    public Vector3 finalPositionRight = new Vector3(0.8f, 1.6f, 1.9f);

    
    //public static string songChoice = "Crab Rave";    // for now
    public static string songChoice = "Crab Rave";
    public string sequenceKey = ""; // for now
    public Song curSong = new Song(songChoice, 125, 0.58f);
    //public Menu curMenu = new Menu();
    //MenuLogic curMenu;
    public int difficultySelected;

    public float duration = 162f; // length of Crab Rave in seconds: 163
    void Start()
    {
        offset=Time.time;
        Debug.Log("SPAWNER AWAKE ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        Invoke("LoadNextScene", duration);  // For switching to the EndScreen scene

        //Debug.Log("Spawner::Start_____________________________________________");
        // Get song choice using speech recognition

        //Debug.Log("Initializing Menu...");
        //curMenu = new MenuLogic();
        //curMenu.init();
        //Debug.Log("Menu Initialized.");
        // Pause execution from Menu screen

        /*
        while (curMenu.inMenuScreen)
        {
            Debug.Log("Waiting for game paramters.");
            //break;
        }
        */

        //curSong =     // for now
        //Song curSong = new Song(songChoice, 125);    // for now

        // load the sequence from file into the song's key

        // Get difficulty
        difficultySelected = 1; // for now
        curSong.setDifficulty(difficultySelected);


        curSong.loadScript(ref curSong);
        

        // Set speed
        if (difficultySelected == 1)
        {
            // easy//was 4.0f//for testing use .8
            speed = curSong.beat() / 8.0f;
        } else if (difficultySelected == 2)
        {
            // medium
            speed = curSong.beat() / 2f;
        } else if (difficultySelected == 3)
        {
            // hard mode
            speed = curSong.beat();
        } else if (difficultySelected == 4)
        {
            // super hard mode
            speed = (curSong.beat()* 2);
        }
        //beat = (60f / curSong.bpm) * 2f;
        beat = (60f / curSong.bpm);

        Debug.Log("curSong Key: " + curSong.getKey() + "++++++++++++++++++++++++++++++++++");
        // Decode the song script and populate the following containers: targets, timeBetweenEachNote, startPoints, endPoints
        decodeAndPopulate(curSong.name(), curSong.getKey(), curSong);

    // Delay execution to synchronize 1st target with the start of the song
    StartCoroutine(DelayedPause(curSong.getDelay())); // Call the DelayedPause coroutine with a startDelay second delay
    }


    void Update()
    {
        //Debug.Log("Targets count: " + targets.Count);
        // Pause?
        //curMenu.PausePlay();    // Check for start/pause/resume/quit
        

        // wait for song delay
        if (!delayInProgress)
        {
            //Debug.Log("Waiting to send first target...");
            // max targets container to 99 items
            if (targets.Count > 999)
            {
                Debug.Log("targets.Count countainer limit reached.");
                targets.RemoveAt(targets.Count - 1);
            }

            timeSinceLastSpawn += Time.deltaTime;
            if (numTargetsSpawned < targets.Count && timeSinceLastSpawn >= timeBetweenEachNote[numTargetsSpawned])
            {
                //foreach (var obstacle in obstacles)
                //{
                 
                //}
                GameObject t = targets[numTargetsSpawned];
                activeTargets.Add(t);
                timeSinceLastSpawn = 0;
                numTargetsSpawned++;
            }
            move(numTargetsSpawned); // spawn a new target and move all existing targets forward a bit
            
        }
    }

    // Utility Functions
    IEnumerator DelayedPause(float delay)
    {
        yield return new WaitForSeconds(delay);
        delayInProgress = false;
    }

    void decodeAndPopulate(string songName, string sequence, Song curSong)
    {
        //Debug.Log("Entered dap function. _-------------------------------------------------------");
        //Debug.Log("songName is " + songName);
        // Transcribe the sequence key and populate the "targets" and "timeBetweenEachNote" containers appropriately
        if (songName != "" && sequence != "")
        {
            Debug.Log("if statement 1.");
            
            
            //Debug.Log("SEQ: " + sequence);
            string sequenceKey = sequence;
            //Debug.Log("SequenceKey length" + sequenceKey.Length);
            //Debug.Log("SeqKey: " + sequenceKey + "+++++++++++++++++++++++++++++++++++++++++++++++");
            // Clear containers
            targets.Clear();
            timeBetweenEachNote.Clear();
            startPoints.Clear();
            endPoints.Clear();

            // declare temporary variables for position, rotation and interval
            Quaternion rotation = Quaternion.Euler(0, 0, 180);  // default
            Vector3 position = startPosition;   // default
            Vector3 positionFinal = finalPosition;  // default
            //float interval = quarterNote;   // default
            float interval = curSong.beat();   // default

            for (int i = 0; i < sequenceKey.Length - 3; i++)
            {
                // set position variable
                if (sequenceKey[i + 3] == ',')
                {
                    if (sequenceKey[i] == 'l')
                    {
                        position = startPositionLeft;
                        positionFinal = finalPositionLeft;
                    }
                    else if (sequenceKey[i] == 'r')
                    {
                        position = startPositionRight;
                        positionFinal = finalPositionRight;
                    }
                    else if (sequenceKey[i] == 'm')
                    {
                        position = startPosition;   // middle
                        positionFinal = finalPosition;
                    }
                }
                // set rotation variable
                else if (sequenceKey[i + 2] == ',')
                {
                    // set rotation variable
                    if (sequenceKey[i] == 'l')
                    {
                        rotation = Quaternion.Euler(0, 0, 270);
                    }
                    else if (sequenceKey[i] == 'r')
                    {
                        rotation = Quaternion.Euler(0, 0, 90);
                    }
                    else if (sequenceKey[i] == 'u')
                    {
                        rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else if (sequenceKey[i] == 'd')
                    {
                        rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (sequenceKey[i] == 'o')
                    {
                        //GameObject ob = Instantiate(obstacle, position, Quaternion.identity);
                        //obstacles.Add(ob);
                        Debug.Log("Add obstacle here");
                    }
                }
                // set time since previous note
                else if (sequenceKey[i + 1] == ',')
                {
                    int c = sequenceKey[i] - '0';
                    // set the temporary interval variable appropriately for the next target instantiation
                    if ((0 < c) && (c <= 4))
                    {
                        // (60 / bpm) / ( 2 ^ i )
                        interval = curSong.beat() / Mathf.Pow(2, c);
                        interval *= 2f;
                    }
                    else if ((4 < c) && (c <= 9))
                    {
                        interval = curSong.beat() * (c - 5);
                        interval *= 2f;
                    }
                    else if (c == 0)
                    {
                        interval = curSong.beat() / 3.0f;  // triplet
                        interval *= 2f;
                    }
                    
                }
                // Populate containers w/ objects + data
                else if (sequenceKey[i] == ',')
                {
                    //Debug.Log("interval cur value: " + interval);
                    // Instantiate a prefab target game object with the temporary fields
                    GameObject target = Instantiate(prefab, position, rotation);

                    // Add the game object to the "targets" container
                    targets.Add(target);
                    

                    // Update the "startPoints" and "endPoints" containers
                    startPoints.Add(position);
                    endPoints.Add(positionFinal);

                    // Update the "timeBetweenEachNote" container by adding the current interval to the end of the list
                    timeBetweenEachNote.Add(interval);
                    continue;
                }

            }
        }
    }
    
    void move(int numTargets)
    {
        //float elapsedTime = Time.time - startDelay;
        float elapsedTime = Time.time - curSong.getDelay()-offset;
        //Debug.Log(numTargets);
        for (int i = 0; i < numTargets; i++) // add time condition
        {
            
            if (targets[i] != null) // memory leak atm? (targets destroyed but container size not decremented)
            {
                GameObject target = activeTargets[i];
                Vector3 currentPosition = target.transform.position;
                //Debug.Log("Moving target" + i); 
                // obstacles
                /*
                int f;
                int m = obstacles.Count;
                if (i < m)
                {
                    f = m-1;
                } else
                {
                    f = i;
                }
                GameObject obstacle = obstacles[f];
                Vector3 obstacleCurrentPosition = obstacle.transform.position;
                Vector3 obstacleStartPoint = startPoints[i];
                Vector3 obstacleEndPoint = endPoints[i];
                */

                Vector3 startPoint = startPoints[i];
                Vector3 endPoint = endPoints[i];

                

                currentPosition = startPoint + (endPoint - startPoint) * ((elapsedTime - timeSinceFirstSpawn(i)) * speed);
                target.transform.position = currentPosition;

                //obstacleCurrentPosition = obstacleStartPoint + (obstacleEndPoint - obstacleStartPoint) * ((elapsedTime - timeSinceFirstSpawn(i)) * speed);
                //obstacle.transform.position = obstacleCurrentPosition;

            }
        }
    }
    float timeSinceFirstSpawn(int n)
    {
        float timeSinceFirstSpawn = 0.0f;
        for (int j = 0; j < n; j++)
        {
            
            timeSinceFirstSpawn += timeBetweenEachNote[j];
        }
        return timeSinceFirstSpawn;

    }

    void LoadNextScene()
    {
        //score = getInput.playerscore;
        SceneManager.LoadScene("EndScreen");
    }


}







