using UnityEngine;
using System.Collections;

public class Action_MoveToLocation : UGAction {
	//example action, moves the unit that the action is assigned to to a location given using the already written pathfinding class
	Vector3 positionWeAreMovingTo;
	UnitMovement um;


	public override void initaliseLocation(Vector3 position)
	{
		positionWeAreMovingTo = position;
	}

	public override void doAction ()
	{
		um = this.GetComponent<UnitMovement> ();
		um.moveToLocation (positionWeAreMovingTo);
	}

	public override void reinitialiseAction ()
	{
		initaliseLocation (positionWeAreMovingTo);
	}

	public override bool getIsActionComplete()
	{
		if (um.areWeMoving == true) {
			Vector2 myPos = new Vector2 (this.transform.position.x, this.transform.position.y);
			Vector2 tarPos = new Vector2 (um.getFinalPosition ().x, um.getFinalPosition ().y);
			if (Vector2.Distance (tarPos, myPos) < 2.0f) {
				return true;
			} else {
				//Debug.LogError(Vector2.Distance(tarPos,myPos));
				return false;
			}
		} else {
			return false;
		}
	}

	public override string getActionType()
	{
		return "Movement";
	}
}
