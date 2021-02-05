using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding sharedInstance;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3> getPath(Vector3 starPos, Vector3 endPos)
    {
        List<TileMasterClass> listOfTiles = new List<TileMasterClass>();
        getPath(starPos, endPos, ref listOfTiles);
        List<Vector3> retVal = convertToVectorPath(listOfTiles);
        return retVal;
    }
    private void getPath(Vector3 starPos, Vector3 endPos, ref List<TileMasterClass> store)
    {
        Vector2 sPos = new Vector2((int)starPos.x, (int)starPos.y);
        Vector2 ePos = new Vector2((int)endPos.x, (int)endPos.y);

        TileMasterClass startNode = GridGenerator.sharedInstance.getTile((int)sPos.x, (int)sPos.y);
        TileMasterClass targetNode = GridGenerator.sharedInstance.getTile((int)ePos.x, (int)ePos.y);

        if(!targetNode.isTileWalkeable())
        {
            Debug.Log("One of tiles is not walkeable");
            return;
        }

        List<TileMasterClass> openSet = new List<TileMasterClass>();
        List<TileMasterClass> closetSet = new List<TileMasterClass>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            TileMasterClass node = openSet[0];
            for(int i = 0; i < openSet.Count; i++)
            {
                if(openSet[i].fCost > node.fCost || openSet[i].fCost == node.fCost)
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

            foreach(TileMasterClass neighbour in GridGenerator.sharedInstance.getTileNeighbors(node))
            {
                if(!neighbour.isTileWalkeable() || closetSet.Contains(neighbour) || neighbour == null || node == null)
                {
                    continue;
                }

                int newCostToNeighbour = node.getG() + GetDistance(node, neighbour); 
                if(newCostToNeighbour < neighbour.getG() || !openSet.Contains(neighbour))
                {
                    neighbour.setG(newCostToNeighbour);
                    neighbour.setH(GetDistance(neighbour, targetNode));
                    neighbour.setParent(node);

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }
    private List<Vector3> convertToVectorPath(List<TileMasterClass> listOfTiles)
    {
        List<Vector3> retVal = new List<Vector3>();
        foreach(TileMasterClass tile in listOfTiles)
        {
            retVal.Add(tile.gameObject.transform.position);
        }
        return retVal;
    }
    private void RetracePath(TileMasterClass startNode, TileMasterClass targetNode, ref List<TileMasterClass> store)
    {
        List<TileMasterClass> path = new List<TileMasterClass>();
        TileMasterClass curretNode = targetNode;
        while(curretNode != startNode)
        {
            path.Add(curretNode);
            curretNode = curretNode.getParent();
        }
        path.Reverse();
        store = path;
    }
    private int GetDistance(TileMasterClass nodeA, TileMasterClass nodeB)
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
