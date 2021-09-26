using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Worker : UnitMasterClass {
	//added methods to building store to store list of workers whilst building is being placed

	void Awake()
	{
		base.Awake ();
		myActions = new string[] { "Delete", "Patrol", "Scout", "BuildBuildings" };

		myHealth.health = UpgradeValues.me.getWorkerHealth ();
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
		} else if (ac.getActionType ().Equals ("ResourceGather")) {
			return true;
		} else if (ac.getActionType ().Equals ("Patrol")) {
			return true;
		} else if (ac.getActionType ().Equals ("Default")) {
			return false;
		} else if (ac.getActionType ().Equals ("Scout")) {
			return true;
		} else if (ac.getActionType ().Equals ("BuildBuilding")) {
			return true;
		}
		else {
			return false;
		}
	}
}
