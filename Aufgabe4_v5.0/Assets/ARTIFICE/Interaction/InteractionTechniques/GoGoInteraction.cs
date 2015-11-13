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
/// Class to select and manipulate scene objects with gogo interaction technique (IT). 
/// 
/// GoGo is a 1st person view IT
/// </summary>
public class GoGoInteraction : ObjectSelectionBase
{
	/* ------------------ VRUE Tasks START -------------------
	* 	Implement GoGo interaction technique
	----------------------------------------------------------------- */



    /* ------------------ VRUE Tasks START -------------------
    * 	Implement GoGo interaction technique
    ----------------------------------------------------------------- */

    public GameObject trackerObject;
    public GameObject interactionObject;
    public GameObject marker;

    GameObject virtualHand;
    GameObject physicalHand;

    TrackMarker trackMarker;

    public float thresholdDistance = 5.0f; // D
    public float coefficient = 0.5f;       // k
    //GOGO: when distance > distancethreshold : position = distance + k * (distance - threshold) ^ 2
    //gogo tldr: extend hand exponentially if outside radius r

    GameObject torso;

    //change to void OnEnable()
    void Start()
    {
        //marker = GameObject.Find("Marker1");
        trackMarker = marker.GetComponent<TrackMarker>();
        //markerVis = GameObject.Find("MarkerVisualisation");
        trackerObject = GameObject.Find("TrackerObject");
        interactionObject = GameObject.Find("InteractionObject");

        torso = GameObject.Find("InteractionOrigin");

        virtualHand = this.gameObject;
        //physicalHand = GameObject.Find("PhysicalHand");
        physicalHand = trackerObject;
    }

    //void Update()
    protected override void UpdateSelect()
    {
        //trackerObject.transform.position = new Vector3(0, 0, 0);
        if (trackerObject)
        {
            //check if marker is tracked:
            if (trackMarker.isTracked())
            {
                // show virtual hand -> physical hand is autmatically rendert due to tracking state
                trackerObject.transform.parent.GetComponent<TrackBase>().setVisability(gameObject, true);

                //Update transform of the selector object (virtual hand)
                this.transform.rotation = trackerObject.transform.rotation;

                //
                Vector3 origin = torso.transform.position;//new Vector3(target.transform.position.x, virtualCamera.transform.position.y, target.transform.position.z) ;//new Vector3(0, 0, 0);// target.transform.position;
                Vector3 physicalHandPosition = physicalHand.transform.position;


                //float distance = Vector3.Distance(target.transform.position, trackerObject.transform.position);
                float distance = Vector3.Distance(origin, physicalHandPosition);

                if (distance > thresholdDistance)
                {

                    //Debug.Log("distance:" + distance);
                    float gogoDistance = distance + coefficient * Mathf.Pow(distance - thresholdDistance, 2);

                    //Debug.Log("gogo distance:" + gogoDistance);

                    //extend to target:
                    Vector3 trackObjPosVec = physicalHandPosition - origin;

                    //float curDist = Vector3.Distance(physicalHandPosition, origin);

                    trackObjPosVec *= (gogoDistance / distance);

                    virtualHand.transform.position = trackObjPosVec + origin;

                    if (selected)
                    {
                        this.transformInter(this.transform.position, this.transform.rotation);
                    }
                }
                else
                {
                    this.transform.position = trackerObject.transform.position;

                }
            }
            else
            {
                //Debug.Log("Set virtual hand invisible hand");
                // show virtual hand -> physical hand is autmatically rendert due to tracking state
                trackerObject.transform.parent.GetComponent<TrackBase>().setVisability(gameObject, false);

            }

        }

    }

    // ------------------ VRUE Tasks END ----------------------------

    /// <summary>
    /// Callback
    /// If our selector-Object collides with anotherObject we store the other object 
    /// 
    /// For usability purpose change color of collided object
    /// </summary>
    /// <param name="other">GameObject giben by the callback</param>
    public void OnTriggerEnter(Collider other)
    {

        //Debug.Log("onTriggerEnter");

        if (isOwnerCallback())
        {
            GameObject collidee = other.gameObject;

            if (hasObjectController(collidee))
            {

                collidees.Add(collidee.GetInstanceID(), collidee);
                //Debug.Log(collidee.GetInstanceID());

                // change color so user knows of intersection
                collidee.renderer.material.SetColor("_Color", Color.blue);
            }
        }
    }

    /// <summary>
    /// Callback
    /// If our selector-Object moves out of anotherObject we remove the other object from our list
    /// 
    /// For usability purpose change color of collided object
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("onTriggerExit");

        if (isOwnerCallback())
        {
            GameObject collidee = other.gameObject;

            if (hasObjectController(collidee))
            {
                collidees.Remove(collidee.GetInstanceID());

                // change color so user knows of intersection end
                collidee.renderer.material.SetColor("_Color", Color.white);
            }
        }
    }
}