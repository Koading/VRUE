using UnityEngine;
using System.Collections;

public class InstrumentBehaviour : MonoBehaviour {


    //public setter for audioclip, for each instrument
    //public AudioClip clip;

    public AudioSource audioSource;

    private Animator animation;
    // Use this for initialization

    public bool playState = false;
	public bool alreadyChangedState = false;
	public bool selected = false;
	private float maxVolumne;

    public bool isSelected;
	public float incrementalDecline = 0.1f;

    GameObject spaceMouse;

	private Material previosMaterial;


    private float oldPostionY;
	void Start () {
        Debug.Log("InstrumentBehaviour Start");

		audioSource = this.gameObject.GetComponent<AudioSource>();
        animation = this.GetComponent<Animator>();
		maxVolumne = audioSource.volume;



        spaceMouse = GameObject.Find("Spacemouse");
		if (spaceMouse) {
			oldPostionY = spaceMouse.transform.localEulerAngles.y;
		}
	}
	
	// Update is called once per frame
	void Update () {

        if(isSelected && audioSource && spaceMouse)
        {
            float deltaSpaceY = spaceMouse.transform.localEulerAngles.y;
            
            //Debug.Log("deltay: " + deltaSpaceY);
            
            if (spaceMouse.transform.localEulerAngles.y < 10f)
                deltaSpaceY = 0;
            if (spaceMouse.transform.localEulerAngles.y >= 180f)
                deltaSpaceY = spaceMouse.transform.localEulerAngles.y - 360f;

            maxVolumne = Mathf.Max(Mathf.Min( maxVolumne + maxVolumne * (-deltaSpaceY / 360f) * Time.deltaTime, 1.0f),0.2f);
        }

	    //TODO kinect stuff
        Debug.Log("InstrumentBehaviour Update");
        if (selected && !alreadyChangedState) {
			Debug.Log ("Entered selected Instrument state");
			if(Input.GetKeyDown(KeyCode.E)) {
				playState = !playState;

				if(!playState) {
					audioSource.Stop();
					animation.enabled = false;
				} else {
					animation.enabled = true;
				}

				Debug.Log("Changed Play state to "  + playState); 
				
				alreadyChangedState = true;
			}
			if(Input.GetKeyDown(KeyCode.R)) {
				isSelected = !isSelected;
			}
		}

		if (playState && audioSource) { 
            
			if(audioSource.volume > 0.0f) {
				audioSource.volume -= Mathf.Max(0.0f, incrementalDecline * Time.deltaTime);
			}
			Debug.Log("Music is now on volume: " + audioSource.volume);

            //Debug.Log("KeyPressed E:" + Input.GetKeyDown(KeyCode.E));
			if(Input.GetKeyDown(KeyCode.W)) {
                
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
        //oldPostionY = spaceMouse.transform.localEulerAngles.y;
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
