using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Building : MonoBehaviour {
	
	public int widthInTiles,heightInTiles;
	public string name;
	public Sprite buildingSprite;


	public int woodCost,stoneCost,foodCost,ironCost,goldCost;
	public int health = 1,maxHealth=100;
	public bool built = false;

	TileMasterClass tileNearest;
	SpriteRenderer sr;

	public List<BuildingAction> buildingActionQueue;
	public List<BuildingAction> actionsWeCanPerform; 
	void Awake()
	{
		sr = this.GetComponent<SpriteRenderer> ();
	}

	void Update()
	{
		if (built == true) {
			actionsQueueMoniter ();
		}
	}


	public void setTileNearMe(TileMasterClass tm)//returns a tile that is walkable near the front of the building (negative on y axis)
	{
		int x = (int)tm.getGridCoords ().x;
		int y = (int)tm.getGridCoords ().y;

		int mod = (heightInTiles / 2) + 1;
		y -= mod;

		tileNearest = GridGenerator.me.getTile (x, y);
		Debug.Log ("TILE NEAREST : "+tileNearest.name);
	}

	public TileMasterClass getGoToTile()//returns the tile that is walkable and below the building in question
	{
		if (tileNearest == null) {
			setTileNearMe (GridGenerator.me.getTile ((int) this.transform.position.x, (int)this.transform.position.y));
			return tileNearest;
		} else {
			return tileNearest;
		}
	}

	public void increaseBuildingHealth()
	{
		if (sr == null) {
			sr = this.GetComponent<SpriteRenderer> ();
		}

		health += 1;
		if (built == false && health == maxHealth) {
			built = true;
		}

		if (built == false) {
			Color cl = Color.white;
			float f = (float)health / (float)maxHealth;
			cl.a = f;
			Color newSrCo = new Color (cl.r, cl.g, cl.b, f);
			sr.color = newSrCo;
		}
	}


	void actionsQueueMoniter()
	{
		if (buildingActionQueue.Count > 0) {
			if (buildingActionQueue [0].enabled == false) {
				buildingActionQueue [0].enabled = true;
			}

			if (buildingActionQueue [0].started == false) {
				buildingActionQueue [0].startAction ();
				buildingActionQueue [0].doAction ();
			} else {
				if (buildingActionQueue [0].callEachFrame == true) {
					buildingActionQueue [0].doAction ();
				}
			}
			if (buildingActionQueue [0].areWeDone () == true) {
				buildingActionQueue [0].onComplete ();
				BuildingAction ba = buildingActionQueue [0];
				buildingActionQueue.RemoveAt (0);
				Destroy (ba.gameObject);
			}

		}
	}
}
