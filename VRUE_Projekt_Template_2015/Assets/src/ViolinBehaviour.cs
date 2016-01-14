using UnityEngine;
using System.Collections;

public class ViolinBehaviour : InstrumentBehaviour {

	new void Update () {
		if (dirigentAtInstrument) {
			if (Input.GetKeyDown (KeyCode.V)) {
				playInstrument ();
			}
		}
		base.Update ();
	}
}
