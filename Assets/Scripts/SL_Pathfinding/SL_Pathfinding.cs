using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class SL_Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    SL_Grid grid;

    private void Awake()
    {
        grid = GetComponent<SL_Grid>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        SL_Node startNode = grid.NodeFromWorldPoint(startPos);
        SL_Node targetNode = grid.NodeFromWorldPoint(targetPos);

        SL_Heap<SL_Node> openSet = new SL_Heap<SL_Node>(grid.MaxSize);
        HashSet<SL_Node> closedSet = new HashSet<SL_Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            SL_Node currentNode = openSet.RemoveFirst();


            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + " ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(SL_Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(SL_Node startNode, SL_Node endNode)
    {
        List<SL_Node> path = new List<SL_Node>();
        SL_Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(SL_Node nodeA, SL_Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
