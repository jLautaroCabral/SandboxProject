using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_GatherResources : UGAction {

	public Vector3 positionOfResource;
	public float resourceTimer = 5.0f;//how long the unit gathers the resource 
	public Vector3 positionOfStorehouse;

	//action states
	public bool movingToResource = false; //moving to resource that the player right clicked on
	public bool gatheredResource = false; //at the location of the resource, gathering
	public bool movingToStorehouse = false; //gathered the resource, moving to the storehouse
	public bool storedResource = false; //have we reached the storehouse
	public bool loop = true; //whether we repeat the action


	//these two bools just to display in the inspector whether the unit has reached the resource/storehouse
	public bool atRes = false;
	public bool atStore = false;
	public string resourceType = "";

	Faction myFaction;
	public void initaliseLocation(Vector3 position,string resourceType)
	{
		
		myFaction = this.gameObject.GetComponent<FactionIdentifier> ().myFaction;
		this.resourceType = resourceType;
		multiPartAction = true; //new bool added to action parent, to mark actions where the doAction method must be called every frame

		GameObject resourceGameobject = ResourceStore.me.getNearestResource(this.transform.position,resourceType);

		if (resourceGameobject == null) {
			loop = false;
			positionOfResource = this.transform.position;
			Debug.LogError ("AI COULD NOT FIND RESOURCE " + resourceType);
		} else {
			positionOfResource = resourceGameobject.transform.position;
		}

		try{
			positionOfStorehouse = myFaction.myBuildings.getNearestBuildingOfType("Storehouse", this.gameObject.transform.position).GetComponent<Building>().getGoToTile().transform.position;
		}
		catch{
			loop = false;//since there isnt a storehouse, we stop the action
		}
	}

	public override void doAction ()
	{
		if (actionStarted == false) {
			resetAction ();
		}


		atRes = atResource ();
		atStore = atStorehouse ();

		if (atResource () == false && movingToResource==false) {
			moveToResource ();
		}

		if (atResource () == true && gatheredResource == false) {
			gatherResource ();
		}

		if (atStorehouse () == false && gatheredResource == true) {
			moveToStorehouse ();
		}

		if (atStorehouse () == true && gatheredResource == true) {
			storeResources ();
		}

	}

	void moveToResource()
	{
		if (movingToResource == false) {
			UnitMovement um = this.GetComponent<UnitMovement> ();
			um.moveToLocation (positionOfResource);
			movingToResource = true;
		}
	}

	bool atResource() //using vector2.distance to make sure z axis is ignored
	{
		Vector2 myPos = new Vector2 (this.transform.position.x, this.transform.position.y);
		Vector2 targetPos = new Vector2 (positionOfResource.x, positionOfResource.y);

		if (Vector2.Distance (myPos, targetPos) < 2.0f) {
			movingToResource = true;
			return true;
		} else {
			return false;
		}
	}

	bool atStorehouse()
	{
		Vector2 myPos = new Vector2 (this.transform.position.x, this.transform.position.y);
		Vector2 targetPos = new Vector2 (positionOfStorehouse.x, positionOfStorehouse.y);

		if (Vector2.Distance (myPos, targetPos) < 2.0f) {
			movingToStorehouse = false;
			return true;
		} else {
			return false;
		}
	}

	void gatherResource()
	{
		resourceTimer -= Time.deltaTime;

		if (resourceTimer <= 0) {
			gatheredResource = true;
			resourceTimer = 5.0f;
		}
	}

	void moveToStorehouse()
	{
		if (movingToStorehouse == false) {
			UnitMovement um = this.GetComponent<UnitMovement> ();
			um.moveToLocation (positionOfStorehouse);
			movingToStorehouse = true;
		}
	}

	void storeResources()
	{
		UnitMasterClass um = this.GetComponent<UnitMasterClass> ();

		if (um.actionsQueue.Count > 1) {//if there have been actions queued up then we go to the next one else we repeat the action
			loop = false;
		} else {
			loop = true;
		}
		myFaction.myResources.IncreaseResources (resourceType, 100);
		resetAction ();
	}

	void resetAction()
	{
		movingToResource = false;
		gatheredResource = false;
		movingToStorehouse = false;
		storedResource = false;
	}

	public override bool getIsActionComplete()
	{
		if (loop == false) {
			Debug.Log ("AI FINISHED GATHERING RESOURCES");
		}
		return !loop;//if we do want to loop we say the action is not complete cause we don't want to finish
	}

	public override string getActionType()
	{
		return "ResourceGather";
	}
}
