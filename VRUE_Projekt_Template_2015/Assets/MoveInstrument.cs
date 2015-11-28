using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveInstrument : MonoBehaviour {

    public GameObject instrument;

    GameObject categoryParent;
    Transform initialTransform;
    GameObject tracker;
    GameObject spaceMouse;

    Transform oldPosition;

    Button button;
	// Use this for initialization
	void Start () {
        //@lva: objects do not seem to have a fixed naming convention :P
        //either use camelcase everywhere or don't
        tracker = GameObject.Find("TrackingCamera");
        spaceMouse = tracker.transform.Find("Spacemouse").gameObject;
        tracker = spaceMouse.transform.Find("TrackerObject").gameObject;
        Debug.Log(tracker);
        if(tracker && instrument)
        {
            Debug.Log("Button pressed");

            categoryParent = instrument.transform.parent.gameObject;
            initialTransform = instrument.transform;
            button = this.GetComponent<Button>();
            button.onClick.AddListener(
                    this.OnClick
                );
        }

    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    
    void OnClick()
    {
        Debug.Log("Button clicked");
        spaceMouse.active = true;
        MoveInstrumentToSpaceMouseParent();
    }
    
    void MoveInstrumentToSpaceMouseParent()
    {
        if(instrument)
        {
            instrument.transform.parent = tracker.transform;
        }
    }

    void ResetInstrument()
    {
        if(instrument)
        {
            instrument.transform.localPosition = initialTransform.localPosition;
            instrument.transform.eulerAngles = initialTransform.eulerAngles;

            instrument.transform.parent = categoryParent.transform;
        }
    }

}
