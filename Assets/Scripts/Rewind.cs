using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour
{
    // The amount of time it takes to rewind
    [SerializeField] private float maxRewindDuration = 5f;
    // How fast we want to rewind
    //[SerializeField] private float rewindSpeed = 2f;
    // Is the PLAYER currently rewinding
    private bool isRewinding = false;
    public bool IsRewinding{
        get {return isRewinding;}
    }
    // Is the GAME currently rewinding
    public static bool GameIsRewinding = false;

    // Store player data for
    private List<TimeSnapshot> timeSnapshots = new List<TimeSnapshot>();

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRewinding)
        { 
            GameIsRewinding = true;
            RewindTime(); 
        }else{ 
            GameIsRewinding = false;
            RecordSnapshot(); 
        }
    }

    private void RecordSnapshot()
    {
        // Record player information
        // TODO: Need to add player current score and item data.
        timeSnapshots.Insert(0, new TimeSnapshot(transform.position, transform.rotation));

        // Remove time data that exceeds max
        if (timeSnapshots.Count > Mathf.Round(maxRewindDuration / Time.fixedDeltaTime))
        {
            timeSnapshots.RemoveAt(timeSnapshots.Count - 1);
        }
    }

     private void RewindTime()
    {
        if (timeSnapshots.Count > 0)
        {
            TimeSnapshot tempShot = timeSnapshots[0];
            transform.position = tempShot.Pos;
            transform.rotation = tempShot.Rot;

            // remove used snapshots
            timeSnapshots.RemoveAt(0);
        }
        else
        {
          StopRewind();  
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        GameHUDManager.Instance.ChangeRemoteHUD("Play");
        isRewinding = false;
    }
}
