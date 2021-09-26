using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAction_CreateHoplite : BuildingAction {

	public override void doAction ()
	{
		if (timer > 0) {
			timer -= Time.deltaTime;
		} else {
			timer = 0.0f;
		}
	}


	public override void startAction ()
	{
		if (this.GetComponentInParent<FactionIdentifier> () == false) {
			timer *= UpgradeValues.me.getHopliteBuildTimeMod ();
			ResourceManager.me.ReduceResources ("food", foodCost);
			ResourceManager.me.ReduceResources ("iron", ironCost);
			started = true;
		} else {
			timer *= 1;
			started = true;
		}
	}

	public override void onComplete ()
	{
		if (this.GetComponentInParent<FactionIdentifier> () == false) {
			
			Building b = this.GetComponentInParent<Building> ();
			Vector3 spawnPos = b.getGoToTile ().transform.position;
			GameObject g = (GameObject)Instantiate (prefabToSpawn, spawnPos, Quaternion.Euler (0, 0, 0));
			UnitManager.me.addUnit (g);
		} else {
			Building b = this.GetComponentInParent<Building> ();
			Vector3 spawnPos = b.getGoToTile ().transform.position;
			GameObject g = (GameObject)Instantiate (prefabToSpawn, spawnPos, Quaternion.Euler (0, 0, 0));
			FactionIdentifier fi = g.AddComponent<FactionIdentifier> ();
			fi.myFaction = this.GetComponentInParent<FactionIdentifier> ().myFaction;
			fi.myFaction.myUnits.hoplites.Add (g);
			g.GetComponent<SpriteRenderer> ().color = fi.myFaction.myColor;
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

	public override bool areWeDone ()
	{
		if (timer <= 0) {
			return true;
		} else {
			return false;
		}
	}

	public override string getButtonText ()
	{


		if (canWeDo () == true) {
			return "Create Hoplite : "  +foodCost + " Food";
		} else {
			return "Insufficient resources to " + "Create Hoplite : "  +foodCost + " Food";
		}
	}

	public override string getProgress ()
	{
		return "Creating Hoplite : " + timer;
	}
}
