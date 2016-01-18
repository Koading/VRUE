using UnityEngine;
using System.Collections;

public class script_RecordingManager : MonoBehaviour
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

    public void OnClickPlayAll()
    {
        this.playAll = true;

        foreach(Recording r in this.recordings)
        {
            r.PlayRecording();
        }
    }

    public void OnClickRecord()
    {
        GameObject instrumentPool = GameObject.Find("Active Instrument Pool");
        Object[] instruments = instrumentPool.GetComponentsInChildren<InstrumentBehaviour>(false);
        foreach(InstrumentBehaviour instrument in instruments)
        {

            Debug.Log("create recording");
            Recording rec = new Recording(instrument);

            rec.playStart= 10;
            rec.playEnd = 20;
            rec.volume = 1.0f;

            this.recordings.Add(rec);
        }
    }

    public void OnCickStopAll()
    {
        this.playAll = false;
        this.ClearRecordings();
    }

    public void OnClickRecordSelectedInstrument()
    {
        InstrumentBehaviour[] instruments = GameObject.FindObjectsOfType<InstrumentBehaviour>();
        foreach (InstrumentBehaviour instrument in instruments)
        {
            if (instrument.selected && instrument.audioSource.isPlaying)
            {
                instrument.OnRecord();
            }
        }
    }
}