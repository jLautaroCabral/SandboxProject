using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decision_BuildBuilding : Decision {


	public string buildingType="";
	public void initialise(FactionIdentifier fi,string buildingType)
	{
		myFaction = fi;
		this.buildingType = buildingType;
	}

	public override bool canWePerformAction()
	{
		if (myFaction.myFaction.myResources.canWeConstructBuilding (BuildingStore.me.getBuilding (buildingType)) == false) {
			return false;
		} else {
			return true;
		}
	}

	public override void doAction()
	{
		if (canWePerformAction () == true) {
			Building toBuild = BuildingStore.me.getBuilding (buildingType);
			myFaction.myFaction.myBuildings.addJobToConstructionQueue (myFaction.myFaction.spawnLocation, toBuild);
			myFaction.myFaction.myDecision.setDecisionInProgress ();
		} else {
			onCantPerformAction ();
		}
	}

	public override void onCantPerformAction()
	{
		myFaction.myFaction.myDecision.gatherResourcesForBuilding (BuildingStore.me.getBuilding (buildingType));
		myFaction.myFaction.myDecision.pauseDecision (this);
	}

	public override string  endResult()
	{
		return buildingType;
	}
}
