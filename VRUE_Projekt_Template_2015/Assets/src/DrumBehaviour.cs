using UnityEngine;
using System.Collections;

public class DrumBehaviour : InstrumentBehaviour {

	new void Update () {
		if (dirigentAtInstrument) {
			if (Input.GetKeyDown (KeyCode.D)) {
				playInstrument ();
			}
		}
		base.Update ();
	}
}
