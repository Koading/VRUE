using UnityEngine;
using System.Collections;

public class InstrumentBehaviour : MonoBehaviour {


    //public setter for audioclip, for each instrument
    //public AudioClip clip;

    public AudioSource audioSource;
	// Use this for initialization
	void Start () {
        Debug.Log("InstrumentBehaviour Start");
		audioSource = this.gameObject.GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
	    //TODO kinect stuff
        Debug.Log("InstrumentBehaviour Update");
		if(audioSource)
		{
			if(Input.GetKeyDown(KeyCode.W)) {
				if (!audioSource.loop)
				{ 
					audioSource.loop = true;
					audioSource.Play();
				} else {
					audioSource.loop = false;
					audioSource.Stop();
				}
			}
        }
	}
}
