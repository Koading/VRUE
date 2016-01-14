using UnityEngine;
using System.Collections;

public class TrumpetBehaviour
    : InstrumentBehaviour {

	new void Update () {
		if (dirigentAtInstrument) {
			if (Input.GetKeyDown (KeyCode.J)) {
				playInstrument ();
			}
		}
		base.Update ();
	}
}
