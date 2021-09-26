using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAction_CreateWorker : BuildingAction {
	
	public override void doAction ()
	{
		if (timer >= 0) {
			timer -= Time.deltaTime;
		}
	}

	public override void startAction ()
	{
		if (this.GetComponentInParent<FactionIdentifier> () == false) {
			timer *= UpgradeValues.me.getWorkerBuildTimeMod (); //TODO MAKE IT SPECIFIC TO THE FACTION BUILDING THE WORKER
			ResourceManager.me.ReduceResources ("food", foodCost);
			started = true;
		} else {
			timer *= 1;
			//add something to reduce resources here
			started = true;
		}
	}

	public override void onComplete ()
	{

		if (this.GetComponentInParent<FactionIdentifier> () == false) {
			Building b = this.gameObject.GetComponentInParent<Building> ();
			Vector3 spawnPos = b.getGoToTile ().transform.position;
			GameObject g = (GameObject)Instantiate (prefabToSpawn, spawnPos, Quaternion.Euler (0, 0, 0));
			UnitManager.me.addUnit (g);
		} else {
			Building b = this.gameObject.GetComponentInParent<Building> ();
			Vector3 spawnPos = b.getGoToTile ().transform.position;
			GameObject g = (GameObject)Instantiate (prefabToSpawn, spawnPos, Quaternion.Euler (0, 0, 0));
			FactionIdentifier fi = this.GetComponentInParent<FactionIdentifier> ();
			fi.myFaction.myUnits.workers.Add (g);
			g.AddComponent<FactionIdentifier> ();
			g.GetComponent<FactionIdentifier> ().myFaction = fi.myFaction;
			g.GetComponent<SpriteRenderer> ().color = fi.myFaction.myColor;

		}


	}

	public override bool areWeDone ()
	{
		if (timer <= 0) {
			return true;
		} else {
			return false;
		}
	}

	public override bool canWeDo ()
	{
		if (this.GetComponentInParent<FactionIdentifier> () == false) {
			return ResourceManager.me.canWeDoBuildingAction (this) && PlayerPopulationManager.me.canWeConstructUnit ();
		} else {
			FactionIdentifier fi = this.GetComponentInParent<FactionIdentifier> ();
			return fi.myFaction.myResources.canWeDoBuildingAction (this) && fi.myFaction.myPopulation.canWeConstructUnit ();
		}
	}

	public override string getButtonText ()
	{
		if (canWeDo () == true) {
			return "Create Worker : " + foodCost + " Food";
		} else {
			return "Insufficient resources to " + "Create Worker : " + foodCost + " Food";
		}
	}

	public override string getProgress ()
	{
		return "Creating worker : " + timer;
	}
}
