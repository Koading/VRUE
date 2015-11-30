using UnityEngine;
using System.Collections;

public class InstrumentBehaviour : MonoBehaviour {


    //public setter for audioclip, for each instrument
    //public AudioClip clip;

    public AudioSource audioSource;
	// Use this for initialization

    public bool playState = false;
	private bool alreadyChangedState = false;
	void Start () {
        Debug.Log("InstrumentBehaviour Start");

		audioSource = this.gameObject.GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
	    //TODO kinect stuff
        Debug.Log("InstrumentBehaviour Update");
        if (playState && audio) { 
            
			if(Input.GetKeyDown(KeyCode.W)) {
				alreadyChangedState = true;
				if (!audioSource.loop) { 
					audioSource.loop = true;
					audioSource.Play ();
				} else {
					audioSource.loop = true;
					audioSource.Stop();
				}
			}			
		}
	}


    //TODO call this from kinect functions
    public void OnKinectTriggerStart()
    {
		this.playState = true;
		
    }

    public void OnKinectTriggerStop()
    {
		alreadyChangedState = true;
		this.playState = false;
    }

}
