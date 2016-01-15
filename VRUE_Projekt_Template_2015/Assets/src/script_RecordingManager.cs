using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RecordingManager : MonoBehaviour {


    List<Recording> recordings;
    public bool playAll;

	// Use this for initialization
	void Start () {
        recordings = new List<Recording>();
        playAll = false;
	}
	
	// Update is called once per frame
	void Update () {
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

    public class Recording
    {
        InstrumentBehaviour instrument;
        float playStart;
        float playEnd;
        float volume;
        AudioSource audioSource;

        private Recording()
        {
            
        }

        public Recording(InstrumentBehaviour instrument)
        {
            this.instrument = instrument;
            this.audioSource = instrument.gameObject.GetComponent<AudioSource>();
            playEnd = -1;
            playStart = -1;
        }

        public void PlayRecording()
        {
            instrument.audioSource.Play();
            instrument.audioSource.time = playStart;
        }

        public void Stop()
        {
            instrument.audioSource.Stop();
            
        }

        public void RecordingStart()
        {
            if (audioSource)
            {
                this.playStart = audioSource.time;
            }
        }

        public void RecordingEnd()
        {

            if (audioSource)
                this.playEnd = audioSource.time;
        }

        public void Update()
        {
            if(audioSource && playStart > 0 && playEnd > 0)
            {
                if (audioSource.time > playEnd)
                {
                    audioSource.time = playStart;
                }
            }
        }
    }
}
