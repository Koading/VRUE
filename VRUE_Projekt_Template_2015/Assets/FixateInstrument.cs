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
            }
        }
    }
}
