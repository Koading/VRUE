using UnityEngine;
using System.Collections;

public class InstrumentBehaviour : MonoBehaviour {


    //public setter for audioclip, for each instrument
    //public AudioClip clip;

    public AudioSource audioSource;

    private Animator instrumentAnimation;
    // Use this for initialization

    public bool playState = false;
	public bool alreadyChangedState = false;
	public bool selected = false;
	public float maxVolumne;

    public bool controlVolumeSelected = false;
	public float incrementalDecline = 0.1f;

    GameObject spaceMouse;

	private Material previosMaterial;


	//struct healthBar

	
	public bool dirigentAtInstrument = false;
		
	

	void OnGui()
	{
		//Vector2 pos = new Vector2(20,20);
		//Vector2 size = new Vector2(100, 20);
		
		//Texture2D empty = new Texture2D (100, 20,TextureFormat.Alpha8,false);

		Debug.Log ("OnGUI call");
	}



	void Start () {
        Debug.Log("InstrumentBehaviour Start");

		audioSource = this.gameObject.GetComponent<AudioSource>();
        instrumentAnimation = this.GetComponent<Animator>();
		maxVolumne = audioSource.volume;

        spaceMouse = GameObject.Find("Spacemouse");
	}
	
	// Update is called once per frame
	public void Update () {

        ControlMaxVolume();

		if (!dirigentAtInstrument) {
			if (Input.GetKeyDown (KeyCode.W)) {
				CommandInstrument ();
			}
			if (Input.GetKeyDown (KeyCode.E)) {
				Andante ();
			}
		}
		

        ApplyIncrementalDecline();
        
        //TODO: Take the idea and make it better.
        //this crashes because there is no renderer attached to the parent gameobject.
        //oldPostionY = spaceMouse.transform.localEulerAngles.y;
        /*
		if (this.selected) {
			//TODO: let both users see the instrument as selected
			this.gameObject.renderer.material.SetColor("_Color", Color.blue);
		} else {
			this.gameObject.renderer.material.SetColor("_Color", Color.white);
		}
        */

	}

	protected void playInstrument () {
		if (audioSource) { 
			
			if (!audioSource.loop)
			{
				audioSource.loop = true;
				this.Play();
			}
			
			if (audioSource.volume < 0.1f)
			{
				this.Stop();
			}
			
			audioSource.volume += (float)(maxVolumne / 10.0);
		}
	}


    /**
     * 
     */
    private void ControlMaxVolume()
    {
		if (controlVolumeSelected && audioSource && spaceMouse)
        {
            float deltaSpaceY = spaceMouse.transform.localEulerAngles.y;


            if (spaceMouse.transform.localEulerAngles.y < 10f)
                deltaSpaceY = 0;
            if (spaceMouse.transform.localEulerAngles.y >= 180f)
                deltaSpaceY = spaceMouse.transform.localEulerAngles.y - 360f;

            maxVolumne = Mathf.Max(Mathf.Min(maxVolumne + maxVolumne * (-deltaSpaceY / 360f) * Time.deltaTime, 1.0f), 0.2f);
        }
    }
    private void Andante()
    {
	    if (playState && audioSource) { 
     
            if (!audioSource.loop)
            {
                audioSource.loop = true;
                //audioSource.Play ();
                this.Play();
            }

            if (audioSource.volume < 0.1f)
            {
                //audioSource.Stop();
                //audioSource.Play(); //????????????? wtf is this doing here?
                this.Stop();
            }

            audioSource.volume = maxVolumne;
        }
    }

    private void CommandInstrument()
    {
        if (selected && !alreadyChangedState)
        {
            Debug.Log("Entered selected Instrument state");
            {
                playState = !playState;

                if (!playState)
                {
                    //audioSource.Stop();
                    //instrumentAnimation.enabled = false;
                    this.Stop();
                }
                else
                {
                    Debug.Log("enable animation");
                    instrumentAnimation.enabled = true;
                }

                Debug.Log("Changed Play state to " + playState);

                alreadyChangedState = true;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
				controlVolumeSelected = !controlVolumeSelected;
            }
        }
    }

    private void ApplyIncrementalDecline()
    {
        if (audioSource)
        {
            if (audioSource.volume > 0.0f)
            {
				if(dirigentAtInstrument) {
					audioSource.volume -= Mathf.Max(0.0f, incrementalDecline * Time.deltaTime * 2);					
				} else {
                	audioSource.volume -= Mathf.Max(0.0f, incrementalDecline * Time.deltaTime);
				}
            }
        }
        if(audioSource.volume == 0.0f)
        {
            Stop();
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

    public bool isPlaying()
    { 
        return playState;
    }

    private void Play()
    {
        audioSource.Play();
        instrumentAnimation.enabled = true;
    }


    public void Stop()
    {
        audioSource.Stop();
        instrumentAnimation.enabled = false;
    }


}
