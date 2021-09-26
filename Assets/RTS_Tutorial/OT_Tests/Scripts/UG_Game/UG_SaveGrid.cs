using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class UG_SaveGrid : MonoBehaviour
{
    public static UG_SaveGrid sharedIntance;

    public string fileName;
    public string gridDataFolder = "";
    private void Awake()
    {
        if (sharedIntance == null) sharedIntance = this;
        string str = System.IO.Path.Combine(Application.persistentDataPath, "GridData/" + fileName + ".dat");
        if(Directory.Exists(Application.persistentDataPath + "/GridData") == false)
        {
            Debug.Log("Creating folder");
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/GridData");
            Debug.Log(Application.persistentDataPath + "/GridData");
        } else
        {
            Debug.Log("Directory exist");
        }

        gridDataFolder = str;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            writeGridToFile();
        }
        
        if(Input.GetKeyDown(KeyCode.X))
        {
            readGridFromFile();
        }
    }

    private void writeGridToFile()
    {
        GridData gd = new GridData();
        gd.gridWidth = (int)UG_GridGenerator.sharedInstance.gridDimensions.x;
        gd.gridHeight = (int)UG_GridGenerator.sharedInstance.gridDimensions.y;
        gd.tilesInGrid = new List<TileData>();

        foreach(UG_TileMasterClass tile in UG_GridGenerator.sharedInstance.getTiles())
        {
            TileData td = new TileData();
            td.type = tile.type;
            td.gridX = (int)tile.getGridCoors().x;
            td.gridY = (int)tile.getGridCoors().y;
            td.walkable = tile.isTileWalkeable();
            gd.tilesInGrid.Add(td);
        }

        try {

            if(File.Exists(gridDataFolder) == false)
                File.Create(gridDataFolder);

            FileStream file = new FileStream(gridDataFolder, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(file, gd);
            file.Close();

            Debug.Log("DATA WRITTEN TO " + gridDataFolder);
        } catch (Exception ex)
        {
            Debug.Log("SOMETHING WEN WRONG WRITING DATA");
            Debug.Log(ex.ToString());
        }
    }

    internal void readGridFromFile()
    {
        if(File.Exists(gridDataFolder))
        {
            Debug.Log("Loading data from file");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fl = File.Open(gridDataFolder, FileMode.Open);

            GridData data = (GridData)bf.Deserialize(fl);
            fl.Close();

            createLoadedGrid(data);
        } else
        {
            Debug.Log("File you are trying to access does not exist");
        }
    }

    void createLoadedGrid(GridData g)
    {
        UG_TileMasterClass[,] loadedGrid = new UG_TileMasterClass[g.gridWidth, g.gridHeight];
        foreach(TileData tileData in g.tilesInGrid)
        {
            GameObject obj = (GameObject)Instantiate(UG_TileChangeManager.sharedInstance.getTilesByType(tileData.type), new Vector3(tileData.gridX, tileData.gridY, 0), Quaternion.identity);
            obj.transform.parent = this.transform;
            obj.name = tileData.gridX + " | " + tileData.gridY + " | " + tileData.type;
            obj.GetComponent<UG_TileMasterClass>().setGridCoors(new Vector2(tileData.gridX, tileData.gridY));
            obj.SetActive(true);
            loadedGrid[tileData.gridX, tileData.gridY] = obj.GetComponent<UG_TileMasterClass>();
        }
        UG_GridGenerator.sharedInstance.setGrid(loadedGrid);
    }
}

[System.Serializable]
class GridData
{
    public int gridWidth, gridHeight;
    public List<TileData> tilesInGrid;
}

[System.Serializable]
class TileData
{
    public string type;
    public int gridX, gridY;
    public bool walkable;
}
