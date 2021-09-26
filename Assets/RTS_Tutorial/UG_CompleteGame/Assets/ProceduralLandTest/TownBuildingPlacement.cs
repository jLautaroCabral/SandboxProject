using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBuildingPlacement : MonoBehaviour {

	public List<GameObject> myBuildings;

	//variables for the threaded building location find
	ThreadedLocationFindTest tblf;
	public List<TileMasterClass> tilesFromThread;
	bool waitingForLocation =false;
	List<PotentialBuildingLocation> potentialLocationList;

	//variables for queue of construction jobs
	public List<ConstructionJobStore> myJobs;
	void Awake()
	{
		myBuildings = new List<GameObject> ();
		myJobs = new List<ConstructionJobStore> ();


	}

	void Start()
	{

	}

	void Update()
	{
		threadMoniter ();
		moniterConstructionJobQueue ();
	}

	void threadMoniter()
	{
		if (waitingForLocation == true) {
			if (tblf != null) {
				Debug.Log ("Waiting for building location...");
				if (tblf.Update ()) {
					tblf = null;
					if (tilesFromThread.Count == 0) {
						Debug.Log ("Couldn't find area to place building");
						waitingForLocation = false;
						createNextBuilding ();
					} else {
						waitingForLocation = false;

						foreach (TileMasterClass tm in tilesFromThread) {
							tm.setTileWalkable (false);
						}
						Debug.Log ("Found area for building");
						createNextBuilding ();
					}
				} else {
				}
			} else {
			}
			return;
		}
	}

	//this is a temporary
	public void createBuilding(int width,int height,Vector3 posInWorld)
	{
		//if (tblf == null) {
			threadedLocationGet (posInWorld, width, height);
		//}
	}

	void threadedLocationGet(Vector3 pos,int width,int height)
	{
		//tilesFromThread = placeBuilding ((int)pos.x,(int)pos.y, width, height);
		tblf = new ThreadedLocationFindTest ();
		tblf.initialise (this, pos, width, height);
		tblf.Start ();
		waitingForLocation = true;

		//Debug.Log ("Created thread");
	}


	void addLocationIfValid(int x,int y,int width,int height){
		TileMasterClass[,] tiles = GridGenerator.me.getTiles ();

		List<TileMasterClass> tilesInArea = new List<TileMasterClass> ();

		//goes through the grid with the x/y passed in being the starting point getting a set of tiles the width & height 
		//specified with a 2 tile border to ensure that it has a walkable area around it
		for (int x1 = x-2; x1 < x+width + 2; x1++) {
			for (int y1 = y-2; y1 < y+height + 2; y1++) {
				tilesInArea.Add (tiles [x1, y1]);
			}
		}

		//if the location is valid then it will get the area for the size of the building without the 2 tile border and store it as a potential building location
		if (isLocationValidForBuilding (tilesInArea) == true) {
			List<TileMasterClass> tilesForBuilding = new List<TileMasterClass> ();
			for (int x1 = x; x1 < x + width; x1++) {
				for (int y1 = y; y1 < y + height; y1++) {
					tilesForBuilding.Add (tiles [x1, y1]);
				}
			}

			PotentialBuildingLocation pbt = new PotentialBuildingLocation ();
			pbt.tilesInList = tilesForBuilding;
			pbt.distanceFromStart = Vector3.Distance (myJobs[0].locationInWorld, pbt.tilesInList [0].getPosInWorld ());
			potentialLocationList.Add (pbt);
		}
	}

	public List<TileMasterClass> placeBuilding(int x, int y,int width,int height)
	{
		potentialLocationList = new List<PotentialBuildingLocation> ();

		//goes through the grid and checks if the tiles at that x/y would be a valid location for the building
		//has an offset for the start and end to ensure that there are no null referenece exceptions
		// take 2 because we're getting a 2 tile border for each building to make sure there is a walkable path around it
		for (int x1 = 2; x1 < ((int)GridGenerator.me.gridDimensions.x-2) -width; x1++) {
			for (int y1 = 2; y1 < ((int)GridGenerator.me.gridDimensions.y - 2) - height; y1++) {
				addLocationIfValid (x1, y1, width, height);
			}
		}

		//gets the closest potential location and returns it, if none have been found then it will return an empty list
		if (potentialLocationList.Count > 0) {
			float closest = potentialLocationList [0].distanceFromStart;
			PotentialBuildingLocation nearest = potentialLocationList [0];
			foreach (PotentialBuildingLocation pb in potentialLocationList) {

				if (pb.distanceFromStart < closest) {
					closest = pb.distanceFromStart;
					nearest = pb;
				}
			}


			return nearest.tilesInList;
		} else {
			return new List<TileMasterClass> ();
		}
	}

	bool isLocationValidForBuilding(List<TileMasterClass> tiles)
	{

		foreach (TileMasterClass tm in tiles) {
			if (tm.myNode.isNodeWalkable() == false || tm.isTileWalkable()==false || tm.hasResource==true) {
				return false;
			}
		}
		return true;
	}

	void moniterConstructionJobQueue()
	{
		if (myJobs.Count > 0) {
			if (tblf == null) { //not started job
				ConstructionJobStore curJob = myJobs [0];
				createBuilding (curJob.width, curJob.height, curJob.locationInWorld);

			} 
		}
	}

	void createNextBuilding()
	{
		if (tilesFromThread.Count > 0) { //if we can't find a place for it then we don't create a building
			GameObject g = createBuilding ();
			Vector3 posToPlace = tilesFromThread [0].transform.position + ((tilesFromThread [tilesFromThread.Count - 1].transform.position - tilesFromThread [0].transform.position) / 2);

			myBuildings.Add (g);
			/////
			/////
			/////
			//int halfThroughIndex = tilesFromThread.Count / 2;
			g.transform.position = posToPlace;

			//creates a new AI build for the created building


			//tells the appropriate workers to go and build the building

			//myBuildings.Add (g);
		}
		myJobs.RemoveAt(0); //remove the current job as its done
	}

	//creates a building based off the current job in the queue
	GameObject createBuilding()
	{

		GameObject g = Instantiate (myJobs [0].toBuild.gameObject, new Vector3(0,0,0),Quaternion.Euler(0,0,0));
		g.SetActive (true);
		SpriteRenderer sr = g.AddComponent<SpriteRenderer> ();
		sr.sortingOrder = 10;
		Building b = g.GetComponent<Building> ();
		sr.sprite = b.buildingSprite;
		b.built = true;
		b.health = 100;
		Color cl = new Color (1, 1, 1, 1.0f);
		sr.color = cl;
		g.AddComponent<BoxCollider2D> ();
		return g;
	}

	//will change to use buildings when AI for deciding buildings is implemented
	public void addJobToConstructionQueue(Vector3 posInWorld,Building toBuild)
	{

		myJobs.Add(new ConstructionJobStore(posInWorld,toBuild,new List<GameObject>()));
	}

	public bool doWeHaveBuildingOfType(string type)
	{
		foreach (GameObject g in myBuildings) {
			Building b = g.GetComponent<Building> ();
			if (b.name.Equals (type)) {
				return true;
			}
		}

		return false;
	}


	public GameObject getNearestBuildingOfType(string type, Vector3 position)
	{
		GameObject building = null;
		float distance = 99999.0f;
		foreach (GameObject g in myBuildings) {
			Building b = g.GetComponent<Building> ();
			if (b.name.Equals (type)) {
				float distBetween = Vector3.Distance (g.transform.position, position);
				if (distBetween < distance) {
					distance = distBetween;
					building = g;
				}
			}
		}
		return building;
	}

}
