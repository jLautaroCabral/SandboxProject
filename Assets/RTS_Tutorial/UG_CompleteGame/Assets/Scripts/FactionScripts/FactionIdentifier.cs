using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionIdentifier : MonoBehaviour {
	public Faction myFaction;

	public bool isItPartOfMyFaction(Faction toCompare)
	{
		if (toCompare == null) {//pass in null if its the player
			return false;
		}

		if (myFaction == toCompare) {
			return true;
		} else {
			return false;
		}
	}



}
