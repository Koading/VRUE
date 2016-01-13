using UnityEngine;
using System.Collections;

public class GuitarBehaviour : InstrumentBehaviour {

	new void Update () {
		if (dirigentAtInstrument) {
			if (Input.GetKeyDown (KeyCode.G)) {
				playInstrument ();
			}
		}
		base.Update ();
	}
}
