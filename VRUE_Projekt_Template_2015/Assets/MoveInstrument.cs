using UnityEngine;
using System.Collections;

public class MoveInstrument : MonoBehaviour {

    public GameObject instrument;

    GameObject tracker;

    

    Transform oldPosition;
	// Use this for initialization
	void Start () {
        tracker = GameObject.Find("TrackerObject");
	    
        if(tracker && instrument)
        {
            Debug.Log("Button pressed");


        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
    /*
    [SerializeField]
    public Button MyButton = null; // assign in the editor

    void Start()
    {
        MyButton.onClick.AddListener(() => { MyFunction(); MyOtherFunction(); });
    }
     * 
     */
}
