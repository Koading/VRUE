using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FixateInstrument : MonoBehaviour {

    public GameObject tracker;
    Button button;
    private GameObject instrumentPool;

    // Use this for initialization
	void Start () {
        if(!tracker)
            tracker = GameObject.Find("TrackingCamera").transform.Find("Spacemouse").transform.Find("TrackerObject").gameObject;
        
        button = this.GetComponent<Button>();
        button.onClick.AddListener(this.OnClick);

        instrumentPool = GameObject.Find("Active Instrument Pool");
	}

    private void OnClick()
    {
        Debug.Log("fixate instrument button clicked");
        if(tracker)
        {

            if(tracker.transform.childCount > 0)
            {
                
                Transform obj = (Transform) tracker.transform.GetChild(0);

                InstrumentBehaviour instrument = obj.GetComponent<InstrumentBehaviour>();

                //instrument.MoveToPool(instrument.viewID, new NetworkMessageInfo());
                //instrument.MoveToPool();
                instrument.MoveToPoolNetwork();

                //assign instrument to random audience member
                int numAudience = GameObject.Find("Audience").transform.childCount;

                Debug.Log("numaudience " + numAudience);

                for (int i = 0; i < numAudience; i++)
                {
                    int audIndex = Random.Range(0, numAudience - 1);

                    Debug.Log("audindex " + audIndex);

                    GameObject visitor = GameObject.Find("Audience").transform.GetChild(audIndex).gameObject;
                    visitor = visitor.transform.GetChild(0).gameObject;

                    if (visitor.GetComponents<AudienceBehaviour>() != null)
                    {
                        AudienceBehaviour ab = visitor.GetComponent<AudienceBehaviour>();

                        //TODO move this to AudienceBehaviour, make RPC, check if it works

                        Debug.Log(visitor);
                        Debug.Log(ab);

                        if (ab && !ab.assignedInstrumentScript)
                        {
                            Debug.Log(obj.GetComponent<InstrumentBehaviour>());
                            ab.assignedInstrumentScript = obj.GetComponent<InstrumentBehaviour>();
                            break;
                        }
                    }

                }
            }
        }
    }
    
}
