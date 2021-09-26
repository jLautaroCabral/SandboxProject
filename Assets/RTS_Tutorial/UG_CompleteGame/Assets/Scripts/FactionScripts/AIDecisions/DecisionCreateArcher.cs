using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionCreateArcher : Decision {


	BuildingAction_CreateArcher baca;
	public void initialise(FactionIdentifier fi)
	{
		myFaction = fi;
	}

	public override bool canWePerformAction()
	{
		if (baca == null) {
			baca = FindObjectOfType<BuildingAction_CreateArcher> ();
		}

		if (myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Barracks",myFaction.myFaction.spawnLocation) == null || myFaction.myFaction.myPopulation.getPotentialPopulation() == false || myFaction.myFaction.myResources.canWeDoBuildingAction (baca) == false) {
			return false;
		} else {
			return true;
		}
	}

	public override void doAction()
	{
		if (canWePerformAction () == true) {
			Building barrakcs = myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Barracks",myFaction.myFaction.spawnLocation).GetComponent<Building>();
			if (barrakcs.buildingActionQueue.Count < 1) {
				BuildingAction_CreateArcher bw = (BuildingAction_CreateArcher)Instantiate (baca, barrakcs.gameObject.transform);
				barrakcs.buildingActionQueue.Add (bw);
			}
			myFaction.myFaction.myDecision.setDecisionInProgress ();
		} else {
			onCantPerformAction ();
		}
	}

	public override void onCantPerformAction()
	{


		if (myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Barracks",myFaction.myFaction.spawnLocation) == null && myFaction.myFaction.myDecision.areWeAlreadyTryingToDoAnAction("Barracks")==false) {
			myFaction.myFaction.myDecision.buildBuilding ("Barracks");
		}

		if (myFaction.myFaction.myPopulation.getPotentialPopulation() == false ) {//has to be first to stop the Ai building loads of houses while the temple is being built
			myFaction.myFaction.myDecision.buildBuilding ("House");
		}

		if (myFaction.myFaction.myResources.canWeDoBuildingAction (baca) == false) {
			myFaction.myFaction.myDecision.gatherResourcesForBuildingAction (baca);

		}

		myFaction.myFaction.myDecision.pauseDecision (this);

	}

	public override string  endResult()
	{
		return "Archer";
	}
}