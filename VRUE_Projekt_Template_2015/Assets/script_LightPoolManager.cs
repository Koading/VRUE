using UnityEngine;
using System.Collections;

public class script_LightPoolManager : MonoBehaviour {

    public GameObject LightPoolParent;
    public System.Collections.Generic.List<InstrumentSpotlight> lights;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
        foreach (InstrumentSpotlight IS in lights)
        {
            IS.Update();
        }
	}

    void OnSpawnInstrument(GameObject instrument)
    {
        lights.Add(new InstrumentSpotlight(instrument));
    }

    class InstrumentSpotlight : MonoBehaviour
    {
        public GameObject instrument;
        public Light spotlight;
        private GameObject instrument1;

        public InstrumentSpotlight(GameObject instrument)
        {
            this.instrument = instrument;
            this.spotlight = new Light();
            spotlight.type = LightType.Spot;
            spotlight.color = Color.green;
            spotlight.range = 50.0f;
            spotlight.spotAngle = 20.0f;
        }

        public InstrumentSpotlight (GameObject instrument, Light spotlight)
        {
            this.instrument = instrument;
            this.spotlight = spotlight;
        }


        public void Update()
        {
            if (instrument && spotlight)
            {
                spotlight.transform.LookAt(instrument.transform.position);

            }
        }

    }
}
