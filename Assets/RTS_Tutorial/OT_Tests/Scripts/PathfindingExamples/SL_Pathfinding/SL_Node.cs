using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SL_Node : UG_IHeapItem<SL_Node>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public SL_Node parent;

    int heapIndex;

    public SL_Node(bool _walkeable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkeable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(SL_Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if(compare == 0)
            compare = hCost.CompareTo(nodeToCompare.hCost);
        
        return -compare;
    }
}
 