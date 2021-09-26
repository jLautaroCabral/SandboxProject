using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GUIManager : MonoBehaviour {

	//creating cursor icons
	//created base classes for resources, used for identification now, will come back later when implementing resources
	//changed the isMoreThanOneTypeSelected bool to use == 1 rather than > 1

	public static GUIManager me;//acts as a store for any gui elements that are used by other scripts
	public GUIStyle ButtonGUIStyle;
	public Texture2D blackBoxSemiTrans;
	public Texture2D pickaxe,sickle,arrow,axe,spear;
	void Awake()
	{
		me = this;
	}

	public Texture2D getBlackBoxSemiTrans()
	{
		return blackBoxSemiTrans;
	}


	float originalWidth = 1920.0f; //scaleing will make the gui proportional to what you see onscreen no matter the resolution, will cause stretching
	float originalHeight = 1080.0f;
	Vector3 scale;

	selectingModes selectionMode;

	void detectObjAtCursor()
	{
		if (SelectionManager.me.moreThanOneTypeSelected () == true) {
			//more than one unit is selected, could display multiple icons or just the arrow?
			decideCursorIcon ("");
		} else {
			Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

			RaycastHit2D raycast = Physics2D.Raycast (mousePos, Vector2.zero, 0f);
			try {
				GameObject hitObject = raycast.collider.gameObject;

				//Debug.Log (hitObject.name);

				List<string> selType = SelectionManager.me.getSelectedTypes();

				if(selType[0]=="Worker")//workers can attack, gather resources and move
				{
					if (hitObject.GetComponent<Resource> () == true) {
						decideCursorIcon (hitObject.GetComponent<Resource> ().myType);
					} else if (hitObject.tag == "Unit") {
						if(hitObject.GetComponent<FactionIdentifier>()==true)
						{
							decideCursorIcon ("Enemy");
						}
					} else {
						decideCursorIcon ("");
					}
				}
				else if(selType[0]=="Hoplite"||selType[0]=="Archer")//hoplites and archers can just attack and move
				{
					if (hitObject.tag == "Unit") {//will need to add in propper enemy checking when they are implemented
						if(hitObject.GetComponent<FactionIdentifier>()==true)
						{
							decideCursorIcon ("Enemy");
						}
					} else {
						decideCursorIcon ("");
					}
				}
			
			} catch {
				Debug.Log ("No valid object selected");
				//clearSelected ();
				decideCursorIcon ("");
			}
		}
	}

	void decideCursorIcon(string str)
	{
		if (str == "Iron" || str == "Stone" || str == "Gold") {
			//set cursor to pickaxe
			Cursor.SetCursor(pickaxe,new Vector2(5,5),CursorMode.ForceSoftware);
		} else if (str == "Wood") {
			//set cursor to axe
			Cursor.SetCursor(axe,new Vector2(5,5),CursorMode.ForceSoftware);
		} else if (str == "Wheat") {
			//set cursor to sickle
			Cursor.SetCursor(sickle,new Vector2(5,5),CursorMode.ForceSoftware);
		} else if (str == "Enemy") {
			//set cursor to spear
			Cursor.SetCursor(spear,new Vector2(5,5),CursorMode.ForceSoftware);
		} else {
			//set cursor to arrow
			Cursor.SetCursor(arrow,new Vector2(5,5),CursorMode.ForceSoftware);

		}
	}

	// Update is called once per frame
	void Update () {
		selectionMode = SelectionManager.me.selectionMode;
		if (selectionMode == selectingModes.units) {
			detectObjAtCursor ();
		}
	}

	void OnGUI()
	{
		GUI.depth = 0;
		scale.x = Screen.width/originalWidth;
		scale.y = Screen.height/originalHeight;
		scale.z =1;
		var svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);

		if (selectionMode == selectingModes.tiles) {
			// n/a
		} else if (selectionMode == selectingModes.creatingBuildings) {
			drawGUIBackground ();
			drawBuildingConstructButtons ();
		} else if (selectionMode == selectingModes.units) { //new for rts1
			//gui for creating units
			drawGUIBackground ();
			if (SelectionManager.me.moreThanOneTypeSelected () == true) {
				drawFilterUnitButtons ();
			} else {
				//draw selected unit info
				drawUnitInfo ();
				drawUnitOrderButtons ();
			}
		} else if (selectionMode == selectingModes.buildings) {
			drawGUIBackground ();
			drawBuildingActionButtons ();
		}
		GUI.matrix = svMat;
	}



	void drawFilterUnitButtons()
	{
		List<string> types = SelectionManager.me.getSelectedTypes ();
		int xOff = 0;
		int yOff = 0;
		foreach(string t in types) {
			Rect buttonPos = new Rect (50 + (200*xOff),originalHeight-(originalHeight/5)+(50 + (100*yOff)),200,100);
			if (t == "Worker") {
				if(GUI.Button(buttonPos,"Filter Workers",ButtonGUIStyle))
				{
					SelectionManager.me.filterWorkers ();
					Debug.Log ("Filtering workers");
				}
				xOff++;
			}

			if (t == "Hoplite") {
				if(GUI.Button(buttonPos,"Filter Hoplites",ButtonGUIStyle))
				{
					SelectionManager.me.filterHoplites ();
					Debug.Log ("Filtering hoplites");
				}
				xOff++;
			}

			if (t == "Archer") {
				if(GUI.Button(buttonPos,"Filter Archers",ButtonGUIStyle))
				{
					SelectionManager.me.filterArchers ();
					Debug.Log ("Filtering archers");
				}
				xOff++;
			}

			if (xOff > 1) {
				xOff = 0;
				yOff++;
			}
		}
	}


	void drawUnitOrderButtons()
	{
		string[] types = SelectionManager.me.getSelected () [0].GetComponent<UnitMasterClass> ().getAvailableActions ();
		int xOff = 0;
		int yOff = 0;
		foreach(string t in types)
		{
			Rect buttonPos = new Rect (550 + (200*xOff),originalHeight-(originalHeight/5)+(20 + (100*yOff)),200,100);

			if (t == "Patrol") {
				if (GUI.Button (buttonPos, "Patrol",ButtonGUIStyle)) {
					foreach (GameObject g in SelectionManager.me.getSelected()) {
						g.GetComponent<UnitMasterClass> ().patrol ();
					}
					Debug.Log ("Setting to patrol");
				}
				xOff++;
			}

			if (t == "Delete") {
				if (GUI.Button (buttonPos, "Delete Unit",ButtonGUIStyle)) {
					foreach (GameObject g in SelectionManager.me.getSelected()) {
						g.GetComponent<UnitMasterClass> ().destroyMe ();
					}
				}
				xOff++;
			}

			if (t == "Scout") {
				if (GUI.Button (buttonPos, "Scout",ButtonGUIStyle)) {
					foreach (GameObject g in SelectionManager.me.getSelected()) {
						g.GetComponent<UnitMasterClass> ().scout ();
					}
				}
				xOff++;
			}

			if (t == "BuildBuildings") {
				if (GUI.Button (buttonPos, "Build Buidlings",ButtonGUIStyle)) {
					BuildingStore.me.storeWorkers ();
					SelectionManager.me.selectionMode = selectingModes.creatingBuildings;
				}
				xOff++;
			}

			if (xOff > 4) {
				xOff = 0;
				yOff++;
			}
		}
	}

	void drawBuildingConstructButtons()
	{
		int xOff = 0;
		int yOff = 0;

		foreach (GameObject b in BuildingStore.me.buildings) {
			Rect buttonPos = new Rect (550 + (200*xOff),originalHeight-(originalHeight/5)+(20 + (100*yOff)),200,100);
			try {
				Building buildingScr = b.GetComponent<Building> ();
				if (GUI.Button (buttonPos ,buildingScr.name,ButtonGUIStyle)) {
					BuildingStore.me.setSelectedBuilding(b);
				}
				xOff++;

				
				if(xOff==4)
				{
					yOff++;
					xOff=0;
				}
			} catch {
				Debug.Log ("Building missing a component");
			}
		}
	}

	void drawBuildingActionButtons(){
		
		List<GameObject> building = SelectionManager.me.getSelected();
		if (building.Count > 0) { //needs to check that a building is selected
			Building b = building [0].GetComponent<Building> ();
			List<BuildingAction> buildActions = b.actionsWeCanPerform;

			if (buildActions.Count > 0) {
				string queueInfo = "";
				if (b.buildingActionQueue.Count == 0) {
					queueInfo = "No actions queued";
				} else {
					queueInfo = "\n" + b.buildingActionQueue [0].getProgress () + "\n" + "There are " + (b.buildingActionQueue.Count - 1).ToString () + " actions remaining.";
				}

				string BuildingInfo = b.name + "\n" + "Building Health : " + b.health + " / " + b.maxHealth + "\n";

				Rect infoPos = new Rect (20, (originalHeight - (originalHeight / 5)) + 10, originalWidth / 4, (originalHeight / 5) - 20);//have slight offsets so there is a gap at the edge
				GUI.Box (infoPos, BuildingInfo + queueInfo);

				int xOff = 0;
				int yOff = 0;
					foreach (BuildingAction ba in buildActions) {
						Rect buttonPos = new Rect (550 + (200 * xOff), originalHeight - (originalHeight / 5) + (20 + (100 * yOff)), 200, 100);


						if (ba.canWeDo () == true) {
							if (GUI.Button (buttonPos, ba.getButtonText (), ButtonGUIStyle)) {
								BuildingAction baNew = (BuildingAction)Instantiate (ba, building [0].transform);
								building [0].gameObject.GetComponent<Building> ().buildingActionQueue.Add (baNew);
							}
						} else {
							GUI.Box (buttonPos, ba.getButtonText (), ButtonGUIStyle);
						}
						xOff++;

						if (xOff > 4) {
							xOff = 0;
							yOff++;
						}

					}
			}
		}
	}

	void drawUnitInfo()
	{
		//unit info is only displayed if one type of unit is selected so thats why we can use selected[0] and it will be correct
		List<GameObject> selected = SelectionManager.me.getSelected ();
		UnitMasterClass um = selected [0].GetComponent<UnitMasterClass> ();

		string unitName = um.unitType;
		float unitHealth = um.myHealth.health;
		float attackPower = um.myAttack.attackDamage;
		string combined = unitName + "\n" + "Health : " + unitHealth + "\n" + "Attack Damage : " + attackPower + "\n";
		Rect infoPos = new Rect(20,(originalHeight-(originalHeight/5)) +10, originalWidth/4,(originalHeight / 5) - 20);//have slight offsets so there is a gap at the edge
		GUI.Box(infoPos,combined);
	}

	void drawGUIBackground()
	{
		Rect bgPos = new Rect (0, originalHeight-(originalHeight/5), originalWidth, originalHeight / 5);
		GUI.Box (bgPos, "");
	}

}
