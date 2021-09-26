using UnityEngine;
using System.Collections;
public class UnitOrderScript : MonoBehaviour {

	public static bool canGiveOrders = true;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (canGiveOrders == true) {
			commandUnitsToMove ();
		}
		orderCheck ();
	}

	void orderCheck()
	{
		if (areAnyUnitsSelected () == false && canGiveOrders==false) {
			canGiveOrders = true;
		}
	}

	void commandUnitsToMove()
	{
		if (areAnyUnitsSelected() == true) {
			if (Input.GetMouseButtonDown (1)) {

				//section below this to the else section is a raycast checking for whether the user has right clicked on a mouse
				Vector2 mousePosRay = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

				GameObject hitObject=null;
				string objectTag = "";
				RaycastHit2D raycast = Physics2D.Raycast (mousePosRay, Vector2.zero, 0f);
				try {
					hitObject = raycast.collider.gameObject;
					objectTag=hitObject.tag;

				} catch {
					Debug.Log ("Nothing Hit");
				}
			

				if (objectTag == "Resource") {
					Debug.Log ("Hit a resource");
					Vector3 mousePos = Input.mousePosition;
					Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint (mousePos);

					TileMasterClass tm = GridGenerator.me.getTile ((int)mouseInWorld.x, (int)mouseInWorld.y);
					if (tm != null) {
						foreach (GameObject g in SelectionManager.me.getSelected()) {
							if (g.GetComponent<UnitMasterClass> () != null) {
								UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
								UGAction a = g.AddComponent<Action_GatherResource> ();
						
								if (um.canWePerformAction (a) == true) { //added check to make sure we can perform the action
									a.initaliseLocation (mouseInWorld);
									um.actionsQueue.Add (a);
									Action_GatherResource res = g.GetComponent<Action_GatherResource> (); //new lines to add the type of resource to the action_gather resource
									res.resourceType = hitObject.GetComponent<Resource> ().myType;
									a.enabled = false;
								} else {
									Destroy (a);
									moveUnitToLocation (mouseInWorld, g);
								}
							}
						}
					}
				} else if (objectTag == "Unit") {
					Debug.Log ("Hit a Unit");
					Vector3 mousePos = Input.mousePosition;
					Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint (mousePos);

					TileMasterClass tm = GridGenerator.me.getTile ((int)mouseInWorld.x, (int)mouseInWorld.y);
					if (tm != null) {
						foreach (GameObject g in SelectionManager.me.getSelected()) {
							if (g.GetComponent<UnitMasterClass> () != null) {
								UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
								/*Action a = g.AddComponent<Action_AttackUnit> ();

								if (um.canWePerformAction (a) == true && hitObject != null) { //check for if the hit object is null just to be safe
									a.initaliseTarget (hitObject);
									um.actionsQueue.Add (a);
									a.enabled = false;
								} else {
									Destroy (a);
									moveUnitToLocation (mouseInWorld, g);
								}*/

								um.myAttack.DEBUG_setTarget (hitObject);
							}
						}
					}
				} else if (objectTag == "Building") {
					Debug.Log ("Hit a Building");
					Building b = hitObject.GetComponent<Building> ();
					Vector3 pos = b.getGoToTile ().transform.position;


						foreach (GameObject g in SelectionManager.me.getSelected()) {
							if (b.built == false) {
								if (g.GetComponent<UnitMasterClass> () != null) {
									UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
									UGAction a = g.AddComponent<Action_BuildBuilding> ();

									if (um.canWePerformAction (a) == true && hitObject != null) { //check for if the hit object is null just to be safe
										a.initaliseTarget (hitObject);
										um.actionsQueue.Add (a);
										a.enabled = false;
									} else {
										Destroy (a);
										moveUnitToLocation (pos, g);
									}
								}
							} else {
								moveUnitToLocation (pos, g);
							}
						}
				}
				else{
					Debug.Log ("Did not hit a resource");
					Debug.Log (objectTag);
					Vector3 mousePos = Input.mousePosition;
					Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint (mousePos);
					mouseInWorld.z = 0;

					TileMasterClass tm = GridGenerator.me.getTile ((int)mouseInWorld.x,(int)mouseInWorld.y);
					if (tm != null) {
						//Debug.Log (tm.name);
						foreach (GameObject g in SelectionManager.me.getSelected()) {
							moveUnitToLocation (mouseInWorld, g);
						}
					}
				}



			}

		}
	}

	void moveUnitToLocation(Vector3 mouseInWorld,GameObject g){ //moved the move to location code into its own function so we can call it as a default option if a unit can't interact with what has been clicked on e.g. resources
		if (g.GetComponent<UnitMasterClass> () != null) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			UGAction a = g.AddComponent<Action_MoveToLocation> ();
			//	Debug.Log (um.canWePerformAction (a));
			if (um.canWePerformAction (a) == true) { //added check to make sure we can perform the action
				a.initaliseLocation (mouseInWorld);
				um.actionsQueue.Add (a);
				a.enabled = false;
			} else {
				Destroy (a);
			}
		}
	}

	bool areAnyUnitsSelected()
	{
		if (SelectionManager.me.selectionMode == selectingModes.units && SelectionManager.me.getSelected ().Count > 0) {
			return true;
		} else {
			return false;
		}
	}
}
