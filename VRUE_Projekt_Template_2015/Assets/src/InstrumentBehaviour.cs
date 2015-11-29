using UnityEngine;
using System.Collections;

public class InstrumentBehaviour : MonoBehaviour {


    //public setter for audioclip, for each instrument
    //public AudioClip clip;

    public AudioSource audio;
	// Use this for initialization

    public bool playState = false;
	void Start () {
        Debug.Log("InstrumentBehaviour Start");
        audio = this.gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    //TODO kinect stuff
        Debug.Log("InstrumentBehaviour Update");
        if(playState && audio)
        { 
            {
                if (!audio.loop)
                { 
                    audio.loop = true;
                    audio.Play();
                }
            }
        }
	}


    //TODO call this from kinect functions
    void OnKinectTriggerStart()
    {
        this.playState = true;
    }

    void OnKinectTriggerStop()
    {
        this.playState = false;
    }

}
