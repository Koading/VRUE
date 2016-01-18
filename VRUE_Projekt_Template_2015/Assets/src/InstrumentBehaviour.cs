using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	public bool dirigentAtPult = false;


    public NetworkViewID viewID;

    public float colorOffset = 0.3f; // aims to change colorbrightness by 10% when selected

    private bool highlighted;

    private Recording rec;
    private bool isRecording = true;
    private bool hasRecording = false;

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

        //GameObject parentSpace = GameObject.Find("Active Instrument Pool");
        //this.transform.parent = parentSpace.transform;
        highlighted = false;

        
	}
	
	// Update is called once per frame
	public void Update () {

        ControlMaxVolume();

		if (dirigentAtPult) {
			if (Input.GetKeyDown (KeyCode.W)) {
				CommandInstrument ();
			}
			if (Input.GetKeyDown (KeyCode.E)) {
				Andante ();
			}
		}
		
        ApplyIncrementalDecline();

        if(this.hasRecording && audioSource.isPlaying)
        {
            rec.Update();
        }
	}

	protected void playInstrument () {
		if (audioSource) {

            //this.Play();
            this.PlayNetwork();

			if (audioSource.volume < 0.1f)
			{
                //this.Stop();
                this.StopNetwork();
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

            //this.Play();
            this.PlayNetwork();

            if (audioSource.volume < 0.1f)
            {
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

                    //this.Stop();
                    this.StopNetwork();
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
            if (audioSource.volume > 0.0f && audioSource.isPlaying)
            {
				if(dirigentAtInstrument) {
					audioSource.volume -= Mathf.Max(0.0f, incrementalDecline * Time.deltaTime * 2);					
				} else {
                	audioSource.volume -= Mathf.Max(0.0f, incrementalDecline * Time.deltaTime);
				}
            }
        }
        if(audioSource.volume == 0.0f && audioSource.isPlaying)
        {
            //Stop();
            this.StopNetwork();
        }
    }

    //TODO call this from kinect functions
    public void OnKinectTriggerStart()
    {
        //select();

        NetworkView nv = this.gameObject.GetComponent<NetworkView>();

        nv.RPC("select", RPCMode.AllBuffered);
    }

    public void OnKinectTriggerStop()
	{
        //unselect();

        NetworkView nv = this.gameObject.GetComponent<NetworkView>();

        nv.RPC("unselect", RPCMode.AllBuffered);
    }

    [RPC]
    public void select()
    {
        Debug.Log("OnKinect Enter");
        this.selected = true;

        this.StartHighlight();
    }

    [RPC]
    public void unselect()
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

    private void PlayNetwork()
    {

        NetworkView nv = this.gameObject.GetComponent<NetworkView>();
        nv.RPC("Play", RPCMode.AllBuffered);
    }

    private void StopNetwork()
    {

        NetworkView nv = this.gameObject.GetComponent<NetworkView>();
        nv.RPC("Stop", RPCMode.AllBuffered);
    }


    [RPC]
    private void Play()
    {
        if(!audioSource.isPlaying)
        {

            if(hasRecording)
            audioSource.Play();
            audioSource.loop = false;
            instrumentAnimation.enabled = true;

            if(this.hasRecording)
            {
                audioSource.time = this.rec.playStart;
                audioSource.volume = this.rec.volume;
            }
        }
    }

    [RPC]
    public void Stop()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            instrumentAnimation.enabled = false;
        }
    }

    [RPC]
    public void StartHighlight()
    {
        if (this.highlighted)
            return;
        
        int childCount = this.gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Debug.Log(this.gameObject.transform.GetChild(i));
            Transform go = this.gameObject.transform.GetChild(i);
            Debug.Log(go.renderer.materials.Length);

            if (!go.renderer)
                break;


            int colorCount = go.renderer.materials.Length;

            for (int y = 0; y < colorCount; y++)
            {
                Color color = go.renderer.materials[y].color;
                color.r += this.colorOffset;
                go.renderer.materials[y].color = color;
            }
        } 
    }

    [RPC]
    public void StopHighlight()
    {
        if (!this.highlighted)
            return;
        int childCount = this.gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Debug.Log(this.gameObject.transform.GetChild(i));
            Transform go = this.gameObject.transform.GetChild(i);
            Debug.Log(go.renderer.materials.Length);

            if (!go.renderer)
                break;

            int colorCount = go.renderer.materials.Length;

            for (int y = 0; y < colorCount; y++)
            {
                Color color = go.renderer.materials[y].color;
                color.r -= this.colorOffset;
                go.renderer.materials[y].color = color;
            }
        }

        /*
        int childCount = this.gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Debug.Log(this.gameObject.transform.GetChild(i));
            Transform go = this.gameObject.transform.GetChild(i);
            Debug.Log(go.renderer.materials.Length);


            
            if(go.renderer.materials[go.renderer.materials.Length - 1].name == "Outline")
            {

            }
        }
         */
    }

    public void MoveToPoolNetwork()
    {

        NetworkView nv = this.gameObject.GetComponent<NetworkView>();

        nv.RPC("MoveToPool", RPCMode.AllBuffered);

    }

    [RPC]
    public void MoveToPool()
    {
        Debug.Log("Fixating instrument");

        GameObject instrumentPool = GameObject.Find("Active Instrument Pool");

        this.gameObject.transform.parent = instrumentPool.transform;   
    }

    public void OnRecordNetwork()
    {
        NetworkView nv = this.gameObject.GetComponent<NetworkView>();

        nv.RPC("OnRecord", RPCMode.AllBuffered);
    }

    [RPC]
    public void OnRecord()
    {

        if (!this.audioSource.isPlaying)
            return;

        if (this.isRecording)
        {
            this.rec.playEnd = this.audioSource.time;
            this.isRecording = false;
            this.hasRecording = true;
        }

        else { 
        
            this.rec = new Recording(this);
            this.rec.playStart = this.audioSource.time;
            this.rec.volume = this.audioSource.volume;

            this.isRecording = true;
        }
    }
}
