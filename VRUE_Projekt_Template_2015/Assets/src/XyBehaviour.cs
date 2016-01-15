using UnityEngine;
using System.Collections;

public class XyBehaviour
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
