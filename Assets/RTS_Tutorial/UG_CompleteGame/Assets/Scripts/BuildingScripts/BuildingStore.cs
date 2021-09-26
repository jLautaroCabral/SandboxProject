using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BuildingStore : MonoBehaviour {
	public static BuildingStore me; //script stores all possible building and draws gui for placing them when you want to build one
	public GameObject[] buildings;
	[SerializeField]
	GameObject selectedBuilding;

	public List<GameObject> tempWorkerStore;
	public List<Building> buildingsInScene;
	public Building curBuildScr;
	void Awake()
	{
		me = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject getToBuild()
	{
		return selectedBuilding;
	}

	void OnGUI()
	{
		/*if (SelectionManager.me.selectionMode == selectingModes.creatingBuildings) {
			int yMod = 0;
			foreach (GameObject b in buildings) {
				
				try {
					Building buildingScr = b.GetComponent<Building> ();
					Rect pos = new Rect (50, 50 + (50 * yMod), 100, 50);
					if (GUI.Button (pos,buildingScr.name)) {
						selectedBuilding = b;
					}
					yMod+=1;
				} catch {
					Debug.Log ("Building missing a component");
				}
			}
		}*/
	}



	public void setSelectedBuilding(GameObject g)
	{
		selectedBuilding = g;
		curBuildScr = selectedBuilding.GetComponent<Building> ();
	}

	public Building getBuildingScr()
	{
		return curBuildScr;
	}

	public Building getNearestBuildingOfType(string type,Vector3 myPos)
	{
		Building b = null;
		List<Building> buildingsOfType = new List<Building> ();
		foreach (Building bl in buildingsInScene) {
			if (bl.name == type) {
				buildingsOfType.Add (bl);
			}
		}
		float curDistance = 99999.0f;
		Building retVal=null;
		Vector2 myPosV2 = new Vector2 (myPos.x, myPos.y);
		foreach (Building bl in buildingsOfType) {
			Vector2 buildPos = new Vector2 (bl.gameObject.transform.position.x, bl.gameObject.transform.position.y);

			if (Vector2.Distance (buildPos, myPosV2) < curDistance) {
				curDistance = Vector2.Distance (buildPos, myPosV2);
				retVal = bl;
			}
		}

		if (retVal != null) {
			return retVal;
		} else {
			return null;
		}
	}

	public void storeWorkers()
	{
		tempWorkerStore = SelectionManager.me.getSelected ();
	}

	public void returnWorkers()
	{
		SelectionManager.me.setSelected (tempWorkerStore, true);
		SelectionManager.me.selectionMode = selectingModes.units;
	}

	public Building getBuilding(string nameOfBuilding)
	{
		foreach (GameObject g in buildings) {
			Building bl = g.GetComponent<Building> ();

			if (bl.name.Equals (nameOfBuilding)) {
				return bl;
			}
		}
		return null;
	}
}
