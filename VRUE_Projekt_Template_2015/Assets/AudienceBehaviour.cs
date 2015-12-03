using UnityEngine;
using System.Collections;

public class AudienceBehaviour : MonoBehaviour {

	// Use this for initialization

    const string instrumentPool = "Active Instrument Pool";
    
    private GameObject assignedInstrument = null;
    private InstrumentBehaviour assignedInstrumentScript = null;
    private Animation animation;
	void Start () {
        animation = this.transform.parent.GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(assignedInstrumentScript)
        {
            //if(assignedInstrumentScript.audioSource.active< )
            {

            }
        }
	}

    public void assignInstrument(InstrumentBehaviour ib)
    {
        assignedInstrumentScript = ib;
    }
}
