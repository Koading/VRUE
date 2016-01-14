using UnityEngine;
using System.Collections;

public class XylophoneBehaviour
    : InstrumentBehaviour {

	new void Update () {
		if (dirigentAtInstrument) {
			if (Input.GetKeyDown (KeyCode.X)) {
				playInstrument ();
			}
		}
		base.Update ();
	}
}
