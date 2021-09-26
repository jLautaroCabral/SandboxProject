using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created this class, will basicly act as a place for the AI to find a resource when it needs them
//food will be added at some point in the future, need to create farm building first
//also made changes to forest and rockey tile to add them 

//Faction Identifier
//Basicly a marker for which object belongs to which faction, used to determine whether the player can interact with it
//contains a reference to the Faction (going to be used as a quick reference for when units are created)

//Faction Controller
//Stores a reference to all the factions that are in the game + initalises them

//Faction
//stores a reference to the factions units, buildings and ID
//Creates the initial workers for the start of the game
//has a method to find a suitable location for them to be spawned

//AI Unit/Building store 
//self explanatory

//Faction Resources
//Self Explanatory

//Action_AttackUnit
//added check to isItAValidTarget to make sure its not attacking a unit of the same faction
//added condition to check whether the unit was null

//GUI Manager 
//added check that a unit was of a diferent faction for the icon change



//TODO

//add the rest of the AI



public class ResourceStore : MonoBehaviour {
	public static ResourceStore me;
	public List<GameObject> food,wood,stone,iron,gold;
	public float acceptableDistance=10.0f;
	void Awake()
	{
		me = this;
		food = new List<GameObject> ();
		wood = new List<GameObject> ();
		stone = new List<GameObject> ();
		iron = new List<GameObject> ();
		gold = new List<GameObject> ();
	}
		

	public GameObject getNearestResource(Vector3 pos,string resource){
		//resource = resource.ToLower ();
		switch (resource) {
		case "food":
			return getNearestFood (pos);
			break;
		case "wood":
			return getNearestWood (pos);
			break;
		case "stone":
			return getNearestStone (pos);
			break;
		case "iron":
			return getNearestIron (pos);
			break;
		case "gold":
			return getNearestGold (pos);
			break;
		default:
			Debug.LogError ("DEFAULTING ON " + resource + " " + pos.ToString ());
			return null;
			break;
		}
	}

	public GameObject getNearestFood(Vector3 myPosition){
		float nearestDist = 999999.0f;
		GameObject nearest = null;

		foreach (GameObject g in food) {

			if (g == null) {
				continue;
			}

			float dist = Vector3.Distance (g.transform.position, myPosition);
			if (dist < nearestDist && isNodeWalkable(g.transform.position)) {
				nearest = g;
				nearestDist = dist;
				if (dist <= acceptableDistance) {
					return nearest;
				}

			}


		}

		return nearest;
	}

	public GameObject getNearestWood(Vector3 myPosition){
		float nearestDist = 999999.0f;
		GameObject nearest = null;

		foreach (GameObject g in wood) {

			if (g == null) {
				continue;
			}

			float dist = Vector3.Distance (g.transform.position, myPosition);
			if ( dist < nearestDist && isNodeWalkable(g.transform.position)) {
				nearest = g;
				nearestDist = dist;

				if (dist <= acceptableDistance) {
					return nearest;
				}

			}


		}

		return nearest;
	}

	public GameObject getNearestStone(Vector3 myPosition){
		float nearestDist = 999999.0f;
		GameObject nearest = null;

		foreach (GameObject g in stone) {

			if (g == null) {
				continue;
			}

			float dist = Vector3.Distance (g.transform.position, myPosition);
			if ( dist < nearestDist && isNodeWalkable(g.transform.position)) {
				nearest = g;
				nearestDist = dist;

				if (dist <= acceptableDistance) {
					return nearest;
				}
			}


		}

		return nearest;
	}

	public GameObject getNearestGold(Vector3 myPosition){
		float nearestDist = 999999.0f;
		GameObject nearest = null;

		foreach (GameObject g in gold) {
			if (g == null) {
				continue;
			}

				float dist = Vector3.Distance (g.transform.position, myPosition);
				if ( dist < nearestDist && isNodeWalkable(g.transform.position)) {
					nearest = g;
					nearestDist = dist;
					if (dist <= acceptableDistance) {
						return nearest;
					}

				}

		
		}

		return nearest;
	}

	public GameObject getNearestIron(Vector3 myPosition){
		float nearestDist = 999999.0f;
		GameObject nearest = null;

		foreach (GameObject g in iron) {

			if (g == null) {
				continue;
			}
			float dist = Vector3.Distance (g.transform.position, myPosition);
			if ( dist < nearestDist && isNodeWalkable(g.transform.position)) {
				nearest = g;
				nearestDist = dist;

				if (dist <= acceptableDistance) {
					return nearest;
				}
			}


		}

		return nearest;
	}


	bool isNodeWalkable(Vector3 positionOfObject)
	{
		int x = (int)positionOfObject.x;
		int y = (int)positionOfObject.y;

		TileMasterClass tm =GridGenerator.me.getTile (x, y);

		if (tm.isTileWalkable () == true && tm.myNode.isNodeWalkable () == true) {
			return true;
		} else {
			return false;
		}
	}

}
