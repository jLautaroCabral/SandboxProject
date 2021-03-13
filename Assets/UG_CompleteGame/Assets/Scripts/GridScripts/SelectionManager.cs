using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour {
	


	public static SelectionManager me;
	public selectingModes selectionMode;
	public GameObject buildingPlaceCursor;
	[SerializeField]
	List<GameObject> currentlySelected; 

	bool drawMultiSelectBox = false;

	// Use this for initialization
	void Awake () {
		me = this;
		currentlySelected = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentlySelected.Count == 0 && selectionMode != selectingModes.creatingBuildings && selectionMode != selectingModes.tiles) {//mainly will just select buildings and units so this allows for the player to easily switch between the two while not being interrupted if they're deciding where to place a building
			decideSelectionMode ();
		} 

		if (selectionMode == selectingModes.tiles) {
			Tiles_checkForLeftMouseClick ();
			resetStartPositions ();
		} else if (selectionMode == selectingModes.creatingBuildings) {
			CreatingBuildings_checkForLeftMouseClick ();
			resetStartPositions ();
		} else if (selectionMode == selectingModes.units) { //new for rts1
			Units_checkForLeftMouseClick ();
		} else if (selectionMode == selectingModes.buildings) {
			buildings_checkForLeftClick ();
			resetStartPositions ();
		}

		shouldWeDisplayBuildingConstructCursor ();
	}

	void resetStartPositions()
	{
		startPosition = Vector3.zero;
		endPosition = Vector3.zero;
		startWorld = Vector3.zero;
		endWorld = Vector3.zero;
	}

	void decideSelectionMode ()
	{
		if (getSelectionRaycast () == null) {
			selectionMode = selectingModes.units;

		} else {
			if (getSelectionRaycast ().tag == "Building") {
				selectionMode = selectingModes.buildings;
			} else {
				selectionMode = selectingModes.units;
			}
		}
	}

	void shouldWeDisplayBuildingConstructCursor()
	{
		if (selectionMode == selectingModes.creatingBuildings) {
			buildingPlaceCursor.SetActive (true);
		} else {
			buildingPlaceCursor.SetActive (false);
		}
	}

	void clearSelectedIfAnyNull()
	{
		foreach(GameObject g in currentlySelected)
		{
			if (g == null) {
				clearSelected ();
				break;
			}
		}
	}

	GameObject createBuildingAtLocation(Vector3 cursorPos,int width,int height) //creates a building from the one currently selected at the mouse position
	{
		//gets the lowest and highest of each axis, works cause the selected tiles are in increasing order
		int xLowBound = (int)currentlySelected[0].GetComponent<TileMasterClass> ().getGridCoords ().x;
		int xHighBound = (int)currentlySelected[currentlySelected.Count-1].GetComponent<TileMasterClass> ().getGridCoords ().x;
		int yLowBound = (int)currentlySelected[0].GetComponent<TileMasterClass> ().getGridCoords ().y;
		int yHighBound = (int)currentlySelected[currentlySelected.Count-1].GetComponent<TileMasterClass> ().getGridCoords ().y;

		for (int x = 0; x < currentlySelected.Count - 1; x++) { //goes through all the tiles and makes the ones not on the outside (where the actual building will be placed unwalkable
			GameObject tile = currentlySelected [x];
			TileMasterClass tm = tile.GetComponent<TileMasterClass> ();

			Vector2 curTileGrid = tm.getGridCoords ();
			if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound) {
				if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound) {
					//Debug.LogError ("Making " + tile.name + " Unwalkable ");
//					Debug.LogError ("Keeping " + tile.name + " Walkable");

				} 
			} else if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound) {
				if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound) {
					//Debug.LogError ("Making " + tile.name + " Unwalkable ");
//					Debug.LogError ("Keeping " + tile.name + " Walkable");
				
				}
			} else {
				tm.setTileWalkable (false);
			}
		}

		//this code creates the building
		Vector3 spawnPos = currentlySelected [(currentlySelected.Count - 1)/2].transform.position;//need to have a play with this to make it put the building nearer to the center point
		spawnPos.z = -1;
		GameObject built = (GameObject)Instantiate (BuildingStore.me.getToBuild (), spawnPos, Quaternion.Euler (0, 0, 0));
		SpriteRenderer sr = built.AddComponent<SpriteRenderer> ();
		sr.sprite = built.GetComponent<Building> ().buildingSprite;
		sr.sortingOrder = 10;
		built.AddComponent<BoxCollider2D> ();
		built.SetActive (true);
		built.tag = "Building";

		//new code for getting a free tile near buildings & add to list of buildings in scene
		int midX = xHighBound - ((xHighBound - xLowBound)/2);
		int midY = yHighBound - ((yHighBound - yLowBound)/2);
		built.GetComponent<Building> ().setTileNearMe (GridGenerator.me.getTile(midX,midY));
		BuildingStore.me.buildingsInScene.Add (built.GetComponent<Building>());


		clearSelected ();

		return built;
	}

	bool isCurrentLocAValidForConstruction() //basicly goes through the current selection of tiles and checks if they are a valid place to build a building
	{
		if (currentlySelected.Count <= 0) {
			return false;//there isn't a tile at the mouse cursor for the script to use
		}


		//for a building to be placed at a location there has to be 2 conditions met all the tiles must be walkable & the building must have a ring of walkable tiles around it

		foreach (GameObject tile in currentlySelected) {
			TileMasterClass tm = tile.GetComponent<TileMasterClass> ();
			if (tm.isTileWalkable () == false) {
				return false;
			}
		}

		int xLowBound = (int)currentlySelected[0].GetComponent<TileMasterClass> ().getGridCoords ().x;//(int)cursorPos.x - ((width + 0)/2);
		int xHighBound = (int)currentlySelected[currentlySelected.Count-1].GetComponent<TileMasterClass> ().getGridCoords ().x;//(int)cursorPos.x + ((width + 0)/2);
		int yLowBound = (int)currentlySelected[0].GetComponent<TileMasterClass> ().getGridCoords ().y;//(int)cursorPos.y - ((height + 0) / 2);
		int yHighBound = (int)currentlySelected[currentlySelected.Count-1].GetComponent<TileMasterClass> ().getGridCoords ().y;//(int)cursorPos.y + ((height + 0) / 2);

		for (int x = 0; x < currentlySelected.Count - 1; x++) {
			GameObject tile = currentlySelected [x];
			TileMasterClass tm = tile.GetComponent<TileMasterClass> ();

			Vector2 curTileGrid = tm.getGridCoords ();
			if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound) {
				if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound) {
					if (tm.isTileWalkable () == false) {
						return false; //should make sure all edge tiles are walkable
					}
				} 
			}

			if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound) {
				if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound) {
					if (tm.isTileWalkable () == false) {
						return false; //should make sure all edge tiles are walkable
					}
				}
			}
		}

		return true;
	}


	void CreatingBuildings_checkForLeftMouseClick() 
	{
		
		GameObject selectedBuilding = BuildingStore.me.getToBuild ();
		if (selectedBuilding != null) {
			
			Building selectedBuildScript = selectedBuilding.GetComponent<Building> ();
			if (selectedBuildScript != null) { 
				
				getMultipleTilesFromCoords (selectedBuildScript.widthInTiles, selectedBuildScript.heightInTiles); 
				//buildingPlaceCursor.GetComponent<SpriteRenderer> ().sprite = selectedBuildScript.buildingSprite; 
				CursorCollisionCheck.me.setSprite(selectedBuildScript.buildingSprite);
				buildingPlaceCursor.transform.position =  new Vector3(Camera.main.ScreenToWorldPoint (Input.mousePosition).x,Camera.main.ScreenToWorldPoint (Input.mousePosition).y,0);

				Color cursorColour = new Color (1, 1, 1, 0.5f);
				buildingPlaceCursor.GetComponent<SpriteRenderer> ().color = cursorColour; 

				if (isCurrentLocAValidForConstruction () == true && ResourceManager.me.canWeConstructBuilding(BuildingStore.me.getBuildingScr())==true && CursorCollisionCheck.me.overUnit == false && building_pathfindBlockCheck()==true) { //will need to add condition for checking if there are enough resources to build it
					buildingPlaceCursor.GetComponent<SpriteRenderer> ().color = cursorColour; 
				} else {
					cursorColour = new Color (1, 0, 0, 0.5f);
					buildingPlaceCursor.GetComponent<SpriteRenderer> ().color = cursorColour;
				}

				if (Input.GetMouseButtonDown (0)) {

					if (isCurrentLocAValidForConstruction ()==true&& ResourceManager.me.canWeConstructBuilding(BuildingStore.me.getBuildingScr())==true&& CursorCollisionCheck.me.overUnit == false&& building_pathfindBlockCheck()==true) { 
						GameObject built = createBuildingAtLocation (buildingPlaceCursor.transform.position, selectedBuildScript.widthInTiles, selectedBuildScript.heightInTiles);
						ResourceManager.me.buildBuilding (BuildingStore.me.curBuildScr);//new line for building resource reduction
						BuildingStore.me.returnWorkers ();
						Vector3 mousePos = Input.mousePosition;
						Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint (mousePos);

						foreach (GameObject g in currentlySelected) {
							
							UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
							UGAction a = g.AddComponent<Action_BuildBuilding> ();

							if (um.canWePerformAction (a) == true) { //added check to make sure we can perform the action
								a.initaliseTarget(built);
								um.actionsQueue.Add (a);
								a.enabled = false;
							} else {
								Destroy (a);

							}
						}

					}
				}
			}
		} 

		if (Input.GetMouseButtonDown (1)) { //on right click return to unit selection
			BuildingStore.me.returnWorkers();
		}
	}

	public void setSelected(GameObject toSet)
	{
		clearSelected();
		if (toSet.GetComponent<FactionIdentifier> () == false) {
			currentlySelected.Add (toSet);
		}

		if (toSet.GetComponent<TileMasterClass> () == true) {
			toSet.GetComponent<TileMasterClass> ().OnSelect (); 
		}
	}
		

	public void setSelected(List<GameObject> toSet,bool clearExisting)
	{
		if (clearExisting == true) {
			clearSelected ();
		}

		foreach (GameObject g in toSet) {
			if (g.GetComponent<FactionIdentifier> () == false) {
				currentlySelected.Add (g);
			}
		}

		currentlySelected = toSet;

		foreach (GameObject obj in getSelected()) {
			if (obj.GetComponent<TileMasterClass> () == true) {
				obj.GetComponent<TileMasterClass> ().OnSelect ();
			}
		}
		clearSelectedIfAnyNull ();
	}

	public List<GameObject> getSelected()
	{
		return currentlySelected;
	}

	void clearSelected() 
	{
		firstTileSelected = null;
		lastTileSelected = null;

		foreach (GameObject obj in getSelected()) {
			if (obj.GetComponent<TileMasterClass> () == true) {
				obj.GetComponent<TileMasterClass> ().OnDeselect ();
			}
		}
		currentlySelected = new List<GameObject> ();
	}

	GameObject firstTileSelected = null;
	GameObject lastTileSelected = null;


	public Vector3 startPosition,startWorld;
	public Vector3 endPosition,endWorld;
	void Units_checkForLeftMouseClick()
	{
		if (Input.GetKey (KeyCode.LeftControl) == false) {
			drawMultiSelectBox = false;//added this to make sure the box closes when the user stops pressing ctrl
			if (Input.GetMouseButtonDown (2)) {
				clearSelected ();
			}

			if (Input.GetMouseButtonDown (0)) {
				Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

				RaycastHit2D raycast = Physics2D.Raycast (mousePos, Vector2.zero, 0f);
				try {
					GameObject hitObject = raycast.collider.gameObject;

					Debug.Log (hitObject.name);


					if (hitObject.tag == "Unit" ) { //have to also make sure that units have a z value of -1 so that the colliders on the tiles don't block the raycast, maybe use a mask to avoid this issue?
						setSelected (hitObject);
					}
					else{
						//clearSelected();
					}
				} catch {
					Debug.Log ("No valid object selected");
					//clearSelected ();
				}
			}
		} else {


			if (Input.GetMouseButtonDown (0)) {
				startPosition = Input.mousePosition; //need to make these with the camera.ToWorldPosition
				startWorld = Camera.main.ScreenToWorldPoint(startPosition);
			}

			if(Input.GetMouseButton(0))
			{
				drawMultiSelectBox=true;
				endPosition = Input.mousePosition;
				endWorld = Camera.main.ScreenToWorldPoint (endPosition);
			}

			if (Input.GetMouseButtonUp (0)) {
				
				drawMultiSelectBox = false;
				setSelected (UnitManager.me.getUnitsWithinArea(startWorld,endWorld),true);
				startWorld = Vector3.zero;
				endWorld = Vector3.zero;
				endPosition = Vector3.zero;
				startPosition = Vector3.zero;
			}


		}
	}

	bool building_pathfindBlockCheck()
	{
		foreach (GameObject g in getSelected()) {
			TileMasterClass tm = g.GetComponent<TileMasterClass> ();
			if (tm.myNode.isLocationValidForBuilding () == false) {
				return false;
			}
		}
		return true;
	}

	void Tiles_checkForLeftMouseClick() 
	{
		if (Input.GetKey (KeyCode.LeftControl) == true) {
			Debug.Log ("leftCtrl");
			if (Input.GetMouseButtonDown (0) == true) {
				Debug.Log ("Multi select");
				if (firstTileSelected == null) {
					Debug.Log ("Selecting first tile");
					selectionRaycast (ref firstTileSelected);
				} else if (firstTileSelected != null && lastTileSelected != null) {
					Vector2 startCoords = firstTileSelected.GetComponent<TileMasterClass> ().getGridCoords ();
					Vector2 endCoords = lastTileSelected.GetComponent<TileMasterClass> ().getGridCoords ();
					Debug.Log ("Start " + startCoords);
					Debug.Log ("END " + endCoords);
					if (firstTileSelected != null && lastTileSelected != null) {
						List<GameObject> selectedTiles = GridGenerator.me.getTiles (startCoords, endCoords);
						setSelected (selectedTiles, true);


						replaceTiles();
						firstTileSelected = null;
						lastTileSelected = null;
					}
				} 
			}

			if (Input.GetMouseButton (0) == false && firstTileSelected != null) {
				Debug.Log ("Selecting second tile");
				drawMultiSelectBox = true;
				selectionRaycast (ref lastTileSelected);
			} else {
				drawMultiSelectBox = false;
			}
		} else if (Input.GetMouseButtonDown (0) == true) {
			firstTileSelected = null;
			lastTileSelected = null;
			Debug.Log ("Clicking, looking for raycast hits");
			selectionRaycast (); 
			replaceTiles (); 
		} else {
			drawMultiSelectBox = false;
		}
	}

	void replaceTiles() //new for tile alteration 
	{
		if (TileChangeManager.me.getSelectedTile () != null) {
			GridGenerator.me.ChangeTilesInGrid (currentlySelected);
			clearSelected ();
		}
	}


	void getMultipleTilesFromCoords(int width,int height) 
	{
		try{ 
			width += 2;
			height += 2;
			GameObject tileAtMousePoint=null;

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			tileAtMousePoint = GridGenerator.me.getTile((int)mousePos.x,(int)mousePos.y).gameObject;
			//selectionRaycast (ref tileAtMousePoint);//have to change so that it doesnt hit the collider for the building placement
			TileMasterClass tm = tileAtMousePoint.GetComponent<TileMasterClass> (); 
			Vector2 tileGridCoords = tm.getGridCoords ();

			if (isSelectionInGridRange (tileGridCoords, width, height) == true) {
				Debug.Log ("Enough Space");
				Vector2 startPos = new Vector2 (tileGridCoords.x - (width / 2), tileGridCoords.y - (height / 2));
				Vector2 endPos = new Vector2 (tileGridCoords.x + (width / 2), tileGridCoords.y + (height / 2));
				setSelected(GridGenerator.me.getTiles(startPos,endPos),true);
			} else {
				clearSelected ();
				Debug.Log ("Not enough space");
			}
		}
		catch{
			Debug.Log ("No tile at mouse position");
		}
	}

	bool isSelectionInGridRange(Vector2 centerCoords,int width,int height)
	{
		width /= 2;
		height /= 2;
		if ((centerCoords.x - width) < 0 || (centerCoords.y - height) < 0 || (centerCoords.x + width) >= GridGenerator.me.gridDimensions.x || (centerCoords.y + height) >= GridGenerator.me.gridDimensions.y) {
			return false;
		} else {
			return true;
		}
	}




	void selectionRaycast()
	{
		Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint (Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

		RaycastHit2D raycast = Physics2D.Raycast (mousePos, Vector2.zero, 0f);
		try{
			GameObject hitObject = raycast.collider.gameObject;

	//		Debug.Log(hitObject.name);

			setSelected(hitObject);
		}
		catch{
			Debug.Log ("No valid object selected");
		}
	}
		
	void selectionRaycast(ref GameObject objToSet) 
	{
		objToSet = getSelectionRaycast ();
	}

	GameObject getSelectionRaycast()
	{
		Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint (Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

		RaycastHit2D raycast = Physics2D.Raycast (mousePos, Vector2.zero, 0f);
		try{
			GameObject hitObject = raycast.collider.gameObject;

//			Debug.Log(hitObject.name);

			return hitObject;
		}
		catch{
			Debug.Log ("No valid object selected");
		}

		return null;
	}

	void buildings_checkForLeftClick()
	{
		if (Input.GetMouseButtonDown (0)) {
			GameObject g = getSelectionRaycast ();

			if (g.tag == "Building") {
				setSelected (g);
			} 

		}

		if (Input.GetMouseButtonDown (1)) {
			clearSelected ();
		}
	}


	void OnGUI()
	{
		if (selectionMode == selectingModes.tiles) {
			if (drawMultiSelectBox == true) { 
				Vector3 startScreenPos = Camera.main.WorldToScreenPoint (firstTileSelected.transform.position);

				Vector3 endScreenPos = Input.mousePosition;

				float width, height;
				if (startScreenPos.x > endScreenPos.x) {
					width = startScreenPos.x - endScreenPos.x;
				} else {
					width = endScreenPos.x - startScreenPos.x;
				}

				if (startScreenPos.y > endScreenPos.y) {
					height = startScreenPos.y - endScreenPos.y;
				} else {
					height = endScreenPos.y - startScreenPos.y;
				}
				Rect posToDrawBox;

				if (endScreenPos.x > startScreenPos.x) {
					if (endScreenPos.y > startScreenPos.y) {
						posToDrawBox = new Rect (startScreenPos.x, Screen.height - endScreenPos.y, width, height);
					} else {
						posToDrawBox = new Rect (startScreenPos.x, Screen.height - startScreenPos.y, width, height);
					}
				} else {
					if (endScreenPos.y > startScreenPos.y) {
						posToDrawBox = new Rect (endScreenPos.x, Screen.height - endScreenPos.y, width, height);
					} else {
						posToDrawBox = new Rect (endScreenPos.x, Screen.height - startScreenPos.y, width, height);
					}
				}

				GUI.DrawTexture (posToDrawBox, GUIManager.me.getBlackBoxSemiTrans ());
			}
		} else if (selectionMode == selectingModes.units) {
			if (drawMultiSelectBox == true) { 
				Vector3 startScreenPos = startPosition;

				Vector3 endScreenPos = Input.mousePosition;

				float width, height;
				if (startScreenPos.x > endScreenPos.x) {
					width = startScreenPos.x - endScreenPos.x;
				} else {
					width = endScreenPos.x - startScreenPos.x;
				}

				if (startScreenPos.y > endScreenPos.y) {
					height = startScreenPos.y - endScreenPos.y;
				} else {
					height = endScreenPos.y - startScreenPos.y;
				}
				Rect posToDrawBox;

				if (endScreenPos.x > startScreenPos.x) {
					if (endScreenPos.y > startScreenPos.y) {
						posToDrawBox = new Rect (startScreenPos.x, Screen.height - endScreenPos.y, width, height);
					} else {
						posToDrawBox = new Rect (startScreenPos.x, Screen.height - startScreenPos.y, width, height);
					}
				} else {
					if (endScreenPos.y > startScreenPos.y) {
						posToDrawBox = new Rect (endScreenPos.x, Screen.height - endScreenPos.y, width, height);
					} else {
						posToDrawBox = new Rect (endScreenPos.x, Screen.height - startScreenPos.y, width, height);
					}
				}

				GUI.DrawTexture (posToDrawBox, GUIManager.me.getBlackBoxSemiTrans ());
			}
		}
	}

	public bool moreThanOneTypeSelected()
	{
		List<string> types = getSelectedTypes ();

		if (types.Count == 1 ) {
			return false;
		} else {
			return true;
		}
	}

	public List<string> getSelectedTypes()
	{
		//get all the types of units selected
		List<string> retval = new List<string>();
		List<GameObject> workers = new List<GameObject> ();

		foreach (GameObject g in getSelected()) {
			if (g.GetComponent<Worker> () == true) {
				workers.Add (g);
			}
		}

		List<GameObject> hoplites = new List<GameObject> ();

		foreach (GameObject g in getSelected()) {
			if (g.GetComponent<Hoplite> () == true) {
				hoplites.Add (g);
			}
		}

		List<GameObject> archers = new List<GameObject> ();

		foreach (GameObject g in getSelected()) {
			if (g.GetComponent<Archer> () == true) {
				archers.Add (g);
			}
		}

		if (workers.Count > 0) {
			retval.Add ("Worker");
		}

		if (hoplites.Count > 0) {
			retval.Add ("Hoplite");
		}

		if (archers.Count > 0) {
			retval.Add ("Archer");
		}

		return retval;
	}

	public void filterWorkers()
	{
		List<GameObject> workers = new List<GameObject> ();

		foreach (GameObject g in getSelected()) {
			Debug.Log (g.GetComponent<UnitMasterClass> ().unitType);
			if (g.GetComponent<UnitMasterClass>().unitType == "Worker") {
				workers.Add (g);

			}
		}

		setSelected (workers, true);
	}

	public void filterHoplites()
	{
		List<GameObject> hoplites = new List<GameObject> ();

		foreach (GameObject g in getSelected()) {
			Debug.Log (g.GetComponent<UnitMasterClass> ().unitType);
			if (g.GetComponent<UnitMasterClass>().unitType == "Hoplite") {
				hoplites.Add (g);
			}
		}

		setSelected (hoplites, true);
	}

	public void filterArchers()
	{
		List<GameObject> archers = new List<GameObject> ();

		foreach (GameObject g in getSelected()) {
			Debug.Log (g.GetComponent<UnitMasterClass> ().unitType);
			if (g.GetComponent<UnitMasterClass>().unitType == "Archer") {
				archers.Add (g);
			}
		}

		setSelected (archers, true);
	}
}

public enum selectingModes{
	tiles,
	units,
	creatingBuildings,
	buildings
}
