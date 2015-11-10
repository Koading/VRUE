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
using System.Collections;


/// <summary>
/// Class to select and manipulate scene objects with HOMER interaction technique (IT). 
/// 
/// HOMER is a 1st person view IT
/// </summary>
public class HomerInteraction : ObjectSelectionBase
{
    /* ------------------ VRUE Tasks START -------------------
    * 	Implement Homer interaction technique
    ----------------------------------------------------------------- */

    LineRenderer rayCastVis;


    public GameObject trackerObject;
    public GameObject interactionObject;
    public GameObject target;
    public GameObject marker;
    public GameObject markerVis;

    GameObject torso;

    GameObject virtualHand;
    GameObject physicalHand;

    TrackMarker trackMarker;

    public bool multiSelect;
    bool lastFrameCollissionDetected;


    GameObject selectedObject;
    Vector3 o;
    GameObject raycastOrigin;


    void Start()
    {
        marker = GameObject.Find("Marker1");
        trackMarker = marker.GetComponent<TrackMarker>();
        markerVis = GameObject.Find("MarkerVisualisation");
        trackerObject = GameObject.Find("TrackerObject");
        interactionObject = GameObject.Find("InteractionObject");

        torso = GameObject.Find("InteractionOrigin");

        virtualHand = this.gameObject;
        physicalHand = GameObject.Find("PhysicalHand");

        target = GameObject.Find("Target");

        multiSelect = true;
        lastFrameCollissionDetected = false;

        //raycastOrigin = torso.transform.position;
        raycastOrigin = physicalHand;
    }


    protected override void UpdateSelect()
    {
        if (trackerObject)
        {
            if (trackMarker.isTracked())
            {
                trackerObject.transform.parent.GetComponent<TrackBase>().setVisability(gameObject, true);

                //Update transform of the selector object (virtual hand)
                this.transform.rotation = trackerObject.transform.rotation;

                if (!rayCastVis)
                {
                    //code works here
                    rayCastVis = virtualHand.AddComponent<LineRenderer>();//this.gameObject.AddComponent<LineRenderer>();

                    rayCastVis.SetWidth(0.2f, 0.2f);

                    rayCastVis.SetColors(Color.green, Color.green);
                }

                if (rayCastVis)
                {
                    //Vector3 dir = physicalHand.transform.position + physicalHand.transform.localPosition;

                    //incidently returns a vector size 1 pointing forward
                    //250 is arbitrarily chosen, as it looks good enough
                    Vector3 dir = physicalHand.transform.forward * 250;

                    //rayCastVis.SetPosition(0, raycastOrigin);
                    //rayCastVis.SetPosition(1, torso.transform.forward * 250);

                    rayCastVis.SetPosition(0, raycastOrigin.transform.position);
                    rayCastVis.SetPosition(1, dir);

                    //rayCastVis.collider.enabled = true;
                    if (!this.RayCast(dir))
                    {
                        RemoveObjectCollider();
                    }

                }

                if (selected)
                {
                    Debug.Log("Selected");

                    //set virtual hand to object:
                    /*
                    Vector3 t = torso.transform.position;
                    Vector3 h = physicalHand.transform.position;

                    float dist_h = Vector3.Distance(h, t);

                    float dist_o = Vector3.Distance(o, t);

                    Vector3 hcurr = physicalHand.transform.position;
                    float dist_hcurr = Vector3.Distance(hcurr, t);

                    float dist_vh = dist_hcurr * (dist_o / dist_h);
                    Vector3 thcurr = (hcurr - t).normalized;

                    Vector3 vh = t + dist_vh * (thcurr);

                    this.transform.position = vh;//trackerObject.transform.position; //t + dvh + (thcurr)
                    */

                    //this.transform.position = o;
                    //this.transformInter(this.transform.position, this.transform.rotation);
                    //this.transform.position = selectedObject.transform.position;
                    this.transform.position = trackerObject.transform.position;
                    this.transformInter(this.physicalHand.transform.position, this.transform.rotation);


                }
                else
                {
                    this.transform.position = trackerObject.transform.position;
                }
            }

            else
            {
                trackerObject.transform.parent.GetComponent<TrackBase>().setVisability(gameObject, false);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Change mode");
            multiSelect = !multiSelect;

            Debug.Log("MultiSelect" + multiSelect);
        }
    }

    bool RayCast(Vector3 dir)
    {
        bool hasHit = false;
        //if(Physics.Raycast(torso.transform.position, dir, out hit, float.PositiveInfinity))
        if (!multiSelect)
        {
            RaycastHit hit;
            //hasHit = Physics.Raycast(physicalHand.transform.position, dir, out hit, float.PositiveInfinity);
            hasHit = Physics.Raycast(raycastOrigin.transform.position, dir, out hit, float.PositiveInfinity);
            if (hasHit)
            {
                lastFrameCollissionDetected = true;

                AddObjectCollider(hit.collider);
                //hit.collider.renderer.material.SetColor("_Color", Color.blue);
            }
            else
            {
                //RemoveObjectCollider();
            }
        }
        else if (multiSelect)
        {
            RaycastHit[] hits;
            //hits = (Physics.RaycastAll(physicalHand.transform.position, dir, float.PositiveInfinity));
            hits = (Physics.RaycastAll(raycastOrigin.transform.position, dir, float.PositiveInfinity));
            if (hits.Length > 0)
            {
                hasHit = true;

                foreach (RaycastHit hit in hits)
                {
                    lastFrameCollissionDetected = true;

                    AddObjectCollider(hit.collider);
                }
            }
        }

        return hasHit;
    }

    void OnEnable()
    {

        //this code does not work in combination on being activated from ITSelectionGUI. Linerenderer component returns null
        //it works when i activate it manually (as in check the enabled box in inspector on runtime) but not when it is activated in the ITSelectionGUI script
        //i copied the exact! same! code to UpdateSelect. 
        //
        //
        /*
        
        Debug.Log("Call OnEnable");
        Debug.Log(this.gameObject.GetType());

        rayCastVis = virtualHand.AddComponent<LineRenderer>();//this.gameObject.AddComponent<LineRenderer>();

        Debug.Log(rayCastVis);

        rayCastVis.SetWidth(0.2f, 0.2f);
         
        rayCastVis.SetColors(Color.green, Color.green);
        */
    }

    void OnDisable()
    {
        //Debug.Log("Destroyed raycast");
        if (rayCastVis)
        {
            rayCastVis.enabled = false;

            GameObject.Destroy(rayCastVis);
            rayCastVis = null;
        }
    }

    void AddObjectCollider(Collider c)
    {

        if (isOwnerCallback())
            return;

        GameObject collidee = c.gameObject;

        //single mode
        if (!multiSelect
            && !collidees.Contains(collidee.GetInstanceID()))
        {
            //if last hit already has an item and a different one is selected, remove old item(s) and add new one
            RemoveObjectCollider();

        }

        if (collidee != null
            && hasObjectController(collidee)
            && !collidees.Contains(collidee.GetInstanceID()))
        {
            selectedObject = collidee;
            o = collidee.transform.position;

            collidees.Add(collidee.GetInstanceID(), collidee);

            // change color so user knows of intersection end
            collidee.renderer.material.SetColor("_Color", Color.blue);
        }
    }

    void RemoveObjectCollider()
    {
        if (isOwnerCallback())
            return;

        ArrayList keys = new ArrayList(this.collidees.Keys);

        foreach (int c in keys)
        {
            GameObject collidee = this.collidees[c] as GameObject;
            //GameObject go = collidee.gameObject;

            if (hasObjectController(collidee))
            {
                Debug.Log("unselect Object");
                collidees.Remove(c);

                // change color so user knows of intersection end
                collidee.renderer.material.SetColor("_Color", Color.white);
            }
        }
    }

    // ------------------ VRUE Tasks END ----------------------------
}