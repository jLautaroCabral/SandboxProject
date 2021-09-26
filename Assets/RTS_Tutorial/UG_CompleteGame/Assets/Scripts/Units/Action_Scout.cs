using UnityEngine;
using System.Collections;

public class Action_Scout : UGAction {
	
	UnitMovement um;
	UnitMasterClass umc;
	Vector3 pos;
	bool loop = true;
	void Awake()
	{
		um = this.GetComponent<UnitMovement> ();
		multiPartAction = true;
		umc = this.GetComponent<UnitMasterClass> ();
		pos = getPos ();
	}

	Vector3 getPos()
	{

		int lowX = 0, lowY = 0, highX = 0, highY = 0;

		if ((transform.position.x > 10)) {
			lowX = (int)transform.position.x - 10;
		} else {
			lowX = 0;
		}

		if ((transform.position.y > 10)) {
			lowY = (int)transform.position.y - 10;
		} else {
			lowY = 0;
		}

		if (transform.position.x < GridGenerator.me.gridDimensions.x-10) {
			highX = (int)transform.position.x + 10;
		} else {
			highX = (int)GridGenerator.me.gridDimensions.x;
		}

		if (transform.position.y < GridGenerator.me.gridDimensions.y-10) {
			highY = (int)transform.position.y + 10;
		} else {
			highY = (int)GridGenerator.me.gridDimensions.y;
		}



		int x = Random.Range (lowX, highX);
		int y = Random.Range (lowY, highY);

		TileMasterClass tm = GridGenerator.me.getTile (x, y);

		if (tm.isTileWalkable () == true) {
			return tm.transform.position;
		} else {
			return getPos();
		}
	}

	public override void doAction ()
	{
		if (um.areWeMoving == false && um.waitingForPath == false) {
			um.moveToLocation(pos);
		}

		if (umc.actionsQueue.Count > 1) {
			loop = false;
		}


		if (Vector3.Distance (this.transform.position, pos) < 2.5f) {
			pos = getPos ();
			um.moveToLocation (pos);
		}
	}

	public override bool getIsActionComplete()
	{
		return !loop;//if we do want to loop we say the action is not complete cause we don't want to finish
	}

	public override string getActionType()
	{
		return "Scout";
	}
}
