using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator sharedInstance;

    TileMasterClass[,] gridOfTiles;
    [SerializeField] GameObject tilePrefab;
    public Vector2 gridDimensions;
    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
        gridOfTiles = new TileMasterClass[(int)gridDimensions.x, (int)gridDimensions.y];
    }
    private void Start()
    {
        generateGrid();
    }
    private void generateGrid()
    {
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for(int y = 0; y < gridDimensions.y; y++)
            {
                Vector3 posToCreateTile = new Vector3(x, y, 0);
                GameObject mostRecentTile = (GameObject)Instantiate(tilePrefab, posToCreateTile, Quaternion.Euler(0,0,0));
                mostRecentTile.GetComponent<TileMasterClass>().setGridCoors(new Vector2(x, y));
                mostRecentTile.transform.SetParent(this.gameObject.transform);
                mostRecentTile.name = "Tile " + mostRecentTile.GetComponent<TileMasterClass>().getGridCoors().ToString();
                
                if(UnityEngine.Random.Range(1,10) > 8)
                {
                    mostRecentTile.GetComponent<TileMasterClass>().setTileWalkeable(false);
                    mostRecentTile.GetComponent<SpriteRenderer>().color = Color.cyan;
                }

                gridOfTiles[x, y] = mostRecentTile.GetComponent<TileMasterClass>();
            }
        }
    }

    internal TileMasterClass getTile(int x, int y)
    {
        return gridOfTiles[x,y];
    }

    public List<GameObject> getTiles(Vector2 starPos, Vector2 endPos)
    {
        Debug.Log("Getting T>iles");
        int lowesX, lowesY, highestX, highestY;
        List<GameObject> retVal = new List<GameObject>();
        if(starPos.x <= endPos.x)
        {
            lowesX = (int)starPos.x;
            highestX = (int)endPos.x;
        } else
        {
            lowesX = (int)endPos.x;
            highestX = (int)starPos.x;
        }

        if(starPos.y <= endPos.y)
        {
            lowesY = (int)starPos.y;
            highestY = (int)endPos.y;
        } else
        {
            lowesY = (int)endPos.y;
            highestY = (int)starPos.y;
        }

        for(int x = (int)lowesX; x <= (int)highestX; x++)
        {
            for(int y = (int)lowesY; y <= (int)highestY; y++)
            {
                retVal.Add(gridOfTiles[x, y].gameObject);
            }
        }

        return retVal;
    }

    internal IEnumerable<TileMasterClass> getTileNeighbors(TileMasterClass tile)
    {
        List<TileMasterClass> retVal = new List<TileMasterClass>();
        TileMasterClass t = tile;

        Vector2 pos = t.getGridCoors();
        if(pos.x == 0) 
        {
            if(pos.y == 0)
            {
                //bottom left
                retVal.Add(gridOfTiles[(int)pos.x + 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y + 1]);
            } else if(pos.y == gridDimensions.y - 1)
            {
                // top left
                retVal.Add(gridOfTiles[(int)pos.x + 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y - 1]);
            } else
            {
                retVal.Add(gridOfTiles[(int)pos.x + 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y + 1]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y - 1]);
            }
        } else if(pos.x == gridDimensions.x - 1)
        {
            if(pos.y == 0)
            {
                //bottom right
                retVal.Add(gridOfTiles[(int)pos.x - 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y + 1]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y - 1]);
            } else if(pos.y == gridDimensions.y - 1)
            {
                retVal.Add(gridOfTiles[(int)pos.x - 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y - 1]);
            } else
            {
                retVal.Add(gridOfTiles[(int)pos.x - 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y + 1]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y - 1]);

            }
        } else
        {
            // grid not on hor x
            if(pos.y == 0)
            {
                // bottom right
                retVal.Add(gridOfTiles[(int)pos.x - 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y + 1]);
                retVal.Add(gridOfTiles[(int)pos.x + 1, (int)pos.y]);
            } else if(pos.y == gridDimensions.y - 1)
            {
                retVal.Add(gridOfTiles[(int)pos.x - 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y - 1]);
                retVal.Add(gridOfTiles[(int)pos.x + 1, (int)pos.y]);
            } else
            {
                retVal.Add(gridOfTiles[(int)pos.x - 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y - 1]);
                retVal.Add(gridOfTiles[(int)pos.x + 1, (int)pos.y]);
                retVal.Add(gridOfTiles[(int)pos.x, (int)pos.y + 1]);
            }
        }

        return retVal;
    }
}
