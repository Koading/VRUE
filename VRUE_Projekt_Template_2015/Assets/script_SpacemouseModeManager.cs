using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * This script handles the modes as per multimedia interface exercise definition
 * Pult mode handles the conductor being on the pult and controll instruments in general
 * instrument mode handles the conductor being moved to an instrument and give small instructions in that form
 * */
public class script_SpacemouseModeManager : MonoBehaviour {

    public Button buttonControlConductor;
    public Button buttonFixConductor;
    public Button buttonResetConductor;

    public GameObject gameObjectConductor;
    public GameObject gameObjectSpaceMouse;
    public GameObject gameObjectPult;

    bool controlConductor;

    
    // Use this for initialization
	void Start () {
        controlConductor = false;
	}
	
    void init()
    {
        buttonControlConductor.onClick.AddListener(this.OnClickControlConductor);
        buttonFixConductor.onClick.AddListener(this.OnClickFixConductor);
    }

    public void OnClickControlConductor()
    {
        if (controlConductor)
            return;

        //change spawntest please
        this.gameObjectConductor = GameObject.Find("SpawnTest");
        this.gameObjectSpaceMouse = GameObject.Find("Spacemouse");

        GameObject tracker = GameObject.Find("TrackingCamera");
        GameObject spaceMouse = tracker.transform.Find("Spacemouse").gameObject;
        tracker = spaceMouse.transform.Find("TrackerObject").gameObject;

        spaceMouse.SetActive(true);

        //gameObjectConductor.transform.parent = tracker.transform;

        ExchangeParentStructure(gameObjectConductor, tracker);

        controlConductor = true;
    }

    public void OnClickFixConductor()
    {
        if (!controlConductor)
            return;

        controlConductor = false;
    }

    public void OnClickResetConductor()
    {
        gameObjectPult = GameObject.Find("Pult");


        this.ExchangeParentStructure(gameObjectConductor, gameObjectPult);
    }

    
    public void ExchangeParentStructure(GameObject newChild, GameObject newParent)
    {

        Debug.Log("Exchanging");
        newChild.transform.position = new Vector3(0f, 0f, 0f);
        newChild.transform.localPosition = new Vector3(0f, 0f, 0f);
        //object1.transform.position = new Vector3(0f, 0f, 0f);

        //spaceMouse.transform.position = new Vector3(0f, 0f, 0f);
        //newParent.transform.localPosition = new Vector3(0f, 0f, 0f);

        newChild.transform.parent = newParent.transform;
    }
}
