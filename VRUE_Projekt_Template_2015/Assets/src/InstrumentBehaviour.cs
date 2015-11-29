using UnityEngine;
using System.Collections;

public class InstrumentBehaviour : MonoBehaviour {


    //public setter for audioclip, for each instrument
    //public AudioClip clip;

    public AudioSource audio;
	// Use this for initialization
	void Start () {
        Debug.Log("InstrumentBehaviour Start");
        audio = this.gameObject.GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
	    //TODO kinect stuff
        Debug.Log("InstrumentBehaviour Update");
        if(audio)
        {
            if (!audio.loop)
            { 
                audio.loop = true;
                audio.Play();
            }
        }
	}
}
