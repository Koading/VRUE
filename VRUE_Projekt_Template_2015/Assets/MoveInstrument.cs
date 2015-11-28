using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveInstrument : MonoBehaviour {

    public GameObject instrument;

    GameObject categoryParent;
    Transform initialTransform;
    GameObject tracker;
    

    Transform oldPosition;

    Button button;
	// Use this for initialization
	void Start () {
        tracker = GameObject.Find("SpaceMouse").transform.Find("TrackerObject").gameObject;
	    
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
