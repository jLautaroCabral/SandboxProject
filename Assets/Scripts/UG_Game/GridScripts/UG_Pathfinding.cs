using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_Pathfinding : MonoBehaviour
{
    public static UG_Pathfinding sharedInstance;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
    }

    public List<Vector3> getPath(Vector3 starPos, Vector3 endPos)
    {
       // UG_GridGenerator.sharedInstance.ClearCostOfTiles();

        Vector2 sPos = new Vector2((int)starPos.x, (int)starPos.y);
        Vector2 ePos = new Vector2((int)endPos.x, (int)endPos.y);
        UG_TileMasterClass startNode = UG_GridGenerator.sharedInstance.getTile((int)sPos.x, (int)sPos.y);
        UG_TileMasterClass targetNode = UG_GridGenerator.sharedInstance.getTile((int)ePos.x, (int)ePos.y);

        List<UG_TileMasterClass> listOfTiles = new List<UG_TileMasterClass>();
        getPath(startNode, targetNode, ref listOfTiles);
        List<Vector3> retVal = convertToVectorPath(listOfTiles);
        return retVal;
    }

    private void getPath(UG_TileMasterClass startNode, UG_TileMasterClass targetNode, ref List<UG_TileMasterClass> store)
    {
        if(!targetNode.isTileWalkeable())
        {
            Debug.Log("One of tiles is not walkeable");
            return;
        }

        List<UG_TileMasterClass> openSet = new List<UG_TileMasterClass>();
        List<UG_TileMasterClass> closetSet = new List<UG_TileMasterClass>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            UG_TileMasterClass node = openSet[0];
            
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if(openSet[i].getH() < node.getH())
                    {  
                        node = openSet[i];
                    }
                }
            }

            openSet.Remove(node);
            closetSet.Add(node);

            if(node == targetNode)
            {
                Debug.Log("Finished path");
                RetracePath(startNode, targetNode, ref store);
                return;
            }

            foreach(UG_TileMasterClass neighbour in UG_GridGenerator.sharedInstance.getTileNeighbors(node))
            {
                if(!neighbour.isTileWalkeable() || closetSet.Contains(neighbour)/* || neighbour == null || node == null*/)
                    continue;

                int newCostToNeighbour = node.getG() + GetDistance(node, neighbour); 
                if(newCostToNeighbour < neighbour.getG() || !openSet.Contains(neighbour))
                {
                    neighbour.setG(newCostToNeighbour);
                    neighbour.setH(GetDistance(neighbour, targetNode));
                    neighbour.setParent(node);

                    if(!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }
    private List<Vector3> convertToVectorPath(List<UG_TileMasterClass> listOfTiles)
    {
        List<Vector3> retVal = new List<Vector3>();
        foreach(UG_TileMasterClass tile in listOfTiles)
        {
            retVal.Add(tile.gameObject.transform.position);
        }
        return retVal;
    }

    private void RetracePath(UG_TileMasterClass startNode, UG_TileMasterClass targetNode, ref List<UG_TileMasterClass> store)
    {
        List<UG_TileMasterClass> path = new List<UG_TileMasterClass>();
        UG_TileMasterClass curretNode = targetNode;
        while(curretNode != startNode)
        {
            path.Add(curretNode);
            curretNode = curretNode.getParent();
        }
        path.Reverse();
        store = path;
    }
    private int GetDistance(UG_TileMasterClass nodeA, UG_TileMasterClass nodeB)
    {
        int dstX = Mathf.Abs((int)nodeA.getGridCoors().x - (int)nodeB.getGridCoors().x);
        int dstY = Mathf.Abs((int)nodeA.getGridCoors().y - (int)nodeB.getGridCoors().y);

        if(dstX > dstY)
        {
            return (14 * dstY) + (10 * (dstX - dstY));
        }
        return (14 * dstX) + (10 * (dstY - dstX));
    }


}
