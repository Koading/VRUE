using UnityEngine;
using System.Collections;

public class AudienceBehaviour : MonoBehaviour {

	// Use this for initialization

    const string instrumentPool = "Active Instrument Pool";
    
    public GameObject assignedInstrument = null;

    public InstrumentBehaviour assignedInstrumentScript; //{get;set;}
    

    private Animator animation;
	void Start () {
        assignedInstrumentScript = null;
        animation = this.transform.parent.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(assignedInstrumentScript)
        {
            if(assignedInstrumentScript.audioSource.active 
                && assignedInstrumentScript.audioSource.volume > 0.1 
                && assignedInstrumentScript.isPlaying())
            {
                animation.active = true;
            }
            else
            {
                animation.active = false;
            }
        }
	}

}
