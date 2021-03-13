using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ADDED CHECK FOR HAS RESOURCE WHEN SWITCHING TILES & THE BLANKGRID BOOL for creating a town, MENTION IN NEXT EPISODE

public class GridGenerator : MonoBehaviour {
	public static GridGenerator me;//Controls grid of tiles

	TileMasterClass[,] gridOfTiles;
	public GameObject prefabTile;

	public GameObject grass, water, mountain,forest,rockey,ocean;

	public Vector2 gridDimensions;
	public bool loadingGrid = false;

	public bool blankGrid = false;
	// Use this for initialization
	void Awake()
	{
		me = this;
		gridOfTiles = new TileMasterClass[(int)gridDimensions.x,(int)gridDimensions.y];


	}

	void Start () {
		if (loadingGrid == false) {
			generateGrid ();
			NodeOptimiser.me.optimiseNodes ();
		} else {
			SaveGrid.me.readGridFromFile ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setGrid(TileMasterClass[,] newGrid)
	{
		gridOfTiles = newGrid;
	}

	public TileMasterClass[,] getTiles()
	{
		return gridOfTiles;
	}

	public void ChangeTilesInGrid(List<GameObject> selectedTiles)
	{
		foreach (GameObject t in selectedTiles) {
			TileMasterClass tm = t.GetComponent<TileMasterClass> ();
			Vector2 gridCoords = tm.getGridCoords ();
			int x = (int)gridCoords.x;
			int y = (int)gridCoords.y;
			bool walkable = tm.isTileWalkable ();
			PathfindNode pn = tm.myNode; //NEW AI DECISIONS

			GameObject oldTile = gridOfTiles [x, y].gameObject;
			Destroy (oldTile);

			//issue is something to do with the list of selected tiles in the selection manager becoming null when the selected tiles are deleted;
			Vector3 posToCreateTile = new Vector3 (x, y, 0);
			GameObject mostRecentTile = (GameObject)Instantiate( TileChangeManager.me.getSelectedTile (),posToCreateTile,Quaternion.Euler(0,0,0));
			mostRecentTile.GetComponent<TileMasterClass> ().setGridCoords (new Vector2 (x, y));
			mostRecentTile.transform.parent = this.gameObject.transform;
			mostRecentTile.GetComponent<TileMasterClass> ().setTileWalkable (walkable);
			mostRecentTile.GetComponent<TileMasterClass> ().myNode = pn;//NEW AI DECISIONS
			mostRecentTile.GetComponent<TileMasterClass>().hasResource = tm.hasResource;
			gridOfTiles [x, y] = mostRecentTile.GetComponent<TileMasterClass> ();
			mostRecentTile.SetActive (true);
			mostRecentTile.name = "New tile" + x + " " + y;

		}
	}
	//new version to switch out 1 tile for another
	public void ChangeTilesInGrid(GameObject selectedTiles,GameObject newTile)
	{

		TileMasterClass tm = selectedTiles.GetComponent<TileMasterClass> ();
		Vector2 gridCoords = tm.getGridCoords ();
		int x = (int)gridCoords.x;
		int y = (int)gridCoords.y;
		bool walkable = tm.isTileWalkable ();
		PathfindNode pn = tm.myNode;

		GameObject oldTile = gridOfTiles [x, y].gameObject;
		Destroy (oldTile);


		//re write to use less .getComponents
		Vector3 posToCreateTile = new Vector3 (x, y, 0);
		GameObject mostRecentTile = (GameObject)Instantiate( newTile,posToCreateTile,Quaternion.Euler(0,0,0));
		mostRecentTile.GetComponent<TileMasterClass> ().setGridCoords (new Vector2 (x, y));
		mostRecentTile.transform.parent = this.gameObject.transform;
		mostRecentTile.GetComponent<TileMasterClass> ().setTileWalkable (walkable);
		mostRecentTile.GetComponent<TileMasterClass> ().myNode = pn;
		mostRecentTile.GetComponent<TileMasterClass>().hasResource = tm.hasResource;
		gridOfTiles [x, y] = mostRecentTile.GetComponent<TileMasterClass> ();
		mostRecentTile.SetActive (true);
		mostRecentTile.name = "New tile" + x + " " + y;
		

	}



	void generateGrid() //creates a grid based on the dimensions provided
	{
		if (blankGrid == false) {
			Texture2D t = LevelGenerator.me.thresholdedNoise ();
			gridDimensions = new Vector2 (t.width, t.height);
			for (int x = 0; x < t.width; x++) {
				for (int y = 0; y < t.height; y++) {
					Color curPixel = t.GetPixel (x, y);




					Vector3 posToCreateTile = new Vector3 (x, y, 0);
					GameObject mostRecentTile = (GameObject)Instantiate (getTileFromColour (curPixel), posToCreateTile, Quaternion.Euler (0, 0, 0));
					mostRecentTile.GetComponent<TileMasterClass> ().setGridCoords (new Vector2 (x, y));
					mostRecentTile.transform.parent = this.gameObject.transform;
					mostRecentTile.name = "Tile " + mostRecentTile.GetComponent<TileMasterClass> ().getGridCoords ().ToString ();
					int r = Random.Range (1, 10);
					gridOfTiles [x, y] = mostRecentTile.GetComponent<TileMasterClass> ();

					//if (r > 8) {
					//	mostRecentTile.GetComponent<TileMasterClass> ().setTileWalkable (false);
					//mostRecentTile.GetComponent<SpriteRenderer> ().color = Color.cyan;
					//}

				}
			}
		} else {
			for (int x = 0; x < gridDimensions.x; x++) {
				for (int y = 0; y < gridDimensions.y; y++) {
					Vector3 posToCreateTile = new Vector3 (x, y, 0);
					GameObject mostRecentTile = (GameObject)Instantiate (grass, posToCreateTile, Quaternion.Euler (0, 0, 0));
					mostRecentTile.GetComponent<TileMasterClass> ().setGridCoords (new Vector2 (x, y));
					mostRecentTile.transform.parent = this.gameObject.transform;
					mostRecentTile.name = "Tile " + mostRecentTile.GetComponent<TileMasterClass> ().getGridCoords ().ToString ();
					int r = Random.Range (1, 10);
					gridOfTiles [x, y] = mostRecentTile.GetComponent<TileMasterClass> ();

				}
			}
		}
	}

	public GameObject getTileFromColour(Color cl)
	{
		

		if (cl.r <= LevelGenerator.me.mountainLimit) {
			
			if (cl.grayscale >= LevelGenerator.me.colorsInThresh [0].grayscale && cl.grayscale < LevelGenerator.me.colorsInThresh [1].grayscale) {
				return mountain;
			} else {
				return rockey;
			}
		} else if (cl.r > LevelGenerator.me.mountainLimit && cl.r <= LevelGenerator.me.grassLimit) {
			
			if (cl.grayscale >= LevelGenerator.me.colorsInThresh [3].grayscale) {
				return grass;
			} else {
				return forest;
			}

		} else if (cl.r > LevelGenerator.me.grassLimit && cl.r <= LevelGenerator.me.seaLimit) {

			if (cl.grayscale <= LevelGenerator.me.colorsInThresh [5].grayscale) {
				return water;
			} else {
				return ocean;
			}
		
		} else if (cl.r > LevelGenerator.me.seaLimit) {
			return prefabTile;
		}



		return prefabTile;

	}

	public TileMasterClass getTile(int x,int y) //added try catch incase player clicks out of grid
	{
		try{
			return gridOfTiles [x, y];
		}
		catch{
			return null;
		}
	}

	public List<GameObject> getTiles(Vector2 startPos,Vector2 endPos) //gets a section of the grid based on the coords passed in
	{
		Debug.Log ("Getting Tiles");
		int lowestX, lowestY,highestX,highestY;
		List<GameObject> retVal = new List<GameObject>();
		if (startPos.x <= endPos.x) {
			lowestX = (int)startPos.x;
			highestX = (int)endPos.x;
		} else {
			lowestX = (int)endPos.x;
			highestX = (int)startPos.x;
		}

		if (startPos.y <= endPos.y) {
			lowestY = (int)startPos.y;
			highestY = (int)endPos.y;
		} else {
			lowestY = (int)endPos.y;
			highestY = (int)startPos.y;
		}


		for(int x = (int)lowestX;x<=(int)highestX;x++)
		{
			for (int y = (int)lowestY; y <= (int)highestY; y++) {
				retVal.Add (gridOfTiles [x, y].gameObject);
			}
		}
		Debug.Log (retVal.Count);
		return retVal;

	}
		

	public List<TileMasterClass> getTileNeighbors(TileMasterClass Tile) //gets passed in a tile and based on the coords in the grid it will get the neighboring tiles
	//and return them as a list
	{
		List<TileMasterClass> retVal = new List<TileMasterClass>();
		TileMasterClass t = Tile;

		Vector2 pos = t.getGridCoords ();

		if (pos.x == 0) {
			
			if (pos.y == 0) {
				//bottom left
				retVal.Add(gridOfTiles[(int)pos.x+1,(int)pos.y]);
				retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y+1]);

			} else if (pos.y == gridDimensions.y - 1) {
				//top left
				retVal.Add(gridOfTiles[(int)pos.x+1,(int)pos.y]);
				retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y-1]);
			} else {
				retVal.Add(gridOfTiles[(int)pos.x+1,(int)pos.y]);
				retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y+1]);
				retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y-1]);
			}

		} else if (pos.x == gridDimensions.x - 1) {
			
			if (pos.y == 0) {
				//bottom right
				retVal.Add(gridOfTiles[(int)pos.x-1,(int)pos.y]);
				retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y+1]);
			} else if (pos.y == gridDimensions.y - 1) {
				//top right
				retVal.Add(gridOfTiles[(int)pos.x-1,(int)pos.y]);
				retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y-1]);
			} else {
				retVal.Add(gridOfTiles[(int)pos.x-1,(int)pos.y]);
				retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y+1]);
				retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y-1]);
			}

		} else {
			//grid not on hor x
			if (pos.y == 0) {
				//bottom right
				retVal.Add (gridOfTiles [(int)pos.x - 1,(int) pos.y]);
				retVal.Add (gridOfTiles [(int)pos.x, (int)pos.y + 1]);
				retVal.Add (gridOfTiles [(int)pos.x+1, (int)pos.y]);
			} else if (pos.y == gridDimensions.y - 1) {
				retVal.Add (gridOfTiles [(int)pos.x - 1,(int) pos.y]);
				retVal.Add (gridOfTiles [(int)pos.x, (int)pos.y - 1]);
				retVal.Add (gridOfTiles [(int)pos.x+1, (int)pos.y]);
			} else {
				retVal.Add (gridOfTiles [(int)pos.x - 1, (int)pos.y]);
				retVal.Add (gridOfTiles [(int)pos.x, (int)pos.y - 1]);
				retVal.Add (gridOfTiles [(int)pos.x+1, (int)pos.y]);
				retVal.Add (gridOfTiles [(int)pos.x, (int)pos.y +1]);
			}
		}

		return retVal;
	}
}
