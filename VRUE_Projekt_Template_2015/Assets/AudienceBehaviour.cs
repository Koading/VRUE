using UnityEngine;
using System.Collections;

public class AudienceBehaviour : MonoBehaviour {

	// Use this for initialization

    const string instrumentPool = "Active Instrument Pool";
    
    public GameObject assignedInstrument = null;

    public InstrumentBehaviour assignedInstrumentScript; //{get;set;}
    

    void Start () {
        assignedInstrumentScript = null;
	}
	
	// Update is called once per frame
	void Update () {
	    if(assignedInstrumentScript)
        {
            if (assignedInstrumentScript.audioSource)
            {
                if (assignedInstrumentScript.audioSource.gameObject.activeSelf
                && assignedInstrumentScript.audioSource.volume > 0.1
                && assignedInstrumentScript.isPlaying())
                {
                    animation.gameObject.SetActive(true);
                }
                else
                {
                    animation.gameObject.SetActive(false);
                }
            }
        }
	}

}
