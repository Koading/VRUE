using UnityEngine;
using System.Collections;

public class RecordingManager : MonoBehaviour
{
    private ArrayList recordings;
    public bool playAll;

    // Use this for initialization
    void Start()
    {
        recordings = new ArrayList();
        playAll = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playAll)
        {
            foreach (Recording r in this.recordings)
            {
                r.Update();
            }
        }
    }

    public void ClearRecordings()
    {
        foreach (Recording r in this.recordings)
        {
            r.Stop();
        }
        recordings.Clear();
    }

    void OnClickPlayAll()
    {
        this.playAll = true;
    }

    void OnCickStopAll()
    {
        this.playAll = false;
        this.ClearRecordings();
    }
}