using UnityEngine;
using System.Collections;

public class UnitHealth : MonoBehaviour {
	public float health = 5.0f;
	public bool dead = false;


	public void dealDamage(float val)
	{
		health -= val;
		deathCheck ();
	}

	void deathCheck()
	{
		if (health <= 0) {
			dead = true;
			die ();
		}
	}

	public void die(){
		this.GetComponent<UnitMasterClass> ().enabled = false;
		UnitAttack ua = this.GetComponent<UnitAttack> ();
		ua.dieWhilstAttacking ();
		ua.enabled = false;
		this.GetComponent<UnitMovement> ().enabled = false;
		this.GetComponent<BoxCollider2D> ().enabled = false;
		if (this.GetComponent<FactionIdentifier> () == false) {
			UnitManager.me.removeUnit (this.gameObject);
		} else {
			if (this.GetComponent<Hoplite> ()) {
				this.GetComponent<FactionIdentifier> ().myFaction.myUnits.hoplites.Remove (this.gameObject);
			} else if (this.GetComponent<Archer> ()) {
				this.GetComponent<FactionIdentifier> ().myFaction.myUnits.archers.Remove (this.gameObject);

			} else if (this.GetComponent<Worker> ()) {
				this.GetComponent<FactionIdentifier> ().myFaction.myUnits.workers.Remove (this.gameObject);

			}
		}
		this.transform.rotation = Quaternion.Euler (0, 0, 90);
	}
}
