using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;
public class SaveGrid : MonoBehaviour {
	public static SaveGrid me;
	/*Stuff to mention in episode
	 * made grid manager a child of game controller so that we can have a prefab that allows for a level to be spawned in any scene
	 * added this script to grid controller object 
	 * added type to tile master class and classes that inherit from it
	 * added serializeable classes to write data
	 * mention imported stuff
	 * added method to tile change manager to get tiles via type
	 * 
	 * saved example = "grid1"
	 * 
	 * MENTION UNITY TYPES CANT BE SERIALIZED SO WE HAVE TO FIND A WAY AROUND THAT WHICH WAS DONE WITH ADDING THE TYPE STRING
	 * */




	// Use this for initialization
	public string fileName;
	public string gridDataFolder = "";
	void Awake()
	{
		me = this;

		//Creates a directory in 
		string str = System.IO.Path.Combine(Application.persistentDataPath,"GridData/"+fileName+".dat");
		if (Directory.Exists (Application.persistentDataPath + "/GridData") == false) {
			Debug.Log ("Creating folder");
			System.IO.Directory.CreateDirectory (Application.persistentDataPath + "/GridData");
			Debug.Log (Application.persistentDataPath + "/GridData");
		} else {
			Debug.Log ("Directory exists");
		}

		//string filePath = gridDataFolder + "/gridData.dat";

		//if(File.Exists(filePath)==false)
		//{
		//	File.Create(filePath);
		//}

		gridDataFolder = str;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			writeGridToFile ();
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			readGridFromFile ();
		}
	}

	void writeGridToFile()
	{
		gridData gd = new gridData ();
		gd.gridWidth = (int)GridGenerator.me.gridDimensions.x;
		gd.gridHeight = (int)GridGenerator.me.gridDimensions.y;
		gd.tilesInGrid = new List<tileData> ();
		foreach (TileMasterClass g in GridGenerator.me.getTiles()) {//added getter for the grid
			tileData td = new tileData();
			//added type string to tiles which is set in awake
			td.type = g.type;
			td.gridX = (int)g.getGridCoords ().x;
			td.gridY = (int)g.getGridCoords ().y;
			td.walkable = g.isTileWalkable ();
			gd.tilesInGrid.Add (td);
		}


		try{
			//File.SetAttributes(gridDataFolder,FileAttributes.Normal);
			if(File.Exists(gridDataFolder)==false)
			{
				File.Create(gridDataFolder);
			}

			FileStream file= new FileStream(gridDataFolder,FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter();


			bf.Serialize(file, gd);
			file.Close();

			Debug.Log ("DATA WRITTEN TO " + gridDataFolder);
		}
		catch (System.Exception e){
			Debug.Log ("SOMETHING WENT WRONG WRITING DATA");
			Debug.Log (e.ToString ());
		}
	}

	public void readGridFromFile()
	{
		if (File.Exists (gridDataFolder)) {
			Debug.Log ("Loading data from file");
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(gridDataFolder, FileMode.Open);

			gridData data = (gridData)bf.Deserialize(file);
			file.Close();

			createLoadedGrid (data);


		} else {
			Debug.Log ("File you are trying to access does not exist");
		}
	}

	void createLoadedGrid(gridData g)
	{
		TileMasterClass[,] loadedGrid = new TileMasterClass[g.gridWidth,g.gridHeight];
		foreach (tileData td in g.tilesInGrid) {
			GameObject go = (GameObject)Instantiate (TileChangeManager.me.getTilesByType (td.type), new Vector3 (td.gridX, td.gridY, 0), Quaternion.Euler (0, 0, 0));
			go.transform.parent = this.transform;
			go.name = td.gridX + " | " + td.gridY + " | " + td.type;
			go.GetComponent<TileMasterClass> ().setGridCoords (new Vector2 (td.gridX, td.gridY));
			go.SetActive (true);
			loadedGrid [td.gridX, td.gridY] = go.GetComponent<TileMasterClass> ();
		}
		GridGenerator.me.setGrid (loadedGrid);
	}

}

[System.Serializable]
class gridData{
	public int gridWidth,gridHeight;
	public List<tileData> tilesInGrid;
}

[System.Serializable]
class tileData{
	public string type; //type of tile
	public int gridX,gridY; //position in the grid & world
	public bool walkable;
}
