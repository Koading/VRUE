using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RecordingManager : MonoBehaviour {


    List<Recording> recordings;
    bool playAll;

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

            }
        }
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
        }

        public void PlayRecording()
        {
        }

        public void RecordingStart()
        {
            if (audioSource)
            {
            }
        }
    }
}
