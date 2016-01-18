using UnityEngine;
using System.Collections;

public class VolControl : MonoBehaviour {

    public InstrumentBehaviour instrument;

    ArrayList bars;
    float stepSize;
	// Use this for initialization
	void Start () {

        bars = new ArrayList();
        for (int i = 0; i < this.transform.childCount; i++)
            bars.Add(this.transform.GetChild(i));

        stepSize = 1.0f / this.transform.childCount;
	}
	
	// Update is called once per frame
	void Update () {
	    if(instrument)
        {

            foreach(Transform bar in bars)
            {
                if (bar.localPosition.x <= instrument.audioSource.volume * 2.0f)
                {

                    bar.gameObject.SetActive(true);
                }
                else
                { 
                    bar.gameObject.SetActive(false);
                }
            }
            
            

        }
	}
}
