﻿using UnityEngine;
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
    private GameObject gameObjectPult;
	private NetworkView nv;



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

		gameObjectPult = GameObject.Find ("Pult");
		nv = this.gameObject.GetComponent<NetworkView> ();
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
            //avatar.transform.Find("VirtualHand").GetComponent<MeshRenderer>().enabled = true;

            avatar.transform.localPosition = new Vector3(0f, 0f, 0f);
            avatar.transform.position = new Vector3(0f, 0f, 0f);
			GameObject virtualHand = GameObject.Find ("VirtualHand(Clone)");
			virtualHand.transform.localRotation = Quaternion.identity;
			ExchangeParentStructure(virtualHand, avatar);

			virtualHand.GetComponent<HomerInteraction>().enabled = true;
            
            isAvatarInstantiated = true;

		}

        /*
        spaceMouse.transform.position = new Vector3(0f, 0f, 0f);
        spaceMouse.transform.localPosition = new Vector3(0f, 0f, 0f);

        tracker.transform.position = new Vector3(0f, 0f, 0f);
        tracker.transform.position = new Vector3(0f, 0f, 0f);
        */

        spaceMouse.transform.position = avatar.transform.position;

        tracker.transform.position = avatar.transform.position;

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

        InstrumentBehaviour[] instruments = FindObjectsOfType<InstrumentBehaviour>();

        GameObject nearestGameObject = gameObjectPult;
        float nearestDistance = Vector3.Distance(avatar.transform.position, gameObjectPult.transform.position);
		Debug.Log("Avatar: " + avatar.transform.position);
		Debug.Log ("Pult: " + gameObjectPult.transform.position);

        foreach(InstrumentBehaviour instrument in instruments)
        {
            float currentDistance = Vector3.Distance(instrument.gameObject.transform.position, avatar.transform.position);
			Debug.Log("Found instrument: " + instrument.name + "CurrentDistance: " + currentDistance + " / NearestDistance: " + nearestDistance);
			Debug.Log("Instrument: " + instrument.gameObject.transform.position);
            if ( currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestGameObject = instrument.gameObject;
            }
		}

        if (!nearestGameObject.Equals (gameObjectPult)) {
			nv.RPC ("setDirigentMode", RPCMode.AllBuffered, nearestGameObject.name, false, true);
		} else {
			foreach(InstrumentBehaviour instrument in instruments) {
				nv.RPC ("setDirigentMode", RPCMode.AllBuffered, instrument.name, true, false);
			}
		}

        controlConductor = false;
    }

    public void OnClickResetConductor()
    {
		InstrumentBehaviour[] instruments = FindObjectsOfType<InstrumentBehaviour>();
		NetworkViewID nvId = Network.AllocateViewID();
		foreach (InstrumentBehaviour instrument in instruments) {
		
			nv.RPC("setDirigentMode", RPCMode.AllBuffered, instrument.gameObject.name, false, false);
		}

		this.ExchangeParentStructure(avatar, gameObjectPult);
    }

	[RPC]
	private void setDirigentMode(string instrumentName, bool dirigentAtPult, bool dirigentAtInstrument) {
		InstrumentBehaviour instrument = GameObject.Find (instrumentName).GetComponent<InstrumentBehaviour>();
		if (instrument) {
			instrument.dirigentAtPult = dirigentAtPult;
			instrument.dirigentAtInstrument = dirigentAtInstrument;
		}
	}

    [RPC]
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
