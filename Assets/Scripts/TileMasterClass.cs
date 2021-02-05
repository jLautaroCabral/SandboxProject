using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMasterClass : MonoBehaviour
{
    float gridX, gridY;

    // Pathfinding
    bool walkeable = true;
    int gCost; // cost for moving from the start tile to this tile
    int hCost; // an estimate of the distance between this tile and the tile you want to get a path to
    TileMasterClass parent; // used in the pathfinding to retrace steps and give the final path

    public int fCost
    {
        get { return gCost + hCost; } // estimation of the total quote to destination if this tile is used
    }
    public void setG(int val)
    {
        gCost = val;
    }
    public int getG()
    {
        return gCost;
    }
    public void setH(int val)
    {
        hCost = val;
    }
    public int getH()
    {
        return hCost;
    }
    public virtual void OnSelect()
    {
        Debug.Log("Tile Master Class " + this.gameObject.transform.position);
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public virtual void OnDeselect()
    {
        Debug.Log("Tile Master Class " + this.gameObject.transform.position);
        this.GetComponent<SpriteRenderer>().color = Color.green;
    }
    internal void setGridCoors(Vector2 coors)
    {
        gridX = coors.x;
        gridY = coors.y;
    }
    internal Vector2 getGridCoors()
    {
        return new Vector2(gridX, gridY);
    }

    internal bool isTileWalkeable()
    {
        return walkeable;
    }

    internal void setTileWalkeable(bool canWalk)
    {
        walkeable = canWalk;
    }

    public TileMasterClass getParent()
    {
        return parent;
    }

    public void setParent(TileMasterClass val)
    {
        parent = val;
    }
}
