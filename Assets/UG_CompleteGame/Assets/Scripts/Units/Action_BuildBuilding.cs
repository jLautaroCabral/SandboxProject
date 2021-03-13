using UnityEngine;
using System.Collections;

public class Action_BuildBuilding : UGAction {
	//created this action script
	// made changes to the worker to allow for having a build button appear
	//added stuff to building manager to store the workers selected whilst placing the building
	//added stuff to building (resource cost, health, isBuilt)
	//made create building method in selection manager return the building

	public bool atBuilding = false;
	public bool movingToBuilding = false;
	GameObject building;
	Building bl;
	Vector3 buildingPos;


	float buildTimer = 0.5f;
	public override void initaliseTarget (GameObject target)
	{

		multiPartAction = true; //new bool added to action parent, to mark actions where the doAction method must be called every frame
		building = target;	
		bl = building.GetComponent<Building> ();
		buildingPos = bl.getGoToTile ().transform.position;

		if (bl.name.Equals ("House")) { //need to add here so that the AI doesn't build them endlessly
			PlayerPopulationManager.me.addHouse (building);
		}
	}

	public override void reinitialiseAction ()
	{
		initaliseTarget (building);
	}



	public override void doAction ()
	{
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
		return bl.built;
	}

	public override string getActionType ()
	{
		return "BuildBuilding";
	}
}
