using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_BuildBuilding : UGAction
{

	public bool atBuilding = false;
	public bool movingToBuilding = false;
	GameObject building;
	Building bl;
	Vector3 buildingPos;
	Faction myFaction;
	float buildTimer = 0.5f;
	public override void initaliseTarget (GameObject target)
	{
		multiPartAction = true; //new bool added to action parent, to mark actions where the doAction method must be called every frame
		building = target;	
		bl = building.GetComponent<Building> ();
		buildingPos = bl.getGoToTile ().transform.position;
		myFaction = this.GetComponent<FactionIdentifier> ().myFaction;
	}



	public override void doAction ()
	{
		if (actionStarted == false) {
			atBuilding = false;
			movingToBuilding = false;
		}

		if (Vector3.Distance (this.gameObject.transform.position, buildingPos) < 2.5f && atBuilding == false) {
			atBuilding = true;
		}

		if (atBuilding == false) {
			if (movingToBuilding == false) {
				this.GetComponent<UnitMovement> ().moveToLocation (buildingPos);
				movingToBuilding = true;
			}
		} else {
			buildTimer -= Time.deltaTime;
			if (buildTimer <= 0) {
				bl.increaseBuildingHealth ();
				buildTimer = 0.5f;
			}
		}
	}

	public override bool getIsActionComplete () //returns true if the building is finished
	{
		//return bl.built;

		if (bl.built == false) {
			return false;
		} else {

			if (bl.gameObject.GetComponent<FactionIdentifier> () == null) {
				FactionIdentifier fi = bl.gameObject.AddComponent<FactionIdentifier> ();
				fi.myFaction = myFaction;

				//add the building to the respective faction building list
				if (myFaction.myBuildings.myBuildings.Contains (bl.gameObject) == false) {
					myFaction.myBuildings.myBuildings.Add (bl.gameObject);
				}
				bl.gameObject.GetComponent<SpriteRenderer> ().color = myFaction.myColor;
			}

			return true;
		}
	}

	public override string getActionType ()
	{
		return "BuildBuilding";
	}
}
