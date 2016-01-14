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
		
	public GameObject tracker;
	private GameObject physicalHand = null;
	private GameObject torso = null;
	private float dh;
	private float d0;
	private LineRenderer lineRenderer;
	private bool multiple = true;
	private bool keyDownAlready = false;


	/// <summary>
	/// </summary>
	public void Start()
	{
		torso = GameObject.Find ("RigSpine3");
        physicalHand = tracker;//GameObject.Find ("PhysicalHand");

        tracker = tracker.transform.FindChild("TrackerObject").gameObject;
		// remove this as soon as GUI is ready
		lineRenderer = this.gameObject.AddComponent<LineRenderer> ();
		
		lineRenderer.SetWidth (0.02f, 0.02f);

		//////////////////////

		Debug.Log("Start Homer");
	}
	
	/// <summary>
	/// Implementation of concrete IT selection behaviour. 
	/// </summary>
	protected override void UpdateSelect()
	{


		if(tracker)
		{
			// INTERACTION TECHNIQUE THINGS ------------------------------------------------
			if (tracker.transform.parent.GetComponent<TrackBase>().isTracked())
			{
				if(this.enabled) {
					if (!keyDownAlready && Input.GetKeyDown (KeyCode.M)) {
						keyDownAlready = true;
						multiple = !multiple;
					}
					if (Input.GetKeyUp (KeyCode.M)) {
						keyDownAlready = false;
					}

					// show virtual hand -> physical hand is autmatically rendert due to tracking state
					tracker.transform.parent.GetComponent<TrackBase>().setVisability(gameObject, true);
					this.transform.rotation = tracker.transform.rotation;					


					if(lineRenderer && !selected) {
						//Update transform of the selector object (virtual hand)
						this.transform.position = tracker.transform.position;
						lineRenderer.SetPosition (0, physicalHand.transform.position);
						lineRenderer.SetPosition (1, (physicalHand.transform.position - torso.transform.position ).normalized * 99999 + physicalHand.transform.position);
						ArrayList removableCollidees = new ArrayList();							

						foreach(GameObject collidee in collidees.Values) {
							removableCollidees.Add(collidee);
						}
					
						
						if(multiple) {

							RaycastHit[] hits = Physics.RaycastAll(new Ray(physicalHand.transform.position, (physicalHand.transform.position - torso.transform.position ).normalized));
							Debug.Log ("Homer hits: "  +  hits.Length);
							if(hits.Length > 0 ) {
								for (int i = 0; i < hits.Length; i++) {
									RaycastHit hit = hits[i];
									GameObject collidee = hit.collider.gameObject;

									Debug.Log ("Collidee name: " + collidee.name);

									if(removableCollidees.Contains(collidee)) {
										removableCollidees.Remove(collidee);
									}
									
									if (isOwnerCallback() && 
                                        hasObjectController(collidee) ) {
																			
										if(!collidees.Contains(collidee.GetInstanceID())) {
											collidees.Add(collidee.GetInstanceID(), collidee);
											
											InstrumentBehaviour behaviour = collidee.GetComponent<InstrumentBehaviour>();
											if(behaviour) {
												behaviour.OnKinectTriggerStart();
											}
										}
									}
								}
								Vector3 positionOfColliddeeObject = new Vector3(0,0,0);
								
								foreach(GameObject collidee in collidees.Values) {
									positionOfColliddeeObject += collidee.transform.position;
								}

								Debug.Log("Number of elements in collidee: " + collidees.Count);
								
								positionOfColliddeeObject = positionOfColliddeeObject / collidees.Count;
								dh = (physicalHand.transform.position - torso.transform.position).magnitude;
								d0 = (positionOfColliddeeObject - torso.transform.position).magnitude;
							}
						} else  {
							RaycastHit hit;
							if(Physics.Raycast(new Ray(physicalHand.transform.position, (physicalHand.transform.position - torso.transform.position ).normalized), out hit)) {
								GameObject collidee = hit.collider.gameObject;

								if(removableCollidees.Contains(collidee)) {
									removableCollidees.Remove(collidee);
								}
								
								if( isOwnerCallback() &&
                                    hasObjectController(collidee) ) {
									if(!collidees.Contains(collidee.GetInstanceID())) {
										collidees.Add(collidee.GetInstanceID(), collidee);
									
										Debug.Log(collidee.GetInstanceID());
									
									}
								}

								dh = (tracker.transform.position - torso.transform.position).magnitude;
								d0 = (collidee.transform.position - torso.transform.position).magnitude;
							
							} 
						}

						foreach(GameObject collidee in removableCollidees) {
							if (isOwnerCallback()) { 
								collidees.Remove(collidee.GetInstanceID());

								InstrumentBehaviour behaviour = collidee.GetComponent<InstrumentBehaviour>();
								if(behaviour) {
									behaviour.OnKinectTriggerStop();
								}
							}
						}
					}
					
					
					// Transform (translate and rotate) selected object depending on of virtual hand's transformation
					if (selected)
					{
						lineRenderer.SetPosition (0, tracker.transform.position);
						lineRenderer.SetPosition (1, tracker.transform.position);
						float dhcurr = (physicalHand.transform.position - torso.transform.position).magnitude;
						float dvh = dhcurr *(d0 / dh);
						this.transform.position = torso.transform.position + dvh * (physicalHand.transform.position - torso.transform.position).normalized;

						Debug.Log("New Hand Position: " + this.transform.position);

						this.transformInter(this.transform.position, this.transform.rotation);
					}
				}
			}else 
			{
				// make virtual hand invisible -> physical hand is autmatically rendert due to tracking state
				tracker.transform.parent.GetComponent<TrackBase>().setVisability(gameObject, false);
			}
		}
		else
		{
			Debug.Log("No GameObject with name - TrackerObject - found in scene");
		}
	}
	
	
	public void onEnable() {
		lineRenderer = this.gameObject.AddComponent<LineRenderer> ();
		
		lineRenderer.SetWidth (0.02f, 0.02f);
	
	}

	public void onDisabled() {
		Destroy (lineRenderer);
	}




	// ------------------ VRUE Tasks END ----------------------------
}