using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created this script
//made build building and gather resources public in AIDecisionMaker
//added resource maniptulation methods to AI_gather resources
//created AI_GatherResources
//added checks to see if an decision had already been made to get something

//ALTERATIONS
//made it so any resources spawned are a child of the tile they are on
//TODO fix selection of AI units
public class Decision_GatherResources : Decision {

	string resourceToGather ="";
	public void initialise(FactionIdentifier fi,string resourceType)
	{
		myFaction = fi;
		resourceToGather = resourceType;
	}

	public override bool canWePerformAction()
	{
		if (resourceToGather == "food") { //food is a special case cause it needs a farm aswell
			if (ResourceStore.me.getNearestResource (myFaction.myFaction.spawnLocation,resourceToGather) == null) {
				Debug.Log ("CANT FIND FOOD NEAR US");
				return false;
			} else {
				if (myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Storehouse", myFaction.myFaction.spawnLocation) == null ) {
					Debug.Log ("DONT HAVE A STOREHOUSE");
					return false;
				}else if(myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Farm", myFaction.myFaction.spawnLocation) == null) 
				{
					Debug.Log ("DONT HAVE A FARM");
					return false;
				}
				else {
					return true;
				}
			}
		} else {
			if (ResourceStore.me.getNearestResource (myFaction.myFaction.spawnLocation,resourceToGather) == null) {
				return false;
			} else {
				if (myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Storehouse", myFaction.myFaction.spawnLocation) == null) {
					return false;
				} else {
					return true;
				}
			}
		}
	}

	public override void doAction()
	{
		if (canWePerformAction () == true) {
			List<GameObject> workers = myFaction.myFaction.myUnits.getFreeWorkers ();

			//if (workers.Count == 0) {
			//	List<GameObject> temp=myFaction.myFaction.myUnits.getWorkersThatAreLowPriority ();
			//	workers.Add (temp [0]);
			//}

			Debug.Log ("Found " + workers.Count + " Workers to gather resources");

			foreach (GameObject w in workers) {
				UnitMasterClass um = w.GetComponent<UnitMasterClass> ();
				AI_GatherResources aigr = w.gameObject.AddComponent<AI_GatherResources> ();
				aigr.initaliseLocation (this.transform.position, resourceToGather);
				um.actionsQueue.Add (aigr);
				//um.interruptQueue(aigr);
			}
			myFaction.myFaction.myDecision.setDecisionInProgress ();
		} else {
			onCantPerformAction ();
		}
	}

	public override void onCantPerformAction()
	{
		if (myFaction.myFaction.myDecision.areWeAlreadyTryingToDoAnAction ("Storehouse") == false && myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Storehouse", myFaction.myFaction.spawnLocation) == null ) {
			myFaction.myFaction.myDecision.buildBuilding ("Storehouse");
		}


		if (resourceToGather == "food" && myFaction.myFaction.myBuildings.getNearestBuildingOfType ("Farm", myFaction.myFaction.spawnLocation) != null) {
			Debug.Log ("WE CAN GATHER FOOD");

		}
		else{
			if (myFaction.myFaction.myDecision.areWeTryingToBuildFarm ("Farm") == true )
			{
				Debug.Log ("CURRENTLY BUILDING FARM");
			} else {
				myFaction.myFaction.myDecision.buildBuilding ("Farm");

			}
		} 

		myFaction.myFaction.myDecision.pauseDecision (this);

		//if (myFaction.myFaction.myDecision.areWeAlreadyTryingToDoAnAction (resourceToGather) == false) {
		//	myFaction.myFaction.myDecision.gatherResources (resourceToGather);
		//}
	}

	public override string  endResult()
	{
		return resourceToGather;
	}
}
