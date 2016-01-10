using UnityEngine;
using System.Collections;

public class script_InstrumentPoolManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	// Tell all instruments in the instrumentpool to stop playing
	// should be used when avatar is moved
	void StopAll()
	{
		InstrumentBehaviour[] instruments = this.gameObject.GetComponentsInChildren<InstrumentBehaviour> ();

		foreach (InstrumentBehaviour instrument in instruments) {
			instrument.Stop();
		}
	}


}
