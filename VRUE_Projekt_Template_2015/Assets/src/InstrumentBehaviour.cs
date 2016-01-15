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


    public NetworkViewID viewID;	

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
        
        this.viewID = this.gameObject.GetComponent<NetworkView>().viewID;
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

	}

	protected void playInstrument () {
		if (audioSource) {

            this.Play(this.viewID, new NetworkMessageInfo());
			
			if (audioSource.volume < 0.1f)
			{
                this.Stop(this.viewID, new NetworkMessageInfo());
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

            this.Play(this.viewID, new NetworkMessageInfo());

            if (audioSource.volume < 0.1f)
            {
                //audioSource.Stop();
                //audioSource.Play(); //????????????? wtf is this doing here?
                this.Stop(this.viewID, new NetworkMessageInfo());
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
                    this.Stop(this.viewID, new NetworkMessageInfo());
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
            Stop(this.viewID, new NetworkMessageInfo());
        }
    }

    //TODO call this from kinect functions
    public void OnKinectTriggerStart()
    {
        select(this.viewID, new NetworkMessageInfo());
    }

    public void OnKinectTriggerStop()
	{
        unselect(this.viewID, new NetworkMessageInfo());
    }

    [RPC]
    public void select(NetworkViewID viewID, NetworkMessageInfo info)
    {
        Debug.Log("OnKinect Enter");
        this.selected = true;

        this.StartHighlight();
    }

    [RPC]
    public void unselect(NetworkViewID viewID, NetworkMessageInfo info)
    {
        Debug.Log("OnKinect Leave");
        alreadyChangedState = false;
        this.selected = false;

        this.StopHighlight();
    }

    public bool isPlaying()
    { 
        return playState;
    }

    [RPC]
    private void Play(NetworkViewID viewID, NetworkMessageInfo info)
    {
        if(!audioSource.isPlaying)
        { 
            audioSource.Play();
            audioSource.loop = false;
            instrumentAnimation.enabled = true;
        }
    }

    [RPC]
    public void Stop(NetworkViewID viewID, NetworkMessageInfo info)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            instrumentAnimation.enabled = false;
        }
    }

    public void StartHighlight()
    {

    }

    public void StopHighlight()
    {

    }

    [RPC]
    public void MoveToPool()
    {
        Debug.Log("Fixating instrument");

        GameObject instrumentPool = GameObject.Find("Active Instrument Pool");

        this.gameObject.transform.parent = instrumentPool.transform;   
    }
}
