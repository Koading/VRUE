using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour {

    GameObject userInterface;

	// Use this for initialization
	void Start () {
        userInterface = GameObject.Find("UserInterface");
	
	}
	
	// Update is called once per frame
	void Update () {
        OnConnect();
	}

    void OnConnect()
    {
        if (Network.isClient && !userInterface.transform.Find("Canvas SpaceMouseUser").active)
        {
            userInterface.transform.Find("Canvas SpaceMouseUser").active = true;
        }
    }
}
