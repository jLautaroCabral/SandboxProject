using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//added code to stop the SelectionManager Null reference error //
//added reinitialise method to actions
//added check to selection manager to make sure units clicked by the player don't have a faction identifier on them //
//added ranged attack stuff + had to add action condition to archer script for can we perform action//
//added check for faction identifier when killing unit, will need to add condition for removing AI units

//added interrupted queue to unit master class//
//added logic for finding enemies to attack
//added arrow class//
//changed the unit order script to use this script when units are commanded to attack something so it doesn't go for another one on the way//
//made change to die method to call dieWhilstAttacking when it dies
//added null check to make sure that units arent going after dead units in the Action_Attack unit script



//TODO
//Add way of removing dead units from the respective factions unit list
//Add way to clear dead units out of a units 'unitsIAmBeingAttackedBy' list
public class UnitAttack : MonoBehaviour {
	public float detectRange;
	public float attackRange;
	public float attackDamage;
	public float attackRate;
	public attackType myAttack;

	GameObject myTarget;
	UnitHealth targetHealth;
	bool goingToAttackTarget=false;
	public List<GameObject> unitsIAmBeingAttackedBy;
	public FactionIdentifier myFactionIdentifier;
	public bool hasFactionIdentifier = true;

	bool startedCoroutine=false;
	bool canStart = false;

	public bool rangedAttack = false;
	public GameObject arrow;
	void Awake()
	{
		unitsIAmBeingAttackedBy = new List<GameObject> ();
	}

	void Start()
	{
		DEBUG_checkForFactionIdentifier ();
		if (hasFactionIdentifier == false) {
			StartCoroutine ("findTargetToAttack");
			startedCoroutine = true;
		}
	}


	public void attack(GameObject target)
	{
		if (rangedAttack == false) {
			target.GetComponent<UnitHealth> ().dealDamage (attackDamage);
		} else {
			Vector3 dir = target.transform.position - this.transform.position;
			dir = dir.normalized;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			GameObject arrowInst = (GameObject)Instantiate (arrow, this.transform.position,Quaternion.Euler(0,0,angle));
			if (myFactionIdentifier != null) {
				Arrow ar = arrowInst.GetComponent<Arrow> ();
				ar.initialise (myFactionIdentifier);
			}
		}
	}

	void Update()
	{

		if (goingToAttackTarget == true) {
			onTargetDead ();
		}

		if (startedCoroutine == false && hasFactionIdentifier==true) {
			startedCoroutine = true;
			StartCoroutine ("findTargetToAttack");
		}

		//if (startedCoroutine == true) {
			//coroutineReplacement ();
		//}
		
	}

	public void setFactionIdentifier(FactionIdentifier fi){
		myFactionIdentifier = fi;
		myFactionIdentifier.myFaction = fi.myFaction;
		hasFactionIdentifier = true;

	}

	void DEBUG_checkForFactionIdentifier() //needed for debug, when factions produce units the faction identifier will be got automaticly, since these are just demo units it needs to be set here
	{
		if (hasFactionIdentifier == true) {
			if (this.GetComponent<FactionIdentifier> () == true) {
				myFactionIdentifier = this.GetComponent<FactionIdentifier> ();
			} else {
				hasFactionIdentifier = false;
			}
		}
	}

	void onTargetDead()
	{
		if (isCurrentTargetDead ()) {
			//StopCoroutine ("findTargetToAttack");
			myTarget = null;
			targetHealth = null;
			goingToAttackTarget = false;
		}
	}

	bool isCurrentTargetDead()
	{
		return targetHealth.dead;
	}

	public void DEBUG_setTarget (GameObject g){
		myTarget = g;
		targetHealth = myTarget.GetComponent<UnitHealth> ();
		//Color cl = getRandomColor ();
		//g.GetComponent<SpriteRenderer> ().color = cl;
		//this.GetComponent<SpriteRenderer> ().color = cl;
		goingToAttackTarget = true;
		setAttackAction (g);
	}

	void setAttackAction(GameObject g)
	{
		//need this bit here after demo cause we won't need to debug it
		//myTarget = g;
		//targetHealth = myTarget.GetComponent<UnitHealth> ();
		//goingToAttackTarget = true;


		UnitMasterClass um = this.GetComponent<UnitMasterClass> ();
		UGAction a = this.gameObject.AddComponent<Action_AttackUnit> ();

		if (um.canWePerformAction (a) == true && g != null) { //check for if the hit object is null just to be safe
			Debug.Log ("Attacking Unit");
			g.GetComponent<UnitAttack> ().unitsIAmBeingAttackedBy.Add (this.gameObject);
			a.initaliseTarget (g);
			um.interruptQueue (a);
			a.enabled = false;
		} else {
			Debug.Log ("Attacking Went Wrong");
			myTarget = null;
			targetHealth = null;
			goingToAttackTarget = false;
			Destroy (a);
		}
	}

	public void dieWhilstAttacking()
	{
		if (goingToAttackTarget == true) {
			myTarget.GetComponent<UnitAttack> ().unitsIAmBeingAttackedBy.Remove (this.gameObject);
		}
	}

	Color getRandomColor()
	{
		float r = Random.Range (0.0f, 1.0f);
		float g = Random.Range (0.0f, 1.0f);
		float b  = Random.Range (0.0f, 1.0f);
		return new Color (r, g, b, 1.0f);
	}


	IEnumerator findTargetToAttack()
	{
		//Debug.Log ("Executed target find coroutine");
		if (goingToAttackTarget == false) {//don't already have a target
			if (hasFactionIdentifier == false) {//player unit
				searchForUnitsAttackingMe();
				if (goingToAttackTarget == false) {
					playerSearchForUnit ();
				}
			} else { //AI unit
				searchForUnitsAttackingMe();
				if (goingToAttackTarget == false) {
					AISearchForUnit ();
				}
			}
		}
		yield return new WaitForSeconds (1.0f);
		StartCoroutine ("findTargetToAttack");
	}

	float searchForEnemyTimer = 2.0f;
	//just here to test if it was the coroutine causing the crashes
	void coroutineReplacement(){
		searchForEnemyTimer -= Time.deltaTime;

		if (searchForEnemyTimer <= 0) {
			if (goingToAttackTarget == false) {//don't already have a target
				if (hasFactionIdentifier == false) {//player unit
					searchForUnitsAttackingMe();
					if (goingToAttackTarget == false) {
						playerSearchForUnit ();
					}
				} else { //AI unit
					searchForUnitsAttackingMe();
					if (goingToAttackTarget == false) {
						AISearchForUnit ();
					}
				}
			}

			searchForEnemyTimer = 2.0f;
		}
	}


	void searchForUnitsAttackingMe()
	{
		foreach (GameObject g in unitsIAmBeingAttackedBy) {
			if (Vector3.Distance (this.transform.position, g.transform.position) < detectRange) {
				UnitHealth uh = g.GetComponent<UnitHealth> ();
				if (uh.dead == false) {
					DEBUG_setTarget (g);
					break;
				}
			}
		}
	}

	void AISearchForUnit()
	{
		bool breakFromSearch = false;

		if (myFactionIdentifier.myFaction == null) {

		} else {
			foreach (Faction f in myFactionIdentifier.myFaction.factionsNotMe) {
				if (goingToAttackTarget == false) {
					foreach (GameObject g in f.myUnits.hoplites) {
						if (Vector3.Distance (this.transform.position, g.transform.position) < detectRange) {
							UnitHealth uh = g.GetComponent<UnitHealth> ();
							if (uh.dead == false) {
								DEBUG_setTarget (g);
								breakFromSearch = true;
								break;
							}
						}
						if (breakFromSearch == true) {
							break;
						}
					}
				}
				if (breakFromSearch == true) {
					break;
				}

				if (goingToAttackTarget == false) {
					foreach (GameObject g in f.myUnits.archers) {
						if (Vector3.Distance (this.transform.position, g.transform.position) < detectRange) {
							UnitHealth uh = g.GetComponent<UnitHealth> ();
							if (uh.dead == false) {
								DEBUG_setTarget (g);
								breakFromSearch = true;
								break;
							}
						}
						if (breakFromSearch == true) {
							break;
						}
					}
				}
				if (breakFromSearch == true) {
					break;
				}

				if (goingToAttackTarget == false) {
					foreach (GameObject g in f.myUnits.workers) {
						if (Vector3.Distance (this.transform.position, g.transform.position) < detectRange) {
							UnitHealth uh = g.GetComponent<UnitHealth> ();
							if (uh.dead == false) {
								DEBUG_setTarget (g);
								breakFromSearch = true;
								break;
							}
						}
						if (breakFromSearch == true) {
							break;
						}
					}
				}
				if (breakFromSearch == true) {
					break;
				}

				if (goingToAttackTarget == false) {
					foreach (GameObject g in UnitManager.me.units) {
						if (Vector3.Distance (this.transform.position, g.transform.position) < detectRange) {
							UnitHealth uh = g.GetComponent<UnitHealth> ();
							if (uh.dead == false) {
								DEBUG_setTarget (g);
								breakFromSearch = true;
								break;
							}
						}
						if (breakFromSearch == true) {
							break;
						}
					}
				}
			}
		}//
	}

	void playerSearchForUnit()
	{
		bool breakFromSearch = false;


		foreach (Faction f in FactionController.me.factionsInGame) {
			if (goingToAttackTarget == false) {
				foreach (GameObject g in f.myUnits.hoplites) {
					if (Vector3.Distance (this.transform.position, g.transform.position) < detectRange) {
						UnitHealth uh = g.GetComponent<UnitHealth> ();
						if (uh.dead == false) {
							DEBUG_setTarget (g);
							breakFromSearch = true;
							break;
						}
					}
					if (breakFromSearch == true) {
						break;
					}
				}
			}
			if (breakFromSearch == true) {
				break;
			}

			if (goingToAttackTarget == false) {
				foreach (GameObject g in f.myUnits.archers) {
					if (Vector3.Distance (this.transform.position, g.transform.position) < detectRange) {
						UnitHealth uh = g.GetComponent<UnitHealth> ();
						if (uh.dead == false) {
							DEBUG_setTarget (g);
							breakFromSearch = true;
							break;
						}
					}
					if (breakFromSearch == true) {
						break;
					}
				}
			}
			if (breakFromSearch == true) {
				break;
			}

			if (goingToAttackTarget == false) {
				foreach (GameObject g in f.myUnits.workers) {
					if (Vector3.Distance (this.transform.position, g.transform.position) < detectRange) {
						UnitHealth uh = g.GetComponent<UnitHealth> ();
						if (uh.dead == false) {
							DEBUG_setTarget (g);
							breakFromSearch = true;
							break;
						}
					}
					if (breakFromSearch == true) {
						break;
					}
				}
			}
			if (breakFromSearch == true) {
				break;
			}

		}
	}
}
	

public enum attackType{
	melee,
	ranged
}
