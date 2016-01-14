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



    GameObject spaceMouse;
    public GameObject tracker;

    GameObject avatar;
    public Transform prefab;


    bool controlConductor;

    public bool isAvatarInstantiated;
    // Use this for initialization
	void Start () {
        controlConductor = false;
        isAvatarInstantiated = false;
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



        if (!isAvatarInstantiated)
        {
            //change spawntest please
            this.gameObjectConductor = GameObject.Find("Pult");
            this.gameObjectSpaceMouse = GameObject.Find("Spacemouse");

            tracker = GameObject.Find("TrackingCamera");
            spaceMouse = tracker.transform.Find("Spacemouse").gameObject;
            tracker = spaceMouse.transform.Find("TrackerObject").gameObject;

            avatar = ((Transform)Network.Instantiate(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity, 0)).gameObject;


            
            spaceMouse.transform.position = new Vector3(0f, 0f, 0f);
            spaceMouse.transform.localPosition = new Vector3(0f, 0f, 0f);

            tracker.transform.position = new Vector3(0f, 0f, 0f);
            tracker.transform.position = new Vector3(0f, 0f, 0f);

            
            isAvatarInstantiated = true;

        }

        spaceMouse.SetActive(true);



        //gameObjectConductor.transform.parent = tracker.transform;

        ExchangeParentStructure(avatar, tracker);

        controlConductor = true;
    }

    public void OnClickFixConductor()
    {
        if (!controlConductor)
            return;


        //OnClickResetConductor();
        avatar.transform.parent = gameObjectPult.transform;

        controlConductor = false;
    }

    public void OnClickResetConductor()
    {
        gameObjectPult = GameObject.Find("Pult");

        this.ExchangeParentStructure(avatar, gameObjectPult);
    }

    public void ExchangeParentStructure(GameObject newChild, GameObject newParent)
    {

        Debug.Log("Exchanging " + newChild.name + " with parent: " + newParent.name);
        newChild.transform.position = new Vector3(0f, 0f, 0f);
        newChild.transform.localPosition = new Vector3(0f, 0f, 0f);
        //object1.transform.position = new Vector3(0f, 0f, 0f);

        //spaceMouse.transform.position = new Vector3(0f, 0f, 0f);
        //newParent.transform.localPosition = new Vector3(0f, 0f, 0f);

        newChild.transform.parent = newParent.transform;
    }
}
