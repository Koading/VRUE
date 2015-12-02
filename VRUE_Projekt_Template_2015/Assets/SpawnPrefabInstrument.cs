using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnPrefabInstrument : SpawnPrefab {

    Button button;

	// Use this for initialization
	void Start () {
        button = this.GetComponent<Button>();
        //button.onClick.AddListener(this.SpawnNetworkObject);
	}
	
}
