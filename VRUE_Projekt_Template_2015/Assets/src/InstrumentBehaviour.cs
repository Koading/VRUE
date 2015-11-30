using UnityEngine;
using System.Collections;

public class InstrumentBehaviour : MonoBehaviour {


    //public setter for audioclip, for each instrument
    //public AudioClip clip;

    public AudioSource audioSource;
	// Use this for initialization

    public bool playState = false;
	private bool alreadyChangedState = false;
	private bool selected = false;
	private float maxVolumne;

	public float incrementalDecline = 0.1f;

	void Start () {
        Debug.Log("InstrumentBehaviour Start");

		audioSource = this.gameObject.GetComponent<AudioSource>();
		maxVolumne = audioSource.volume;

	}
	
	// Update is called once per frame
	void Update () {
	    //TODO kinect stuff
        Debug.Log("InstrumentBehaviour Update");
        if (selected && !alreadyChangedState) {
			Debug.Log ("Entered selected Instrument state");
			if(Input.GetKeyDown(KeyCode.E)) {
				playState = !playState;
				Debug.Log("Changed Play state to "  + playState); 
				
				alreadyChangedState = true;
			}
		}

		if (playState && audio) { 
            
			if(audioSource.volume > 0.0f) {
				audioSource.volume -= Mathf.Max(0.0f, incrementalDecline * Time.deltaTime);
			}
			Debug.Log("Music is now on volume: " + audioSource.volume);

			if(Input.GetKeyDown(KeyCode.E)) {
				if (!audioSource.loop) { 
					audioSource.loop = true;
					audioSource.Play ();
				} 

				if(audioSource.volume < 0.1f) {
					audioSource.Stop();
					audioSource.Play();
				}

				audioSource.volume = maxVolumne;
			}			
		}
	}


    //TODO call this from kinect functions
    public void OnKinectTriggerStart()
    {
		Debug.Log ("OnKinect Enter");
		this.selected = true;
		
    }

    public void OnKinectTriggerStop()
	{
		Debug.Log ("OnKinect Leave");
		alreadyChangedState = false;
		this.selected = false;
    }

}
