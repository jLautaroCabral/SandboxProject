using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {
	public FactionIdentifier myFaction;
	public AIUnitStore myUnits;
	public AIBuildingStore myBuildings;
	public FactionResources myResources;
	public AIDecisionMaker myDecision;
	public FactionPopulationController myPopulation;
	public FactionAI myAI;
	public Vector3 spawnLocation;
	public List<Faction> factionsNotMe;
	public Color myColor;
	public void initialise()
	{
		this.gameObject.AddComponent<FactionIdentifier> ();
		this.gameObject.AddComponent<AIUnitStore> ();
		this.gameObject.AddComponent<AIBuildingStore> ();
		this.gameObject.AddComponent<FactionResources> ();
		this.gameObject.AddComponent<AIDecisionMaker> ();
		this.gameObject.AddComponent<FactionPopulationController> ();
		this.gameObject.AddComponent<FactionAI> ();
		myFaction = this.gameObject.GetComponent<FactionIdentifier> ();
		myFaction.myFaction = this; 



		myUnits = this.gameObject.GetComponent<AIUnitStore> ();
		myBuildings = this.gameObject.GetComponent<AIBuildingStore> ();
		myResources = this.gameObject.GetComponent<FactionResources> ();
		myDecision = this.GetComponent<AIDecisionMaker> ();
		myPopulation = this.GetComponent<FactionPopulationController> ();
		myAI = this.GetComponent<FactionAI> ();
		myColor = createColor ();
		createStartingWorkers ();

	}

	//needs to be called after all ingame factions have been created
	public void setFactionsNotMe()
	{
		factionsNotMe = new List<Faction> ();

		foreach (Faction f in FactionController.me.factionsInGame) {
			if (f != this) {
				factionsNotMe.Add (f);
			}
		}
	}

	public void createStartingWorkers()
	{
		Vector3 spawnPos = getWalkablePosition ();
		spawnLocation = spawnPos;

		for (int i = 0; i < 5; i++) {
			Vector3 pos = new Vector3 (spawnPos.x + Random.Range (-1.0f, 1.0f),spawnPos.y + Random.Range (-1.0f, 1.0f), -2.0f);
			GameObject g = (GameObject)Instantiate (FactionController.me.worker, pos, Quaternion.Euler (0, 0, 0));
			g.GetComponent<SpriteRenderer> ().color = myColor;
			myUnits.workers.Add (g);
			g.AddComponent<FactionIdentifier>();
			FactionIdentifier f = g.GetComponent<FactionIdentifier> ();
			f.myFaction = this;
		}
	}

	Vector3 getWalkablePosition()
	{
		int x = Random.Range (10,(int)GridGenerator.me.gridDimensions.y-10);
		int y = Random.Range (10,(int)GridGenerator.me.gridDimensions.y-10);

		TileMasterClass tm = GridGenerator.me.getTile (x, y);

		if (tm.isTileWalkable () == true) {
			return tm.transform.position;
		} else {
			return getWalkablePosition ();
		}
	}

	Color createColor()
	{
		float r = Random.Range (0.3f, 0.7f);
		float b = Random.Range (0.3f, 0.7f);
		float g = Random.Range (0.3f, 0.7f);
		return new Color (r, g, b, 1.0f);
	}



}
