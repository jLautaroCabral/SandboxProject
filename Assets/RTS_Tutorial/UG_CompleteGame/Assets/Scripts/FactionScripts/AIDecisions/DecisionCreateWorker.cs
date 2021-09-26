using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionCreateWorker : Decision {


	BuildingAction_CreateWorker bacw;
	public void initialise(FactionIdentifier fi)
	{
		myFaction = fi;
	}

	public override bool canWePerformAction()
	{
		if (bacw == null) {
			bacw = FindObjectOfType<BuildingAction_CreateWorker> ();
		}

		if (myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Temple",myFaction.myFaction.spawnLocation) == null || myFaction.myFaction.myPopulation.getPotentialPopulation() == false || myFaction.myFaction.myResources.canWeDoBuildingAction (bacw) == false) {
			return false;
		} else {
			return true;
		}
	}

	public override void doAction()
	{
		if (canWePerformAction () == true) {
			Building temple = myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Temple",myFaction.myFaction.spawnLocation).GetComponent<Building>();
			if (temple.buildingActionQueue.Count < 1) {
				BuildingAction_CreateWorker bw = (BuildingAction_CreateWorker)Instantiate (bacw, temple.gameObject.transform);
				temple.buildingActionQueue.Add (bw);
			}
			myFaction.myFaction.myDecision.setDecisionInProgress ();
		} else {
			onCantPerformAction ();
		}
	}

	public override void onCantPerformAction()
	{
		//Debug.Log (myFaction.myFaction.myPopulation.getPotentialPopulation());


		if (myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Temple",myFaction.myFaction.spawnLocation) == null && myFaction.myFaction.myDecision.areWeAlreadyTryingToDoAnAction("Temple")==false) {
			myFaction.myFaction.myDecision.buildBuilding ("Temple");
		}

		if (myFaction.myFaction.myPopulation.getPotentialPopulation() == false ) {//has to be first to stop the Ai building loads of houses while the temple is being built
			myFaction.myFaction.myDecision.buildBuilding ("House");
		}

		if (myFaction.myFaction.myResources.canWeDoBuildingAction (bacw) == false) {
			myFaction.myFaction.myDecision.gatherResourcesForBuildingAction (bacw);

		}

		myFaction.myFaction.myDecision.pauseDecision (this);

	}

	public override string  endResult()
	{
		return "Worker";
	}
}
