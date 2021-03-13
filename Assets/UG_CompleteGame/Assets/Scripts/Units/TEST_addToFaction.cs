using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_addToFaction : MonoBehaviour {

	public int factionIndex = 0;
	void Update()
	{
		try{
			if (this.GetComponent<Hoplite> () == true) {
				FactionController.me.factionsInGame [factionIndex].myUnits.hoplites.Add (this.gameObject);
				FactionIdentifier fi = this.gameObject.GetComponent<FactionIdentifier> ();
				fi.myFaction = FactionController.me.factionsInGame [factionIndex];
				this.GetComponent<UnitAttack>().setFactionIdentifier(fi);
				Destroy (this);
			} else if (this.GetComponent<Archer> () == true) {
				FactionController.me.factionsInGame [factionIndex].myUnits.archers.Add (this.gameObject);
				FactionIdentifier fi = this.gameObject.GetComponent<FactionIdentifier> ();
				fi.myFaction = FactionController.me.factionsInGame [factionIndex];
				this.GetComponent<UnitAttack>().setFactionIdentifier(fi);

				Destroy (this);
			} else if (this.GetComponent<Worker> () == true) {
				FactionController.me.factionsInGame [factionIndex].myUnits.workers.Add (this.gameObject);
				FactionIdentifier fi = this.gameObject.GetComponent<FactionIdentifier> ();
				fi.myFaction = FactionController.me.factionsInGame [factionIndex];
				this.GetComponent<UnitAttack>().setFactionIdentifier(fi);
				Destroy (this);
			}
		}
		catch{

		}
	}
}
