using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TileChangeManager : MonoBehaviour {
	public static TileChangeManager me;
	public List<GameObject> tilesWeCanUse;
	public GameObject selectedTile;
	// Use this for initialization
	void Awake()
	{
		me = this;
		checkTilesValid ();
	}

	void checkTilesValid()
	{
		foreach (GameObject g in tilesWeCanUse) {
			if (g.GetComponent<TileMasterClass> () == null) {
				tilesWeCanUse.Remove (g);
				checkTilesValid ();
				break;
			}
		}

	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject getTilesByType(string type)
	{
		foreach (GameObject g in tilesWeCanUse) {
			TileMasterClass tm = g.GetComponent<TileMasterClass> ();
			if (tm.type == type) {
				return g;
			}
		}
		return tilesWeCanUse[0];//just return the first tile as a default, should be the dirt tile
	}

	public GameObject getSelectedTile()
	{
		return selectedTile;
	}

	void OnGUI()
	{
		if (SelectionManager.me.selectionMode == selectingModes.tiles) {
			int yMod = 0;
			foreach (GameObject b in tilesWeCanUse) {

				try {
					Building buildingScr = b.GetComponent<Building> ();
					Rect pos = new Rect (50, 50 + (50 * yMod), 100, 50);
					if (GUI.Button (pos, b.name)) {
						selectedTile = b;
					}
					yMod+=1;
				} catch {
					Debug.Log ("Tile missing a component");
				}
			}
		}
	}
}
