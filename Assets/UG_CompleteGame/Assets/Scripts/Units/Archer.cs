using UnityEngine;
using System.Collections;

public class Archer : UnitMasterClass {


	void Awake()
	{
		base.Awake ();
		myHealth.health = UpgradeValues.me.getArcherHealth ();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		queueMoniter ();
	}

	public override bool canWePerformAction(UGAction ac)
	{
		if (ac.getActionType ().Equals ("Movement")) {
			return true;
		} else if(ac.getActionType ().Equals ("AttackUnit")) {
			return true;
		}else if (ac.getActionType ().Equals ("Default")) {
			return false;
		} else if (ac.getActionType ().Equals ("Patrol")) {
			return true;
		}else if (ac.getActionType ().Equals ("Scout")) {
			return true;
		}  else {
			return false;
		}
	}
}
