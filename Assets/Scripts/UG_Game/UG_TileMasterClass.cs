using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_TileMasterClass : MonoBehaviour
{
    public float gridX, gridY;

    // Pathfinding
    bool walkeable = true;
    protected int gCost; // cost for moving from the start tile to this tile
    protected int hCost; // an estimate of the distance between this tile and the tile you want to get a path to
    UG_TileMasterClass parent; // used in the pathfinding to retrace steps and give the final path

    public virtual int fCost
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
        //this.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public virtual void OnDeselect()
    {
        /*
        if(walkeable)
        {
            if(Input.GetKey(KeyCode.T))
            {
                this.GetComponent<SpriteRenderer>().color = Color.yellow;
            } else
            {
                this.GetComponent<SpriteRenderer>().color = new Color(0.3043479f, 1, 0);
            }
        } else
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0.641f, 0.406f, 0.220f);
        }
        */
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

    public UG_TileMasterClass getParent()
    {
        return parent;
    }

    public void setParent(UG_TileMasterClass val)
    {
        parent = val;
    }
}
