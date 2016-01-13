using UnityEngine;
using System.Collections;

public class script_LightPoolManager : MonoBehaviour {

    public GameObject LightPoolParent;
    public System.Collections.Generic.List<InstrumentSpotlight> lights;

	// Use this for initialization
	void Start () {
        lights = new System.Collections.Generic.List<InstrumentSpotlight>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (lights != null)
        { 
            foreach (InstrumentSpotlight IS in lights)
            {
                IS.Update();
            }
        }
	}

    public void OnSpawnInstrument(GameObject instrument)
    {
        lights.Add(new InstrumentSpotlight(instrument));
    }

    public class InstrumentSpotlight
    {
        public GameObject instrument;
        public GameObject spotlight;
        public Light spotlightComponent;
        private GameObject instrument1;

        public InstrumentSpotlight(GameObject instrument)
        {
            this.instrument = instrument;
            this.spotlight = new GameObject();
            spotlight.transform.parent = GameObject.Find("LightPool").transform;
            spotlight.transform.position = new Vector3(0, 0, 0);
            spotlight.transform.localPosition = new Vector3(0, 0, 0);

            spotlightComponent = spotlight.AddComponent<Light>();
            spotlightComponent.type = LightType.Spot;
            //spotlightComponent.color = Color.green; //pick random
            
            float r = Random.Range(0.25f, 0.75f);
            float g = Random.Range(0,1);
            float b = Random.Range(0,1);

            spotlightComponent.color = new Color(r, g, b);

            spotlightComponent.range = 75.0f;
            spotlightComponent.spotAngle = 30.0f;
        }

        public InstrumentSpotlight(GameObject instrument, GameObject spotlight)
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
