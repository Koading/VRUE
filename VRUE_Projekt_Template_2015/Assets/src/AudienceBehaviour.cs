using UnityEngine;
using System.Collections;

public class AudienceBehaviour : MonoBehaviour {

	// Use this for initialization

    const string instrumentPool = "Active Instrument Pool";
    
    //public GameObject assignedInstrument = null;

    public InstrumentBehaviour assignedInstrumentScript; //{get;set;}
    
    
	Animator rootAnimator;

    void Start () {
        assignedInstrumentScript = null;
		rootAnimator = this.gameObject.transform.parent.gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	    if(assignedInstrumentScript)
        {
            Debug.Log("audience loop update");
            if (assignedInstrumentScript.audioSource)
            {
                if (assignedInstrumentScript.audioSource.gameObject.activeSelf
                && assignedInstrumentScript.audioSource.volume > 0.1
                && assignedInstrumentScript.isPlaying())
                {
                    Debug.Log("audience animator set active true");
					rootAnimator.enabled = true;
                }
                else
                {
                    Debug.Log("audience animater set active false");
					rootAnimator.enabled= false;
                }
            }
        }
	}

}
