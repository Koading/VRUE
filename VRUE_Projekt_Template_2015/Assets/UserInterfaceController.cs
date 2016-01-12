using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour {

    GameObject userInterface;

    public GameObject canvasHome;
    public GameObject canvasInstruments;
    public GameObject canvasControl;

    public Button buttonGUIInstrumnent;
    public Button buttonGUIControl;


    enum UIState
    {
        Disabled = 0,
        Home = 1,
        Instrument = 2,
        Control = 3,
        Invalid = -1
    };


    UIState state;
    UIState oldState;
	// Use this for initialization
	void Start () {
        state = UIState.Disabled;
        oldState = UIState.Invalid;
        userInterface = GameObject.Find("UserInterface");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Update userinterfacecontroller");
        stateAction();

	}



    void OnConnect()
    {
        if (Network.isClient 
            && !userInterface.transform.Find("Canvas SpaceMouseUser").gameObject.activeSelf
            && this.state == UIState.Disabled)
        {
            userInterface.transform.Find("Canvas SpaceMouseUser").gameObject.SetActive(true);
            this.changeState(UIState.Home);
        }
    }



    private void stateAction()
    {
        OnConnect();

        if(!this.stateChanged())
            return;

            switch(this.state)
            {
                case UIState.Home:
                    canvasControl.SetActive(false);
                    canvasHome.SetActive(true);
                    canvasInstruments.SetActive(false);
                    
                    break;
                case UIState.Instrument:
                    canvasControl.SetActive(false);
                    canvasHome.SetActive(false);
                    canvasInstruments.SetActive(true);
                    
                    break;
                case UIState.Control:
                    canvasControl.SetActive(true);
                    canvasHome.SetActive(false);
                    canvasInstruments.SetActive(false);
                    
                    break;
                default:
                    Debug.Log("shouldnt happen");
                    canvasControl.SetActive(false);
                    canvasHome.SetActive(false);
                    canvasInstruments.SetActive(false);
                    break;
                case UIState.Disabled:
                    canvasControl.SetActive(false);
                    canvasHome.SetActive(false);
                    canvasInstruments.SetActive(false);
                    
                    
                    break;
            }

    }

    private void changeState(UIState newState)
    {
        state = newState;
    }

    private bool stateChanged()
    {
        bool ret = !(state == oldState);

        state = oldState;
        
        return ret;
    }

    public void OnButtonInstruments()
    {
        this.changeState(UIState.Instrument);
    }

    public void OnButtonControlConductor()
    {
        this.changeState(UIState.Control);
    }

    public void OnButtonHome()
    {
        this.changeState(UIState.Home);
    }
}
