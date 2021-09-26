using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionController : MonoBehaviour {
	public static FactionController me;
	public List<Faction> factionsInGame;
	public int numberOfFactionsToCreate;
	public GameObject worker; //so that the factions have a reference to the worker to spawn

	bool initialised = false;
	void Awake()
	{
		me = this;
		factionsInGame = new List<Faction> ();
	
	}

	void Start()
	{
		
	}

	void Update()
	{
		if (initialised == false) {
			if (GridGenerator.me.getTiles () != null) {
				initialiseFactions ();
				initialised = true;
			}
		}
	}

	void initialiseFactions()
	{
		for (int x = 0; x < numberOfFactionsToCreate; x++) {
			GameObject g = Instantiate (new GameObject (), this.transform);
			g.name = "Faction Controller " + factionsInGame.Count;
			Faction f = g.AddComponent<Faction> ();
			f.initialise ();
			factionsInGame.Add (f);
		}

		foreach (Faction f in factionsInGame) {
			f.setFactionsNotMe ();
		}
	}
}
