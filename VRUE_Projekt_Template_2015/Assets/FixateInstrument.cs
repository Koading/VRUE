using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FixateInstrument : MonoBehaviour {

    public GameObject tracker;
    Button button;
    
    // Use this for initialization
	void Start () {
        if(!tracker)
            tracker = GameObject.Find("TrackingCamera").transform.Find("Spacemouse").transform.Find("TrackerObject").gameObject;
        
        button = this.GetComponent<Button>();
        button.onClick.AddListener(this.OnClick);
	}

    private void OnClick()
    {
        Debug.Log("fixate instrument button clicked");
        if(tracker)
        {

            if(tracker.transform.childCount > 0)
            {
                Debug.Log("Fixating instrument");

                Transform obj = (Transform) tracker.transform.GetChild(0);
                obj.parent = GameObject.Find("Active Instrument Pool").transform;


                //assign instrument to random audience member
                int numAudience = GameObject.Find("Audience").transform.childCount;

                for (int i = 0; i < numAudience; i++)
                {
                    GameObject visitor = GameObject.Find("Audience").transform.GetChild(Random.Range(0, numAudience - 1)).gameObject;
                    visitor = visitor.transform.GetChild(0).gameObject;

                    if (visitor.GetComponents<AudienceBehaviour>() != null)
                    {
                        AudienceBehaviour ab = visitor.GetComponent<AudienceBehaviour>();

                        Debug.Log(visitor);
                        Debug.Log(ab);

                        if (ab && !ab.assignedInstrumentScript)
                        {
                            ab.assignedInstrumentScript = obj.GetComponent<InstrumentBehaviour>();
                            break;
                        }
                    }

                }
            }
        }
    }
}
