using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnInstrumentButtonBehaviour : MonoBehaviour {

    public GameObject instrument;

    protected GameObject categoryParent;
    protected Transform initialTransform;
    protected GameObject tracker;
    protected GameObject spaceMouse;

    protected Transform oldPosition;

    Button button;
	// Use this for initialization
	void Start () {
        //@lva: objects do not seem to have a fixed naming convention :P
        //either use camelcase everywhere or don't
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
	
    protected void InitEssentials()
    {

        tracker = GameObject.Find("TrackingCamera");
        spaceMouse = tracker.transform.Find("Spacemouse").gameObject;
        tracker = spaceMouse.transform.Find("TrackerObject").gameObject;

    }
    
    void OnClick()
    {
        Debug.Log("Button clicked");
        
        MoveInstrumentToSpaceMouseParent();
    }
    
    protected void MoveInstrumentToSpaceMouseParent()
    {

        if(instrument)
        {

            if (!spaceMouse.gameObject.activeSelf)
            {
                spaceMouse.SetActive(true);
            }

            
            //reset positioning
            //motivation: after positioning one instrument, the next magically gets a global position != origin
            //TODO: check if this isn't a dirty hack
            instrument.transform.localPosition = new Vector3(0f, 0f, 0f);
            instrument.transform.position = new Vector3(0f, 0f, 0f);

            spaceMouse.transform.position = new Vector3(0f, 0f, 0f);
            spaceMouse.transform.localPosition = new Vector3(0f, 0f, 0f);

            tracker.transform.position = new Vector3(0f, 0f, 0f);
            tracker.transform.position = new Vector3(0f, 0f, 0f);

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
