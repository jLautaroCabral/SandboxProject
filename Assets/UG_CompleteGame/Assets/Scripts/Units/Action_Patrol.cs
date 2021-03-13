using UnityEngine;
using System.Collections;

public class Action_Patrol : UGAction {
	//linking unit actions to GUI


	//Minor Alterations
	//added static bool to unit order script to temporarily stop units getting orders whilst using this script + method to make sure it isnt stuck off when there isn't anything selected
	//made die in unit health public and added line to remove units from the active unit list when they die
	//added methods to clear out current orders in unit list
	//changed actions units can do to make sure that they can do the new actions

	//New Features
	//added method to draw buttons
	//added array for types of button actions
	//added virtual methods for patrolling, scouting and deleting a unit


	public Vector3 positionWeAreMovingTo,startingPosition,endposition;
	public bool setPatLoc = false; 
	public bool loop = true; //whether we repeat the action
	UnitMovement um;
	UnitMasterClass umc;
	void Awake()
	{
		um = this.GetComponent<UnitMovement> ();
		multiPartAction = true;
		startingPosition = this.transform.position;
		umc = this.GetComponent<UnitMasterClass> ();
	}


	public override void doAction ()
	{
		
		if (setPatLoc == false) {
			if (this.GetComponent<FactionIdentifier> () == false) {
				UnitOrderScript.canGiveOrders = false;
			}

			if (Input.GetMouseButtonDown (1)) {
				Vector3 mousePos = Input.mousePosition;
				Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint (mousePos);
				mouseInWorld.z = 0;

				TileMasterClass tm = GridGenerator.me.getTile ((int)mouseInWorld.x, (int)mouseInWorld.y);

				endposition = tm.gameObject.transform.position;
				positionWeAreMovingTo = endposition;
				setPatLoc = true;
			}
		} else {
			if (this.GetComponent<FactionIdentifier> () == false) {
				UnitOrderScript.canGiveOrders = true;
			}


			if (um.areWeMoving==false && um.waitingForPath==false) {
				um.moveToLocation (positionWeAreMovingTo);
			}

			if (umc.actionsQueue.Count > 1) {
				loop = false;
			}
			//Debug.LogError ("moveing " + Vector3.Distance (positionWeAreMovingTo, this.transform.position));
			if (Vector3.Distance (positionWeAreMovingTo, this.transform.position) < 2.5f) {
				//um.areWeMoving = false;
				if (positionWeAreMovingTo == endposition) {
					positionWeAreMovingTo = startingPosition;
				} else {
					positionWeAreMovingTo = endposition;
				}
				um.moveToLocation (positionWeAreMovingTo);
			}
		}
	}

	public override bool getIsActionComplete()
	{
		return !loop;//if we do want to loop we say the action is not complete cause we don't want to finish
	}

	public override string getActionType()
	{
		return "Patrol";
	}




}
