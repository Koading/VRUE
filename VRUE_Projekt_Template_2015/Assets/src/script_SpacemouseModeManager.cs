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

    public GameObject gameObjectSpaceMouse;
    private GameObject gameObjectPult;
	private NetworkView nv;



    GameObject spaceMouse;
    private GameObject tracker;

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
            this.gameObjectSpaceMouse = GameObject.Find("Spacemouse");

            tracker = GameObject.Find("TrackingCamera");
            spaceMouse = tracker.transform.Find("Spacemouse").gameObject;
            tracker = spaceMouse.transform.Find("TrackerObject").gameObject;

			avatar = GameObject.Find("SportyGirl(Clone)");
            //avatar.transform.Find("VirtualHand").GetComponent<MeshRenderer>().enabled = true;

            avatar.transform.localPosition = new Vector3(0f, 0f, 0f);
            avatar.transform.position = new Vector3(0f, 0f, 0f);
            
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
			foreach(InstrumentBehaviour instrument in instruments) {
				if(!nearestGameObject.Equals(instrument)) {
					nv.RPC ("setDirigentMode", RPCMode.AllBuffered, instrument.name, false, false);
				}
			}
			nv.RPC ("setDirigentMode", RPCMode.AllBuffered, nearestGameObject.name, false, true);
		} else {
			foreach(InstrumentBehaviour instrument in instruments) {
				nv.RPC ("setDirigentMode", RPCMode.AllBuffered, instrument.name, true, false);
			}
		}

		nv.RPC ("SetNetworkParentStructure", RPCMode.AllBuffered, avatar.name, gameObjectPult.name );

        controlConductor = false;
    }

	[RPC]
	void SetNetworkParentStructure (string avatarName, string gameObjectPultName)
	{
		GameObject.Find (avatarName).transform.parent = GameObject.Find(gameObjectPultName).transform;		 
	}

    public void OnClickResetConductor()
    {
		if (controlConductor) 
			return;

		InstrumentBehaviour[] instruments = FindObjectsOfType<InstrumentBehaviour>();
		foreach (InstrumentBehaviour instrument in instruments) {
		
			nv.RPC("setDirigentMode", RPCMode.AllBuffered, instrument.gameObject.name, true, false);
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
	public void ExchangeNetworkParentStructure(string newChildName, string newParentName) {
		Debug.Log("Exchanging " + newChildName + " with parent: " + newParentName);

		GameObject newChild = GameObject.Find (newChildName);
		GameObject newParent = GameObject.Find (newParentName);

		if (newChild && newParent) {
			newChild.transform.position = new Vector3 (0f, 0f, 0f);
			newChild.transform.localPosition = new Vector3 (0f, 0f, 0f);
		
			newChild.transform.parent = newParent.transform;
		}
	}


    public void ExchangeParentStructure(GameObject newChild, GameObject newParent)
    {
		nv.RPC ("ExchangeNetworkParentStructure", RPCMode.AllBuffered, newChild.name, newParent.name);


    }
}
