using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SpawnInstrumentPrefab : SpawnInstrumentButtonBehaviour
{

    //generic use:
    //create a prefab from a model, drag element from prefab folder to public field in gameinspector
    public Object prefab;

    GameObject instantiatedPrefab;

    public Transform playerPrefab;

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

                //change to instantiate over network: ask for client, do for server
                /*
                if (Network.isClient)
                {

                }
                else if (Network.isServer)
                {
                    //Network.instantiate
                }
                else
                {
                    //basically invalid state
                }
                */
                instantiatedPrefab = (GameObject) Network.Instantiate(prefab, new Vector3(0f,0f,0f), Quaternion.identity, 0);
                //instantiatedPrefab = (GameObject)GameObject.Instantiate(prefab, new Vector3(0f,0f,0f),Quaternion.identity);
                Debug.Log(instantiatedPrefab);

                this.instrument = instantiatedPrefab;
                this.MoveInstrumentToSpaceMouseParent();

                
            }
        }
    }
}
