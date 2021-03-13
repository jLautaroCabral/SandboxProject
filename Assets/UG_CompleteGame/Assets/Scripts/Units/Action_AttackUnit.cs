 using UnityEngine;
using System.Collections;

public class Action_AttackUnit : UGAction {

	public GameObject target;
	public bool loop=true;

	float pathRefreshTimer=0.0f;
	public float pathRefreshReset = 1.5f;

	public float range;
	float attackTimer=0.0f;
	UnitMasterClass me;
	public float dist;
	public override void initaliseTarget (GameObject g)
	{
		if (isThisAValidTarget (g)==true) {
			target = g;
			multiPartAction = true;
			me = this.GetComponent<UnitMasterClass> ();
			range = me.myAttack.attackRange;
		} else {
			//destroy action
			loop=false;
		}

	}

	public override void reinitialiseAction ()
	{
		initaliseTarget (target);
	}

	public bool isThisAValidTarget(GameObject g)
	{
		if (this.gameObject.GetComponent<FactionIdentifier> ()==null) {
			//unit attacking is a player unit
			if (g.GetComponent<FactionIdentifier> () != null) {
				return true;
			} else {
				return false;
			}
		} else {
			if (g.GetComponent<FactionIdentifier> () != null) {
				if (g.GetComponent<FactionIdentifier> () != this.gameObject.GetComponent<FactionIdentifier> ()) {
					return true;
				} else {
					return false;
				}
			}
		}

			return true;	
	}

	public override void doAction ()
	{
		if (target == null) {///new bit 28/05/17
			targetDeadMoniter ();
			return;
		}

			Vector2 myPos = new Vector2 (transform.position.x, transform.position.y);
			Vector2 tarPos = new Vector2 (target.transform.position.x, target.transform.position.y);


			dist=Vector2.Distance (myPos, tarPos);



			if (nearTarget() == false) {
				targetFollow ();
			} else {
				//attackTarget
				attackTarget();
			}
		targetDeadMoniter ();
	}

	void targetDeadMoniter()
	{
		if (target.GetComponent<UnitHealth> ().dead == true || target==null) {
			loop = false;
		}
	}

	public void attackTarget()
	{
		
		if (attackTimer > 0.0f) {
			attackTimer -= Time.deltaTime;
		} else {
			me.myAttack.attack (target);

//				Debug.LogError ("AttackEnemy");
				attackTimer = me.myAttack.attackRate;
			
		}
	}

	public void targetFollow()
	{
		pathRefreshTimer -= Time.deltaTime;

		if (pathRefreshTimer <= 0) {
			this.GetComponent<UnitMovement> ().moveToLocation (target.transform.position);
			pathRefreshTimer = pathRefreshReset;
		}
	}

	bool nearTarget()
	{
		Vector2 myPos = new Vector2 (transform.position.x, transform.position.y);
		Vector2 tarPos = new Vector2 (target.transform.position.x, target.transform.position.y);

		if (Vector2.Distance (myPos, tarPos) < range) {
			return true;
		} else {
			return false;
		}
	}

	public override bool getIsActionComplete()
	{
		return !loop;//if we do want to loop we say the action is not complete cause we don't want to finish
	}

	public override string getActionType()
	{
		return "AttackUnit";
	}
}
