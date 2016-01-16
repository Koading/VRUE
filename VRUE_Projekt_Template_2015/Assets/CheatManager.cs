using UnityEngine;
using System.Collections;

public class CheatManager : MonoBehaviour {

    // Use this for initialization

    public GameObject CheaterUI;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

        if(Input.GetKeyDown(KeyCode.Insert))
        {
            CheaterUI.SetActive(true);
        }

	}
}
