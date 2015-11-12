using System;
using System.Collections;
/* =====================================================================================
 * ARTiFICe - Augmented Reality Framework for Distributed Collaboration
 * ====================================================================================
 * Copyright (c) 2010-2012 
 * 
 * Annette Mossel, Christian Schönauer, Georg Gerstweiler, Hannes Kaufmann
 * mossel | schoenauer | gerstweiler | kaufmann @ims.tuwien.ac.at
 * Interactive Media Systems Group, Vienna University of Technology, Austria
 * www.ims.tuwien.ac.at
 * 
 * ====================================================================================
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *  
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * =====================================================================================
 */
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to show a GUI to select a IT by ARToolkitMarker during runtime. 
/// </summary>
public class ITSelectionGUI : MonoBehaviour
{
    public GUIStyle vrue11Style;
    protected Text selectionText;
    protected string _vh;
    protected string _triggerMarker;

    /* ------------------ VRUE Tasks START --------------------------
    * Place required member variables here
    ----------------------------------------------------------------- */

    //list of ITs 
    //protected GameObject virtualHand;
    protected Hashtable listIT = new Hashtable();
    protected Component[] listComponent;

    //TrackMultiMarker tracker;
    protected GameObject tracker;
    protected MultiMarkerSwitch markerSwitch;

    bool selectionActive;

    protected Canvas GUICanvas;
    protected GameObject ArrowImage;
    // ------------------ VRUE Tasks END ----------------------------

    /// <summary>
    /// Set StartUp Data. Method is called by OnEnable Unity Callback
    /// Must be overwritten in deriving class
    /// </summary>
    protected virtual void StartUpData()
    {
        // name of interaction object in Unity Hierarchy
        _vh = "VirtualHand";

        // name of trigger marker
        _triggerMarker = "Marker2";

    }

    protected GameObject getVirtualHand()
    {

        return GameObject.Find(_vh);

    }

    /// <summary>
    /// </summary>
    protected void OnEnable()
    {
        tracker = GameObject.Find("TrackerObject");
        markerSwitch = this.gameObject.GetComponent<MultiMarkerSwitch>();

        GUICanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        selectionText = GameObject.Find("ITSelectionText").GetComponent<Text>();
        ArrowImage = GameObject.Find("SelectionArrow");

        //selectionText.enabled = false;
        GUICanvas.enabled = false;

        // set init data
        StartUpData();

        /* ------------------ VRUE Tasks START --------------------------
        * find ITs (components) attached to interaction game object
        * if none is attached, manually attach 3 ITs to interaction game object
        * initially det default IT
        ----------------------------------------------------------------- */
        selectionActive = false;

        GenerateITList();

        // ------------------ VRUE Tasks END ----------------------------
    }

    protected void GenerateITList()
    {
        if (!getVirtualHand() )
            return;
        if (listIT.Count > 0)
            return;

        listComponent = getVirtualHand().GetComponents<Component>();

        foreach (Component osb in listComponent)
        {
            String type = osb.GetType().ToString();
            //Debug.Log(type);
            if ("GoGoInteraction".CompareTo(type) == 0)
            {
                listIT.Add(type, osb);
                ((Behaviour)osb).enabled = false;

            }
            else if ("HomerInteraction".CompareTo(type) == 0)
            {
                listIT.Add(type, osb);
                ((Behaviour)osb).enabled = false;

            }
            else if ("VirtualHandInteraction".CompareTo(type) == 0)
            {
                listIT.Add(type, osb);
                ((Behaviour)osb).enabled = true;
                selectionText.text = "VirtualHandInteraction";
            }
        }

        if (listIT.Count == 0)
        {
            VirtualHandInteraction vhi = getVirtualHand().AddComponent<VirtualHandInteraction>();

            GoGoInteraction gg = getVirtualHand().AddComponent<GoGoInteraction>();
            gg.thresholdDistance = 4.0f;
            gg.coefficient = 0.5f;

            HomerInteraction hi = getVirtualHand().AddComponent<HomerInteraction>();

            hi.enabled = true;
            vhi.enabled = false;
            gg.enabled = false;

            listIT.Add(vhi.GetType().ToString(), vhi);
            listIT.Add(gg.GetType().ToString(), gg);
            listIT.Add(hi.GetType().ToString(), hi);

            selectionText.text = "VirtualHandInteraction";
        }

    }


    /// <summary>
    /// Unity Callback
    /// OnGUI is called every frame for rendering and handling GUI events.
    /// </summary>
    protected void OnGUI()
    {

        /* ------------------ VRUE Tasks START --------------------------
        * check if ITs are available
        * if trigger marker is visible and no objects are currently selected by interaction game object show GUI
        * depending on visible marker switch through availabe ITs
        * implement user confirmation and set selected IT only if user has confirmed it
        * disable the GUI if virtual hand has selected objects and if user has confirmend an IT
        ----------------------------------------------------------------- */

        GenerateITList();

        if (listIT.Count > 0 && tracker && markerSwitch)
        {

            // show virtual hand -> physical hand is autmatically rendert due to tracking state
            //tracker.transform.parent.GetComponent<TrackBase>().setVisability(virtualHand, true);

            if (_triggerMarker.CompareTo(markerSwitch.GetFaceFront()) == 0)
            {
                if (!selectionActive)
                {
                    //show GUI here
                    Debug.Log("Marker2 tracked, selection ready");
                    selectionActive = true;
                    //selectionText.enabled = true;
                    GUICanvas.enabled = true;

                }

                //Vector3 z = new Vector3(0, 0, 1);
                //ArrowImage.transform.eulerAngles = new Vector3(ArrowImage.transform.eulerAngles.x, ArrowImage.transform.eulerAngles.y, tracker.transform.eulerAngles.z);
            }

            if (selectionActive)
            {
                switch (markerSwitch.GetFaceFront())
                {
                    case "Marker5":
                        //Gogointeraction
                        if (listIT.ContainsKey("GoGoInteraction"))
                        {
                            this.DisableAll();
                            ((Behaviour)listIT["GoGoInteraction"]).enabled = true;

                            selectionText.text = "GoGoInteraction";
                        }
                        break;
                    case "Marker0":
                        //Homerinteraciotn
                        if (listIT.ContainsKey("HomerInteraction"))
                        {
                            this.DisableAll();
                            ((Behaviour)listIT["HomerInteraction"]).enabled = true;


                            selectionText.text = "HomerInteraction";
                        }
                        break;
                    case "Marker4":
                        //virtualhandinteraction
                        if (listIT.ContainsKey("VirtualHandInteraction"))
                        {
                            this.DisableAll();
                            ((Behaviour)listIT["VirtualHandInteraction"]).enabled = true;

                            selectionText.text = "VirtualHandInteraction";
                        }
                        break;
                    default:
                        break;
                }

                //check for key
                if (Input.GetButtonDown("Fire1"))
                {
                    //selectionActive = false;
                    GUICanvas.enabled = false;
                    //selectionText.enabled = false;
                    selectionActive = false;
                }
            }
        }
        //else
        // show virtual hand -> physical hand is autmatically rendert due to tracking state
        //tracker.transform.parent.GetComponent<TrackBase>().setVisability(gameObject, false);



        // ------------------ VRUE Tasks END ----------------------------
    }

    protected void DisableAll()
    {
        foreach (Behaviour c in listIT.Values)
        {
            ((Behaviour)c).enabled = false;
        }
    }

    /* ------------------ VRUE Tasks START -------------------
    ----------------------------------------------------------------- */

    //too much whitespace




    // ------------------ VRUE Tasks END ----------------------------

}
