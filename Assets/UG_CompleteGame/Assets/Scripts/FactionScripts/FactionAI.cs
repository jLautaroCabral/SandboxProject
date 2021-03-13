using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//added methods to see if we have enough soldiers
//added methods to see if we are producing archers/hoplites
//added 2 lists to AI unit store, units guarding & units attacking
//added faction colours and code to apply this to units & buildings
//added condition to Action_Patrol so it can be used by the AI
//added part of attack bool to the unit master class
//switch if statement in threaded building location get
//added myThread.abort when threads are done
//changed back to using tiles as pathfinding nodes
//changed the Building Actions start Action method to use getComponentInParent
public class FactionAI : MonoBehaviour {

	public FactionIdentifier fi;
	public int numberOfWorkersFree,numberOfWorkersGettingResources,numberOfWorkersBuilding;
	public int gatheringFood,gatheringStone,gatheringWood,gatheringIron,gatheringGold;
	public bool canWeCreateWorkersDisp = false,canWeCreateSoldiersDisp=false,doWeHaveResourceGatheringDisp=false;
	int numberOfSoldiersForAttack = 5;

	Faction currentEnemy;
	public bool attackInProgress = false;
	public bool playerIsEnemy = false;
	public bool defeated = false;
	void Awake()
	{
		fi = this.gameObject.GetComponent<FactionIdentifier> ();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine ("workOutValues");
		StartCoroutine ("decisionMaker");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator workOutValues()
	{
		Debug.Log ("Calculating Values");
		numberOfWorkersFree = fi.myFaction.myUnits.numberOfWorkersFree ();
		numberOfWorkersGettingResources = fi.myFaction.myUnits.numberOfWorkersDoingAction ("ResourceGather");
		numberOfWorkersBuilding = fi.myFaction.myUnits.workOutWorkersBuilding ();
		gatheringFood = fi.myFaction.myUnits.workOutWorkersGatheringResource ("food");
		gatheringStone = fi.myFaction.myUnits.workOutWorkersGatheringResource ("stone");
		gatheringWood = fi.myFaction.myUnits.workOutWorkersGatheringResource ("wood");
		gatheringIron = fi.myFaction.myUnits.workOutWorkersGatheringResource ("iron");
		gatheringGold = fi.myFaction.myUnits.workOutWorkersGatheringResource ("gold");
		Debug.Log ("FINISHED CALCULATING VALUES");
		yield return new WaitForSeconds(5.0f);
		StartCoroutine ("workOutValues");
	}


	IEnumerator decisionMaker()
	{
		Debug.Log ("Making Decision");
		if (canWeCreateWorkers () == false) {
			if (areWeTryingToBuildBuilding("Temple") == false) {
				fi.myFaction.myDecision.createUnit ("Worker");
			}
		} else if (canWeCreateSoldiers () == false) {
			if (areWeTryingToBuildBuilding("Barracks") == false && areWeTryingToCreateHoplite()==false) {
				fi.myFaction.myDecision.createUnit ("Hoplite");
			}
		} else {
			
			decideIfWeShouldCreateWorker ();
			makeSureThatWeAreGatheringAllResources ();
			decideIfWeShouldCreateSoldier ();
			assignSoldiers ();
			//shouldWeAttack ();
			//haveWeBeenDefeated ();
		}
		yield return new WaitForSeconds(10.0f);//REMEMBER TO CHANGE BACK TO 30 SECONDS
		StartCoroutine ("decisionMaker");
	}


	void decideIfWeShouldCreateWorker()
	{
		if (doWeHaveEnoughWorkers () == true) {//want to have a 50/50 ratio of army and workers
			if (isPopulationAtLimit ()) {//are we maxed out for population
				//build home
				Debug.Log ("We have enough workers and are at the population limit, building homes");
				fi.myFaction.myDecision.buildBuilding ("House");
			} else {
				//do nothing
			}
		} else {//not enough workers so we build some more
			if (isPopulationAtLimit ()) {//not enough popluation so we need to build a home first
				//build home
				Debug.Log ("We do not have enough workers and are at the population limit, building homes");

				fi.myFaction.myDecision.buildBuilding ("House");

			} else {
				//build worker
				Debug.Log ("We do not have enough workers and are at the population limit, building homes");
				fi.myFaction.myDecision.createUnit ("Worker");
			}
		}
	}

	void decideIfWeShouldCreateSoldier()
	{
		if (doWeHaveEnoughSoldiers () == true) {//want to have a 50/50 ratio of army and workers
			if (isPopulationAtLimit ()) {//are we maxed out for population
				//build home
				Debug.Log ("We have enough soldiers and are at the population limit, building homes");
				fi.myFaction.myDecision.buildBuilding ("House");
			} else {
				//do nothing
			}
		} else {//not enough workers so we build some more
			if (isPopulationAtLimit ()) {//not enough popluation so we need to build a home first
				//build home
				Debug.Log ("We do not have enough soldiers and are at the population limit, building homes");

				fi.myFaction.myDecision.buildBuilding ("House");

			} else {
				//build worker
				Debug.Log ("We do not have enough soldiers and are not at the population limit, creating soldier");
				createSoldier ();
			}
		}
	}

	void assignSoldiers()
	{//fi.myFaction.myUnits.soldiersOnAttack.Contains(g)==false && fi.myFaction.myUnits.soldiersGuardingHome.Contains(g)==false
		foreach (GameObject g in fi.myFaction.myUnits.hoplites) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (um.isReserved==false && um.partOfAttack==false) {
				if (fi.myFaction.myUnits.soldiersGuardingHome.Count <= fi.myFaction.myUnits.soldiersOnAttack.Count) {
					fi.myFaction.myUnits.soldiersGuardingHome.Add (g);

					int r = Random.Range (1, fi.myFaction.myBuildings.myBuildings.Count);
					Vector3 patrolStart = fi.myFaction.myBuildings.myBuildings [r - 1].GetComponent<Building> ().getGoToTile ().posInWorld;
					Vector3 patrolEnd = fi.myFaction.myBuildings.myBuildings [r].GetComponent<Building> ().getGoToTile ().posInWorld;
					Action_Patrol ap = g.AddComponent<Action_Patrol> ();
					ap.enabled = false;
					ap.startingPosition = patrolStart;
					ap.endposition = patrolEnd;
					ap.positionWeAreMovingTo = patrolStart;
					ap.setPatLoc = true;
					um.actionsQueue.Add (ap);
					um.isReserved=true;
				} else {
					fi.myFaction.myUnits.soldiersOnAttack.Add (g);
					um.partOfAttack = true;
				}
			}
		}

		foreach (GameObject g in fi.myFaction.myUnits.archers) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (um.isReserved==false && um.partOfAttack==false) {
				if (fi.myFaction.myUnits.soldiersGuardingHome.Count <= fi.myFaction.myUnits.soldiersOnAttack.Count) {
					fi.myFaction.myUnits.soldiersGuardingHome.Add (g);

					int r = Random.Range (1, fi.myFaction.myBuildings.myBuildings.Count);
					Vector3 patrolStart = fi.myFaction.myBuildings.myBuildings [r - 1].GetComponent<Building> ().getGoToTile ().transform.position;
					Vector3 patrolEnd = fi.myFaction.myBuildings.myBuildings [r].GetComponent<Building> ().getGoToTile ().transform.position;
					Action_Patrol ap = g.AddComponent<Action_Patrol> ();
					ap.enabled = false;
					ap.startingPosition = patrolStart;
					ap.endposition = patrolEnd;
					ap.positionWeAreMovingTo = patrolStart;
					ap.setPatLoc = true;
					um.actionsQueue.Add (ap);
					um.isReserved = true;
				} else {
					fi.myFaction.myUnits.soldiersOnAttack.Add (g);
					um.partOfAttack = true;

				}
			}
		}
	}

	void shouldWeAttack()
	{
		if (currentEnemy == null) {
			currentEnemy = decideFactionToAttack ();
		}

		if (attackInProgress == false) {
			if (fi.myFaction.myUnits.soldiersOnAttack.Count >= numberOfSoldiersForAttack) {
				int r = Random.Range (0, currentEnemy.myUnits.workers.Count);
				foreach (GameObject g in fi.myFaction.myUnits.soldiersOnAttack) {
					UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
					Action_AttackUnit atu = g.AddComponent<Action_AttackUnit> ();
					atu.initaliseTarget (currentEnemy.myUnits.workers [r]);
					atu.enabled = false;
					um.actionsQueue.Add (atu);
				}
				attackInProgress = true;
			}
		} else {
			if (hasAttackFailed () == true) {
				fi.myFaction.myUnits.soldiersOnAttack.Clear ();
				numberOfSoldiersForAttack *= 2;
				attackInProgress = false;
			}

			if (hasAttackSucceded () == true) {
				currentEnemy = decideFactionToAttack ();
				fi.myFaction.myUnits.soldiersOnAttack.Clear ();
				attackInProgress = false;
			}

			foreach (GameObject g in fi.myFaction.myUnits.soldiersOnAttack) {
				UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
				if (um.actionsQueue.Count == 0) {
					int r = Random.Range (0, currentEnemy.myUnits.workers.Count);
					Action_AttackUnit atu = g.AddComponent<Action_AttackUnit> ();
					atu.initaliseTarget (currentEnemy.myUnits.workers [r]);
					atu.enabled = false;
					um.actionsQueue.Add (atu);
				}
			}

		}
	}

	Faction decideFactionToAttack()//need some criteria to work out whether to attack player or not aswell
	{
		List<Faction> potentialFactions = new List<Faction>();

		foreach (Faction f in fi.myFaction.factionsNotMe) {
			if (f.myAI.defeated == false) {
				potentialFactions.Add (f);
			}
		}
		if (potentialFactions.Count == 0) {
			this.enabled = false;
			Debug.Log ("GAME OVER");
			return null;//AI has won
		}
		int r = Random.Range (0, potentialFactions.Count);
		return potentialFactions [r];
	}

	bool hasAttackFailed()
	{
		foreach (GameObject g in fi.myFaction.myUnits.soldiersOnAttack) {
			if (g.GetComponent<UnitHealth> ().dead == false) {
				return false;
			}
		}

		return true;
	}

	void haveWeBeenDefeated()
	{
		if (fi.myFaction.myUnits.workers.Count == 0) {
			StopCoroutine("workOutValues");
			StopCoroutine("decisionMaker");
			defeated = true;
			this.enabled = false;
			//have something to kill off remaining units?
		}
	}

	bool hasAttackSucceded()
	{
		if (playerIsEnemy == false) {
			if (currentEnemy != null) {
				if (currentEnemy.myUnits.workers.Count == 0) {
					return true;
				} else {
					return false;
				}
			}
		}
		return false;
	}

	void createSoldier()
	{
		int r = Random.Range (0, 100);

		if (r < 50) {
			if (areWeTryingToCreateArcher () == false) {
				fi.myFaction.myDecision.createUnit ("Archer");
			}
		} else {
			if (areWeTryingToCreateWorker () == false) {
				fi.myFaction.myDecision.createUnit ("Hoplite");
			}
		}
	}



	bool areWeTryingToBuildBuilding(string building)
	{
		return fi.myFaction.myDecision.areWeAlreadyTryingToDoAnAction (building);
	}

	bool doWeHaveBuilding (string result){
		GameObject g = fi.myFaction.myBuildings.getNearestBuildingOfType (result, Vector3.zero);

		if (g == null) {
			return false;
		} else {
			Building b = g.GetComponent<Building> ();

			if (b.built == false) {
				return false;
			} else {
				return true;
			}
		}
	}


	bool canWeCreateWorkers()
	{
		if (doWeHaveBuilding("Temple")==false) {
			canWeCreateWorkersDisp = false;
			return false;
		} else {
			canWeCreateWorkersDisp = true;
			return true;
		}
	}

	bool canWeCreateSoldiers()
	{
		if (doWeHaveBuilding("Barracks")==false) {
			canWeCreateSoldiersDisp = false;
			return false;
		} else {
			canWeCreateSoldiersDisp = true;
			return true;
		}
	}

	bool doWeHaveEnoughWorkers()
	{
		if (fi.myFaction.myUnits.workers.Count >= (fi.myFaction.myPopulation.populationLimit/2)) {
			//we have enough workers, build homes
			return true;
		} else {
			return false;
		}
	}

	bool doWeHaveEnoughSoldiers()
	{
		if ((fi.myFaction.myUnits.hoplites.Count + fi.myFaction.myUnits.archers.Count) >= (fi.myFaction.myPopulation.populationLimit / 2)) {
			return false;
		} else {
			return false;
		}
	}

	bool isPopulationAtLimit()
	{
		fi.myFaction.myPopulation.getPotentialPopulation ();
		if (fi.myFaction.myPopulation.currentPoplualtion >= fi.myFaction.myPopulation.potentialPopulation && fi.myFaction.myPopulation.currentPoplualtion >= fi.myFaction.myPopulation.populationLimit) {
			return true;
		} else {
			return false;
		}

		//return !fi.myFaction.myPopulation.getPotentialPopulation ();//want to invert cause this returns whether the potential is above the actual and we want the other way around
	}

	bool areWeTryingToCreateWorker()
	{
		if (fi.myFaction.myDecision.areWeAlreadyTryingToDoAnAction ("Worker")) {
			return true;
		} else {
			return false;
		}
	}

	bool areWeTryingToCreateHoplite()
	{
		if (fi.myFaction.myDecision.areWeAlreadyTryingToDoAnAction ("Hoplite")) {
			return true;
		} else {
			return false;
		}
	}

	bool areWeTryingToCreateArcher()
	{
		if (fi.myFaction.myDecision.areWeAlreadyTryingToDoAnAction ("Archer")) {
			return true;
		} else {
			return false;
		}
	}

	bool doWeHaveEnoughFreeWorkers(){
		if (numberOfWorkersFree >= (fi.myFaction.myPopulation.populationLimit / 12)) {
			return true;
		} else {
			return false;
		}
	}

	void makeSureThatWeAreGatheringAllResources()
	{

		string resourceWithLowest = "";
		int numberOfLowest = 99999;

		if (gatheringFood < numberOfLowest) {
			resourceWithLowest = "food";
			numberOfLowest = gatheringFood;
		}

		if (gatheringWood < numberOfLowest) {
			resourceWithLowest = "wood";
			numberOfLowest = gatheringWood;
		}


		if (gatheringStone< numberOfLowest) {
			resourceWithLowest = "stone";
			numberOfLowest = gatheringStone;
		}

		if (gatheringGold< numberOfLowest) {
			resourceWithLowest = "gold";
			numberOfLowest = gatheringGold;
		}

		if (gatheringIron < numberOfLowest) {
			resourceWithLowest = "iron";
			numberOfLowest = gatheringIron;
		}

		fi.myFaction.myDecision.gatherResources (resourceWithLowest);


	}

	public string getResourceWithMost()
	{
		string resourceWithMost = "";
		int numberOfHighest = 99999;

		if (gatheringFood > numberOfHighest) {
			resourceWithMost = "food";
			numberOfHighest = gatheringFood;
		}

		if (gatheringWood > numberOfHighest) {
			resourceWithMost = "wood";
			numberOfHighest = gatheringWood;
		}


		if (gatheringStone> numberOfHighest) {
			resourceWithMost = "stone";
			numberOfHighest = gatheringStone;
		}

		if (gatheringWood > numberOfHighest) {
			resourceWithMost = "gold";
			numberOfHighest = gatheringWood;
		}

		if (gatheringIron > numberOfHighest) {
			resourceWithMost = "iron";
			numberOfHighest = gatheringIron;
		}
		return resourceWithMost;

	}


}
