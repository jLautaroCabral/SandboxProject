using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMasterClass : MonoBehaviour {
	
	public List<UGAction> actionsQueue;



	public UnitHealth myHealth;
	public UnitAttack myAttack;
	public string unitType;

	public string[] myActions = { "Delete", "Patrol", "Scout" };

	public List<UGAction> interruptedQueue;
	bool interrupted = false;
	public bool isReserved = false;
	public bool partOfAttack = false;
	public void Awake()
	{
		actionsQueue = new List<UGAction> ();
		interruptedQueue = new List<UGAction> ();
		myHealth = this.GetComponent<UnitHealth> ();
		myAttack = this.GetComponent <UnitAttack> ();
	}
	
	// Update is called once per frame
	void Update () {
		queueMoniter ();
	}

	public void removeCurrentAction()
	{
		UGAction a = actionsQueue [0];
		actionsQueue.Remove (a);
		Destroy (a);
	}
		

	protected void queueMoniter()
	{
		if (actionsQueue.Count > 0) {
			if (actionsQueue [0].actionStarted == false) {
				actionsQueue [0].enabled = true;
				actionsQueue [0].doAction ();
				actionsQueue [0].actionStarted = true;
			} else {
				if (actionsQueue [0].multiPartAction == true) {
					actionsQueue [0].doAction ();
				}

				if (actionsQueue [0].getIsActionComplete () == true) {
					removeCurrentAction ();

					if (interrupted == true) {
						foreach (UGAction ac in interruptedQueue) {
							actionsQueue.Add (ac);
							//ac.reinitialiseAction ();
						}
						interruptedQueue.Clear ();
						interrupted = false;
					}

				}
			}


		} else {
			if (isReserved == true) {
				isReserved = false;
			}
		}
	}

	public virtual bool canWePerformAction(UGAction ac)
	{
		if (ac.getActionType ().Equals ("Movement")) {
			return true;
		} else if (ac.getActionType ().Equals ("Default")) {
			return false;
		}
		else if(ac.getActionType().Equals("Patrol")){
			return true;
		}
		else {
			
			return false;
		}
	}


	public virtual void destroyMe()
	{
		myHealth.die ();
		SelectionManager.me.setSelected(new List<GameObject>(), true);
	}
		

	public virtual void patrol()
	{
		UGAction p = this.gameObject.AddComponent<Action_Patrol> ();
		if (canWePerformAction (p)==true) {
			while (actionsQueue.Count > 0) {
				removeCurrentAction ();
			}
			actionsQueue.Add (p);
		} else {
			Destroy (p);
		}

		//add an action that lets a unit patrol between two points
	}

	public virtual void scout()
	{
		UGAction p = this.gameObject.AddComponent<Action_Scout> ();
		if (canWePerformAction (p)==true) {
			while (actionsQueue.Count > 0) {
				removeCurrentAction ();
			}
			actionsQueue.Add (p);
		} else {
			Destroy (p);
		}
		//add an action that lets a unit go to random points on the map
	}
		

	public virtual void interruptQueue(UGAction interruptWith)
	{
		Debug.Log ("Interrupting action");
		foreach (UGAction ac in actionsQueue) {
			ac.actionStarted = false;
			interruptedQueue.Add (ac);
		}

		actionsQueue.Clear ();
		actionsQueue.Add (interruptWith);
		interrupted = true;
	}

	public string[] getAvailableActions()//we return as a string cause we're probbably not going to change the types of actions we can do at runtime
	{
		return myActions;//array of actions a unit is allowed to perform
	}

}
