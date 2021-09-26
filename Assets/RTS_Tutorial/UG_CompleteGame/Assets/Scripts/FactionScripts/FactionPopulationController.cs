using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created this script to look after the population of a faction
//added if statement in building store to add houses to the myHouses list, will need to add condition to remove them when destroying buildings is implemented
//added a similar function to the Action_BuildBuilding when the player builds a home to add it to the players population manager

//added checks to unit construction building actions for the population limit
//added building action resource check to the faction resources
//altered the BuildingAction_CreateWorker so it can be used by the AI
//added way to gather resources for an action if we can't build a building/unit (not implemented for buildings yet, only building action for building worker)
//added way for actions to be removed from the queue if there are non trying to be executed

//made changes to the decision build buildings so it will check for resources before building
//added check to ddecision maker gatherResource Method so that it will check for the resource already being gathered


//NEW SCRIPTS
//This one
//Player resource manager
//Decision Create Worker

//MODIFIED SCRIPTS
//FAction (Adds pop controller) ||
//Action_BuildBuilding (Adds houses to the players pop manager) ||
//AIBuildingStore (adds houses to respective AI's pop manager) ||
//BuildingAction_CreateWorker (Changed to work out if its called by the player or an AI then act apropriately) ||
//Faction Resources (Added check to see if there are enough resources to perform a building action/build a building) ||
//AIDecision Maker (CreateUnit,GatherResourcesForBuilding,GatherResourcesForBuildingAction, logic for clearing the list of in progress decisions) ||
//Decision Build Building (Implemented on can't perform action) \\
//Resource Store (added check for the node that the resource being got that it is walkable) ||
//Added hasResource to the tileMasterClass + setters in the resource tile scripts \\
//added condition to the building placement algorithm so that the AI doesn't place buildings on resources \\


//TODO
//expand the BuildingActions for archers & hoplites to let them be used by the AI and the player
public class FactionPopulationController : MonoBehaviour {
	FactionIdentifier myFaction;
	public List<GameObject> myHouses;
	public int populationLimit;
	public int currentPoplualtion;
	public int potentialPopulation;
	float timer = 1.0f;
	void Awake()
	{
		myFaction = this.GetComponent<FactionIdentifier> ();
		myHouses = new List<GameObject> ();
	}

	public void addHouse(GameObject house)
	{
		myHouses.Add (house);
		calculateMaxPopulation ();
	}

	public void removeHouse(GameObject house)
	{
		myHouses.Remove (house);
		calculateMaxPopulation ();
	}
		
	void calculateMaxPopulation()
	{
		int popMax = myHouses.Count*5;

		/*foreach (GameObject g in myHouses) { 
			Building b = g.GetComponent<Building> ();
			if (b.built == true) {
				popMax += 5;
			}
		}*/
		populationLimit = popMax;
	}

	void calculateCurrentPopulation()
	{
		int popCount = 0;
		popCount += myFaction.myFaction.myUnits.archers.Count;
		popCount += myFaction.myFaction.myUnits.workers.Count;
		popCount += myFaction.myFaction.myUnits.hoplites.Count;

		currentPoplualtion = popCount;
	}

	public bool canWeConstructUnit()
	{
		return populationLimit >= currentPoplualtion;
	}

	public bool getPotentialPopulation()
	{
		int pot = 0;

		foreach (Decision d in myFaction.myFaction.myDecision.decisionsOnHold) {
			if (d.endResult ()==("House")) {
				pot += 5;
			}
		}

		foreach (Decision d in myFaction.myFaction.myDecision.decisionQueue) {
			if (d.endResult ()== ("House")) {
				pot += 5;
			}
		}

		foreach (Decision d in myFaction.myFaction.myDecision.inProgressDecisions) {
			if (d.endResult ()== ("House")) {
				pot += 5;
			}
		}
		potentialPopulation = pot;
		pot += populationLimit;
		//Debug.Log ("Potential population = " + pot);
		if (pot > currentPoplualtion) {
			return true;
		} else {
			return false;
		}
	}

	void Update(){
		moniterPopulation ();
	}

	void moniterPopulation()
	{
		timer -= Time.deltaTime;
		if (timer <= 0) {
			calculateMaxPopulation ();
			calculateCurrentPopulation ();
			timer = 1.0f;
		}
	}
}
