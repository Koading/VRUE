using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SpawnInstrumentPrefab : SpawnInstrumentButtonBehaviour
{

    public Object prefab;
    GameObject instantiatedPrefab;

    Button button_;

	// Use this for initialization
	void Start () {

        this.InitEssentials();

        button_ = this.GetComponent<Button>();
        if(button_)
            button_.onClick.AddListener(
                    this.OnClick
                );
	}
	
    
    public void OnClick()
    {
        Debug.Log("Button instantiate prefab clicked");
        //
        if(prefab)
        {

            Debug.Log("Prefab existing");
            if (this.tracker.transform.childCount == 0)
            {

                Debug.Log("instantiating");
                instantiatedPrefab = (GameObject)GameObject.Instantiate(prefab);
                Debug.Log(instantiatedPrefab);

                this.instrument = instantiatedPrefab;
                this.MoveInstrumentToSpaceMouseParent();
            }
        }
    }
}
