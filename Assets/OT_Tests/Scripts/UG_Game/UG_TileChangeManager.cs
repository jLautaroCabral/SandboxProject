using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_TileChangeManager : MonoBehaviour
{
    public static UG_TileChangeManager sharedInstance;
    public List<GameObject> tilesWeCanUse;
    public GameObject selectedTile;
    void Start()
    {
        if (sharedInstance == null) sharedInstance = this;
        checkValidTiles();
    }

    private void checkValidTiles()
    {
        foreach (GameObject g in tilesWeCanUse)
        {
            if(g.GetComponent<UG_TileMasterClass>() == null)
            {
                tilesWeCanUse.Remove(g);
                checkValidTiles();
                break;
            }
        }
    }

    public GameObject getSelectedTile()
    {
        return selectedTile;
    }

    public GameObject getTilesByType(string type)
    {
        foreach(GameObject obj in tilesWeCanUse)
        {
            UG_TileMasterClass tm = obj.GetComponent<UG_TileMasterClass>();
            if(tm.type == type)
                return obj;
        }
        return tilesWeCanUse[0];
    }

    private void OnGUI()
    {
        if(UG_SelectionManager.sharedInstance.selectionMode == SelectionMode.tile)
        {
            int yMod = 0;

            foreach(GameObject obj in tilesWeCanUse)
            {
                try
                {
                    UG_Building buildSrc = obj.GetComponent<UG_Building>();
                    Rect pos = new Rect(50, 50 + (50 * yMod), 100, 50);
                    if (GUI.Button(pos, obj.name))
                    {
                        selectedTile = obj;
                    }
                    yMod++;
                } catch
                {
                    Debug.Log("Tile missing component");
                }
            }
        }
    }
}
