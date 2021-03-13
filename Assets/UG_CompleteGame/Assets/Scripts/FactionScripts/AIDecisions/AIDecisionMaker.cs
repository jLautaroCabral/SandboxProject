using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIDecisionMaker : MonoBehaviour {
	Faction myFaction;

	public List<Decision> decisionQueue = new List<Decision>();
	public List<Decision> inProgressDecisions = new List<Decision>();
	public List<Decision> decisionsOnHold = new List<Decision>();

	void Awake()
	{
		myFaction = this.gameObject.GetComponent<Faction> ();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine ("queueMoniter");
	}
	
	// Update is called once per frame
	void Update () {
		
		/*if (Input.GetKeyDown (KeyCode.I)) {
			createUnit ("Worker");
		}

		if (Input.GetKeyDown (KeyCode.H)) {
			createUnit ("Hoplite");
		}

		if (Input.GetKeyDown (KeyCode.Y)) {
			createUnit ("Archer");
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			gatherResources ("food");
		}*/
	}



	IEnumerator queueMoniter() {
		//Debug.Log ("Running queue moniter coroutine");

		if (decisionQueue.Count > 0) {
			decisionQueue [0].doAction ();

		}

		if (decisionQueue.Count == 0 && decisionsOnHold.Count > 0) {
			unpauseDecisions ();
		} else if (decisionQueue.Count == 0 && decisionsOnHold.Count == 0 && inProgressDecisions.Count > 0) {
			inProgressDecisions.Clear ();//will need to do some cleanup to delete the objects from the scene
		}
		yield return new WaitForSeconds(1.0f);
		StartCoroutine ("queueMoniter");
	}

	/*void queueMoniter(){
		if (decisionQueue.Count > 0) {
			decisionQueue [0].doAction ();
		}

		if (decisionQueue.Count == 0 && decisionsOnHold.Count > 0) {
			unpauseDecisions ();
		} else if (decisionQueue.Count == 0 && decisionsOnHold.Count == 0 && inProgressDecisions.Count > 0) {
			inProgressDecisions.Clear ();//will need to do some cleanup to delete the objects from the scene
		}
	}*/



	public void buildBuilding(string building)
	{
		Decision_BuildBuilding di = this.gameObject.AddComponent<Decision_BuildBuilding> ();
		di.initialise (myFaction.myFaction, building);
		di.setFaction (myFaction.myFaction);
		decisionQueue.Add (di);
	}

	public void gatherResources(string resource)
	{
		if (areWeAlreadyTryingToDoAnAction (resource) == false) {
			Decision_GatherResources gr = this.gameObject.AddComponent<Decision_GatherResources> ();
			gr.initialise (myFaction.myFaction, resource);
			decisionQueue.Add (gr);
		}
	}

	public bool areWeAlreadyTryingToDoAnAction(string result) //may have to create seperate versions for checking if we are trying to perform an action to fix the multiple farms/not finding food bug???
	{
		foreach (Decision d in inProgressDecisions) {
			if(d.endResult().Equals(result))
			{
				return true;
			}
		}

		foreach (Decision d in decisionQueue) {
			if(d.endResult().Equals(result))
			{
				return true;
			}
		}

		/*foreach (Decision d in decisionsOnHold) {
			if(d.endResult().Equals(result))
			{
				return true;
			}
		}*/

		return false;
	}

	public bool areWeTryingToBuildFarm(string result)
	{
		foreach (Decision d in inProgressDecisions) {
			if(d.endResult().Equals(result))
			{
				return true;
			}
		}

		foreach (Decision d in decisionQueue) {
			if(d.endResult().Equals(result))
			{
				return true;
			}
		}

		foreach (Decision d in decisionsOnHold) {
			if(d.endResult().Equals(result))
			{
				return true;
			}
		}

		return false;
	}

	public void createUnit(string unit)
	{
		if (unit.Equals ("Worker")) {
			Decision d = this.gameObject.AddComponent<DecisionCreateWorker> ();
			d.setFaction (myFaction.myFaction);
			decisionQueue.Add (d);
		} else if (unit.Equals ("Hoplite")) {
			Decision d = this.gameObject.AddComponent<DecisionCreateHoplite> ();
			d.setFaction (myFaction.myFaction);
			decisionQueue.Add (d);
		}else if (unit.Equals ("Archer")) {
			Decision d = this.gameObject.AddComponent<DecisionCreateArcher> ();
			d.setFaction (myFaction.myFaction);
			decisionQueue.Add (d);
		}
	}


	public void gatherResourcesForBuilding(Building b)
	{
		if (b.woodCost > 0) {
			gatherResources ("wood");
		}

		if (b.stoneCost > 0) {
			gatherResources ("stone");
		}

		if (b.ironCost > 0) {
			gatherResources ("iron");
		}

		if (b.foodCost > 0) {

			gatherResources ("food");
		}

		if (b.goldCost > 0) {
			gatherResources ("gold");
		}
	}

	public void gatherResourcesForBuildingAction(BuildingAction b)
	{
		if (b.woodCost > 0) {
			gatherResources ("wood");
		}

		if (b.stoneCost > 0) {
			gatherResources ("stone");
		}

		if (b.ironCost > 0) {
			gatherResources ("iron");
		}

		if (b.foodCost > 0) {

			gatherResources ("food");
		}

		if (b.goldCost > 0) {
			gatherResources ("gold");
		}
	}

	public void setDecisionInProgress()
	{
		inProgressDecisions.Add(decisionQueue[0]);
		decisionQueue.RemoveAt (0);
		unpauseDecisions ();
	}

	public void pauseDecision(Decision decisionToPause)
	{
		//for(int x = 0;x<decisionQueue.Count;x++) {
			Decision d = decisionQueue[0];
			if (decisionToPause == d) {
				decisionsOnHold.Add (decisionQueue [0]);
				decisionQueue.RemoveAt (0);
				return;
			}
		//}
		Debug.LogError ("The decision was not found in the list of active decisions");
	}

	void unpauseDecisions()
	{
		if (decisionsOnHold.Count > 0) {
			foreach (Decision d in decisionsOnHold) {
				decisionQueue.Add (d);
			}
			decisionsOnHold.Clear ();
		}
	}
}
