using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decision_CreateFarm : Decision {
	

	public Decision_CreateFarm(FactionIdentifier fi)
	{
		myFaction = fi;
	}

	public override bool canWePerformAction()
	{
		return true;
	}

	public override void doAction()
	{
		if (canWePerformAction () ==true) {
			Building toBuild = BuildingStore.me.getBuilding ("Farm");
			Debug.Log (toBuild.name);
			myFaction.myFaction.myBuildings.addJobToConstructionQueue (myFaction.myFaction.spawnLocation, toBuild);
		}
	}

	public override void onCantPerformAction()
	{

	}
}
