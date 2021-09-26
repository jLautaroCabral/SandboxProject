using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitStore : MonoBehaviour {
	FactionIdentifier fi;
	public List<GameObject> workers,hoplites,archers;


	public List<GameObject> soldiersGuardingHome,soldiersOnAttack;
	void Awake()
	{
		workers = new List<GameObject> ();
		archers= new List<GameObject> ();
		hoplites= new List<GameObject> ();
		soldiersGuardingHome = new List<GameObject> ();
		soldiersOnAttack = new List<GameObject> ();
		fi = this.gameObject.GetComponent<FactionIdentifier> ();
	}

	public List<GameObject> getFreeWorkers()//TRY METHOD WHERE IT RETURNS ALL THE WORKERS IF THERE ISNT A TEMPLE BUILD AND A LIMITED SELECTION IF THERE IS
	{
		GameObject g2 = fi.myFaction.myBuildings.getNearestBuildingOfType ("Temple", Vector3.zero);
		List<GameObject> free = new List<GameObject> ();

		if (g2 == null) {
			return workers;
		}
		else{

			Building b = g2.GetComponent<Building> ();

			if (b.built == false) {
				return workers;
			}
			else{

				foreach (GameObject g in workers) {
					UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
					if (um.actionsQueue.Count == 0 && um.isReserved==false) {
						um.isReserved = true;
						free.Add (g);
					}
				}

				//return free;
			}
		}

		if (free.Count == 0) {
			List<GameObject> workersTemp = new List<GameObject> ();
			int highestNumberOfJobs = 0, lowestNumberOfJobs = 99999;

			foreach (GameObject g in workers) {
				UnitMasterClass um = g.GetComponent<UnitMasterClass> ();

				if (um.actionsQueue.Count > highestNumberOfJobs) {
					highestNumberOfJobs = um.actionsQueue.Count;
				}

				if (um.actionsQueue.Count < lowestNumberOfJobs) {
					lowestNumberOfJobs = um.actionsQueue.Count;
				}
			}

			foreach (GameObject g in workers) {
				UnitMasterClass um = g.GetComponent<UnitMasterClass> ();

				if (um.actionsQueue.Count <= lowestNumberOfJobs && workersTemp.Count < 3) {
					workersTemp.Add (g);
				}
			}
			return workersTemp;
		} else {
			return free;
		}
	}


	public List<GameObject> getFreeWorkers(int numNeeded)
	{
		List<GameObject> free = new List<GameObject> ();

		foreach (GameObject g in workers) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (free.Count < numNeeded) {
				if (um.actionsQueue.Count == 0&& um.isReserved==false) {
					um.isReserved = true;

					free.Add (g);
				}
			} else {
				return free;
			}
		}
		return free;
	}

	public List<GameObject> getFreeArchers()
	{
		List<GameObject> free = new List<GameObject> ();

		foreach (GameObject g in archers) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (um.actionsQueue.Count == 0 && um.isReserved==false) {
				um.isReserved = true;
				free.Add (g);
			}
		}

		return free;
	}




	public List<GameObject> getFreeArchers(int numNeeded)
	{
		List<GameObject> free = new List<GameObject> ();

		foreach (GameObject g in archers) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (free.Count < numNeeded) {
				if (um.actionsQueue.Count == 0&& um.isReserved==false) {
					um.isReserved = true;

					free.Add (g);
				}
			} else {
				return free;
			}
		}
		return free;
	}

	public List<GameObject> getFreeHoplites()
	{
		List<GameObject> free = new List<GameObject> ();

		foreach (GameObject g in hoplites) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (um.actionsQueue.Count == 0 && um.isReserved==false) {
				um.isReserved = true;
				free.Add (g);
			}
		}

		return free;
	}


	public List<GameObject> getFreeHoplites(int numNeeded)
	{
		List<GameObject> free = new List<GameObject> ();

		foreach (GameObject g in hoplites) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (free.Count < numNeeded) {
				if (um.actionsQueue.Count == 0&& um.isReserved==false) {
					um.isReserved = true;

					free.Add (g);
				}
			} else {
				return free;
			}
		}
		return free;
	}



	public int numberOfWorkersFree()
	{
		int count = 0;
		foreach (GameObject g in workers) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (um.actionsQueue.Count == 0 && um.isReserved==false) {
				count++;
			}
		}
		return count;
	}

	public int numberOfWorkersDoingAction(string actionToCheck)
	{
		int count = 0;
		foreach (GameObject g in workers) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (um.actionsQueue.Count > 0) {
				if (um.actionsQueue [0].getActionType ().Equals (actionToCheck)) {
					count++;
				}
			}
		}
		return count;
	}

	public int workOutWorkersBuilding()
	{
		int count = 0;
		foreach (GameObject g in workers) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (um.actionsQueue.Count > 0) {
				try{
					AI_BuildBuilding aib =(AI_BuildBuilding) um.actionsQueue [0];
					if (aib == null) {

					} else {
						count++;
					}
				}
				catch{

				}
			}
		}
		return count;
	}

	public int workOutWorkersGatheringResource(string resource)
	{
		int count = 0;
		foreach (GameObject g in workers) {
			UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
			if (um.actionsQueue.Count > 0) {
				
				try{
					AI_GatherResources aig = (AI_GatherResources) um.actionsQueue [0];
					if (aig == null) {
						
					} else {
						if (aig.resourceType.Equals (resource)) {
							count++;
						}
					}
				}
				catch{

				}
			}
		}
		return count;
	}
}
