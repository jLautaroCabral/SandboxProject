using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator : MonoBehaviour {
	public GameObject farm,house,temple,storehouse,barracks,wall,palisade;
	public GameObject pavedTile;
	TownBuildingPlacement myBuildingStore;
	townBase toBuild;
	public List<GameObject> buildingsForTown;
	public int townCenterX = 0,townCenterY = 0;

	public typeOfSettlement myType;
	public bool createMultipleTowns = false;
	public int numberOfTowns = 4;
	public List<Vector3> connectionPoints;
	void Awake()
	{
		myBuildingStore = this.GetComponent<TownBuildingPlacement> ();
		buildingsForTown = new List<GameObject> ();
		connectionPoints = new List<Vector3> ();
	}
	bool drawnRoadsFromBuildings=false;
	int townCounter = 0;
	void Start()
	{
	}

	void Update()
	{
		
		if (Input.GetKeyDown(KeyCode.I)) {
			createTown ();
		}


		if (toBuild != null && drawnRoadsFromBuildings == false) {
			if (myBuildingStore.myBuildings.Count == toBuild.numberOfBuildings) {
				drawRoadsFromBuildings ();
				drawnRoadsFromBuildings = true;
				roadDialation ();
				roadDialation ();
				drawWalls ();
				readyForNext = true;
				townCounter++;
			}
		}
	}
	bool readyForNext = true;



	void createTown()
	{
			decideType ();
			if (toBuild != null) {
				Debug.Log ("Creating Town");
				initialiseListOfBuildings ();
				getNewRandomCoords ();
				drawPlaza ();
				drawRoads ();
				createBuildings ();
				drawRoadsFromBuildings ();
				readyForNext = false;

			}
	}

	void getNewRandomCoords(){
		townCenterX = (int)GridGenerator.me.gridDimensions.x / 2;//(int)Random.Range (GridGenerator.me.gridDimensions.x / 4, GridGenerator.me.gridDimensions.x - (GridGenerator.me.gridDimensions.x/ 4));
		townCenterY = (int)GridGenerator.me.gridDimensions.y / 2;//(int)Random.Range (GridGenerator.me.gridDimensions.y / 4, GridGenerator.me.gridDimensions.y - (GridGenerator.me.gridDimensions.y / 4));

	}


	void decideType()
	{
		if (myType == typeOfSettlement.city) {
			toBuild = new city ();
		} else if (myType == typeOfSettlement.farm) {
			toBuild = new farm ();
		} else if (myType == typeOfSettlement.town) {
			toBuild = new town ();
		} else if (myType == typeOfSettlement.village) {
			toBuild = new village ();
		} else if (myType == typeOfSettlement.fort) {
			toBuild = new fort ();
		}
	}

	void drawPlaza()
	{
		for (int x = townCenterX - 3; x < townCenterX + 3; x++) {
			for (int y = townCenterY - 3; y < townCenterY + 3; y++) {
				TileMasterClass tm = GridGenerator.me.getTile (x, y);
				tm.hasResource = true;
				if (toBuild.drawRoads == false) {
					GridGenerator.me.ChangeTilesInGrid (tm.gameObject, GridGenerator.me.prefabTile);
				} else {
					GridGenerator.me.ChangeTilesInGrid (tm.gameObject, pavedTile);
				}
			}
		}
	}

	void drawRoadsFromBuildings()
	{
		foreach (GameObject g in myBuildingStore.myBuildings) {
			Debug.Log ("Drawing path from buildings");
			Building b = g.GetComponent<Building> ();
			//if (b.name != "House") {
				Vector3 start = b.getGoToTile ().transform.position;
				Vector3 end = new Vector3 (townCenterX, townCenterY);
				List<TileMasterClass> tilesInPath = Pathfind.me.getPathAsTiles (start, end);
				foreach (TileMasterClass tm in tilesInPath) {
					tm.hasResource = true;
					if (toBuild.drawRoads == false) {
						GridGenerator.me.ChangeTilesInGrid (tm.gameObject, GridGenerator.me.prefabTile);
					} else {
						GridGenerator.me.ChangeTilesInGrid (tm.gameObject, pavedTile);

					}
				}
			//}
		}
	}

	void drawWalls()
	{
		float lowestX=9999999.0f, lowestY=9999999.0f, highestX=0, highestY=0;


		foreach (GameObject g in myBuildingStore.myBuildings) {
			Building b = g.GetComponent<Building> ();
			Vector3 topRight = new Vector3 (g.transform.position.x + (b.widthInTiles / 2), g.transform.position.y + (b.heightInTiles / 2), 0);
			Vector3 bottomLeft = new Vector3 (g.transform.position.x - (b.widthInTiles / 2), g.transform.position.y - (b.heightInTiles / 2), 0);

			if (bottomLeft.x < lowestX) {
				lowestX = bottomLeft.x;
			}

			if (bottomLeft.y < lowestY) {
				lowestY = bottomLeft.y;
			}

			if (topRight.x > highestX) {
				highestX = topRight.x;
			}

			if (topRight.y > highestY) {
				highestY = topRight.y;
			}
		}

		Vector3 topLeft_fin = new Vector3 (lowestX-3, highestY+3, 0);
		Vector3 topRight_fin = new Vector3(highestX+3,highestY+3,0);
		Vector3 bottomLeft_fin = new Vector3 (lowestX-3, lowestY-3,0);
		Vector3 bottomRight_fin = new Vector3 (highestX+3, lowestY-3,0);
		//createWallSection (topLeft_fin);
		//createWallSection (topRight_fin);
		//createWallSection (bottomLeft_fin);
		//createWallSection (bottomRight_fin);

		List<TileMasterClass> tiles = Pathfind.me.getPathAsTiles (topLeft_fin, topRight_fin);
		foreach (TileMasterClass tm in tiles) {
			if (tm.hasResource == false) {
				createWallSection (tm);

			}
		}
		tiles.Clear ();


		tiles = Pathfind.me.getPathAsTiles (topLeft_fin, bottomLeft_fin);
		foreach (TileMasterClass tm in tiles) {
			if (tm.hasResource == false) {
				createWallSection (tm);

			}
		}
		tiles.Clear ();

		tiles = Pathfind.me.getPathAsTiles ( topRight_fin,bottomRight_fin);
		foreach (TileMasterClass tm in tiles) {
			if (tm.hasResource == false) {
				createWallSection (tm);

			}
		}
		tiles.Clear ();

		bottomLeft_fin = new Vector3 (lowestX-4, lowestY-4,0);
		bottomRight_fin = new Vector3 (highestX+4, lowestY-4,0);

		tiles = Pathfind.me.getPathAsTiles ( bottomLeft_fin,bottomRight_fin);
		foreach (TileMasterClass tm in tiles) {
			if (tm.hasResource == false) {
				createWallSection (tm);
			}
		}
		tiles.Clear ();
	}

	void createWallSection(TileMasterClass tm)
	{
		Vector3 pos = tm.posInWorld;
		if (toBuild.drawWalls == true) {
			GameObject g = (GameObject) Instantiate (wall, pos, Quaternion.Euler (0, 0, 0));
			g.GetComponent<SpriteRenderer> ().sortingOrder = (int)(pos.y*-1) + 200;
			tm.setTileWalkable (false);
		} else if (toBuild.drawPalisade == true) {
			GameObject g = (GameObject)Instantiate (palisade, pos, Quaternion.Euler (0, 0, 0));
			g.GetComponent<SpriteRenderer> ().sortingOrder = (int)(pos.y*-1) + 200;
			tm.setTileWalkable (false);

		} else {

		}
	}

	void drawRoads()
	{
		Vector3 roadEnd = new Vector3 (townCenterX + (toBuild.townWidth)+5, Random.Range (townCenterY - (toBuild.townHeight / 2), townCenterY + (toBuild.townHeight / 2)), 0);
		connectionPoints.Add (roadEnd);
		if (roadEnd.x < 0) {
			roadEnd.x = 0;
		}

		if (roadEnd.x > GridGenerator.me.gridDimensions.x - 1) {
			roadEnd.x = GridGenerator.me.gridDimensions.x - 1;
		}

		if (roadEnd.y < 0) {
			roadEnd.y = 0;
		}

		if (roadEnd.y > GridGenerator.me.gridDimensions.y - 1) {
			roadEnd.y = GridGenerator.me.gridDimensions.y - 1;
		}

		List<TileMasterClass> tilesInPath = Pathfind.me.getPathAsTiles (new Vector3 (townCenterX, townCenterX, 0), roadEnd);
		foreach (TileMasterClass tm in tilesInPath) {
			tm.hasResource = true;
			if (toBuild.drawRoads == false) {
				GridGenerator.me.ChangeTilesInGrid (tm.gameObject, GridGenerator.me.prefabTile);
			} else {
				GridGenerator.me.ChangeTilesInGrid (tm.gameObject, pavedTile);

			}
		}

		tilesInPath.Clear ();

		roadEnd = new Vector3 (townCenterX - (toBuild.townWidth)-5, Random.Range (townCenterY - (toBuild.townHeight / 2), townCenterY + (toBuild.townHeight / 2)), 0);
		connectionPoints.Add (roadEnd);
		if (roadEnd.x < 0) {
			roadEnd.x = 0;
		}

		if (roadEnd.x > GridGenerator.me.gridDimensions.x - 1) {
			roadEnd.x = GridGenerator.me.gridDimensions.x - 1;
		}

		if (roadEnd.y < 0) {
			roadEnd.y = 0;
		}

		if (roadEnd.y > GridGenerator.me.gridDimensions.y - 1) {
			roadEnd.y = GridGenerator.me.gridDimensions.y - 1;
		}
		tilesInPath = Pathfind.me.getPathAsTiles (new Vector3 (townCenterX, townCenterX, 0), roadEnd);
		foreach (TileMasterClass tm in tilesInPath) {
			tm.hasResource = true;
			if (toBuild.drawRoads == false) {
				GridGenerator.me.ChangeTilesInGrid (tm.gameObject, GridGenerator.me.prefabTile);
			} else {
				GridGenerator.me.ChangeTilesInGrid (tm.gameObject, pavedTile);

			}		
		}

		tilesInPath.Clear ();

		roadEnd = new Vector3 (Random.Range (townCenterX- (toBuild.townWidth / 2), townCenterX + (toBuild.townWidth / 2)), townCenterY +(toBuild.townHeight)+5, 0);
		connectionPoints.Add (roadEnd);
		if (roadEnd.x < 0) {
			roadEnd.x = 0;
		}

		if (roadEnd.x > GridGenerator.me.gridDimensions.x - 1) {
			roadEnd.x = GridGenerator.me.gridDimensions.x - 1;
		}

		if (roadEnd.y < 0) {
			roadEnd.y = 0;
		}

		if (roadEnd.y > GridGenerator.me.gridDimensions.y - 1) {
			roadEnd.y = GridGenerator.me.gridDimensions.y - 1;
		}
		tilesInPath = Pathfind.me.getPathAsTiles (new Vector3 (townCenterX, townCenterX, 0), roadEnd);
		foreach (TileMasterClass tm in tilesInPath) {
			tm.hasResource = true;
			if (toBuild.drawRoads == false) {
				GridGenerator.me.ChangeTilesInGrid (tm.gameObject, GridGenerator.me.prefabTile);
			} else {
				GridGenerator.me.ChangeTilesInGrid (tm.gameObject, pavedTile);

			}		
		}

		tilesInPath.Clear ();

		roadEnd = new Vector3 (Random.Range (townCenterX- (toBuild.townWidth / 2), townCenterX + (toBuild.townWidth / 2)), townCenterY -(toBuild.townHeight)-5, 0);
		connectionPoints.Add (roadEnd);
		if (roadEnd.x < 0) {
			roadEnd.x = 0;
		}

		if (roadEnd.x > GridGenerator.me.gridDimensions.x - 1) {
			roadEnd.x = GridGenerator.me.gridDimensions.x - 1;
		}

		if (roadEnd.y < 0) {
			roadEnd.y = 0;
		}

		if (roadEnd.y > GridGenerator.me.gridDimensions.y - 1) {
			roadEnd.y = GridGenerator.me.gridDimensions.y - 1;
		}
		tilesInPath = Pathfind.me.getPathAsTiles (new Vector3 (townCenterX, townCenterX, 0), roadEnd);
		foreach (TileMasterClass tm in tilesInPath) {
			tm.hasResource = true;
			if (toBuild.drawRoads == false) {
				GridGenerator.me.ChangeTilesInGrid (tm.gameObject, GridGenerator.me.prefabTile);
			} else {
				GridGenerator.me.ChangeTilesInGrid (tm.gameObject, pavedTile);

			}		
		}

		tilesInPath.Clear ();
	}

	void roadDialation()
	{
		for (int x = 0; x < (int)GridGenerator.me.gridDimensions.x; x++) {
			for (int y = 0; y < (int)GridGenerator.me.gridDimensions.y; y++) {
				TileMasterClass tm = GridGenerator.me.getTile (x, y);
				List<TileMasterClass> neighbours = GridGenerator.me.getTileNeighbors (tm);

				int numberOfRoadTiles = 0;
				if (tm.hasResource == true) {
					foreach (TileMasterClass n in neighbours) {
						if (n.hasResource == true) {
							numberOfRoadTiles++;
						}
					}
				}

				if (numberOfRoadTiles >= 3) {
					tm.hasResource = true;
					if (toBuild.drawRoads == false) {
						GridGenerator.me.ChangeTilesInGrid (tm.gameObject, GridGenerator.me.prefabTile);
					} else {
						GridGenerator.me.ChangeTilesInGrid (tm.gameObject, pavedTile);
					}
				}
			}
		}
	}


	void initialiseListOfBuildings()
	{
		if (toBuild.includeBarracks == true) {
				buildingsForTown.Add (barracks);
		}

		if (toBuild.includeFarms == true) {
			buildingsForTown.Add (farm);
		}

		if (toBuild.includeTemple == true) {
			buildingsForTown.Add (temple);
		}

		if (toBuild.includeHouses == true) {
			for (int x = 0; x < 10; x++) {
				buildingsForTown.Add (house);
			}
		}

		if (toBuild.includeStorehouse == true) {
			buildingsForTown.Add (storehouse);
		}
	}
		
	void createBuildings()
	{
		for (int x = 0; x < toBuild.numberOfBuildings; x++) {
			GameObject toCreate = buildingsForTown [Random.Range (0, buildingsForTown.Count)];
			if (toCreate.GetComponent<Building> ().name == "Farm") {
				int r = Random.Range (0, 100);
				if (r < 50) {
					myBuildingStore.addJobToConstructionQueue (new Vector3 (townCenterX+(int)(GridGenerator.me.gridDimensions.x/4), townCenterY, 0), toCreate.GetComponent<Building> ());
				} else {
					myBuildingStore.addJobToConstructionQueue (new Vector3 (townCenterX-(int)(GridGenerator.me.gridDimensions.x/4), townCenterY, 0), toCreate.GetComponent<Building> ());

				}

			} else {
				myBuildingStore.addJobToConstructionQueue (new Vector3 (townCenterX, townCenterY, 0), toCreate.GetComponent<Building> ());
			}
		}

		if (toBuild.includeFarms == true) {
			int r = Random.Range (0, 100);
			if (r < 50) {
				myBuildingStore.addJobToConstructionQueue (new Vector3 (townCenterX+(int)(GridGenerator.me.gridDimensions.x/3), townCenterY, 0), farm.GetComponent<Building> ());
			} else {
				myBuildingStore.addJobToConstructionQueue (new Vector3 (townCenterX-(int)(GridGenerator.me.gridDimensions.x/3), townCenterY, 0), farm.GetComponent<Building> ());

			}
		}
	}


}

public class townBase{
	public int numberOfBuildings,townWidth,townHeight;
	public bool includeFarms = false,includeHouses=false,includeStorehouse=false,includeTemple=false,includeBarracks=false,drawWalls=false,drawPalisade=false,drawRoads = false;
}

public enum typeOfSettlement{
	farm,
	village,
	town,
	city,
	fort
}

public class farm : townBase{
	public farm()
	{
		numberOfBuildings = 4;
		townWidth = 15;
		townHeight = 15;
		includeFarms = true;
		includeHouses = true;
		includeStorehouse = true;
	}
}

public class village : townBase{
	public village()
	{
		numberOfBuildings = 10;
		townWidth = 25;
		townHeight = 25;
		includeFarms = true;
		includeHouses = true;
		includeTemple = true;
		includeStorehouse = true;
	}
}

public class town : townBase{
	public town()
	{
		numberOfBuildings = 25;
		townWidth = 40;
		townHeight = 40;
		includeHouses = true;
		includeTemple = true;
		includeStorehouse = true;
		includeBarracks = true;
		drawRoads = true;
		drawPalisade = true;
	}
}

public class  city : townBase{
	public city()
	{
		numberOfBuildings =80;
		townWidth = 70;
		townHeight = 70;
		includeHouses = true;
		includeTemple = true;
		includeStorehouse = true;
		includeBarracks = true;
		drawRoads = true;
		drawWalls = true;
	}
}

public class fort : townBase{
	public fort()
	{
		numberOfBuildings = 4;
		townWidth = 10;
		townHeight = 10;
		includeBarracks = true;
		drawPalisade = true;
	}
}