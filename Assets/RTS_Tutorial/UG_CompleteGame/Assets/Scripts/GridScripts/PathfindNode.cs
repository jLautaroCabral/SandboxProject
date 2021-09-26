using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindNode : MonoBehaviour {

	bool walkable = true; //decides if the tile can be walked on
	protected int gCost;//cost for moving from the start tile to this tile
	protected int hCost;//an estimate of the distance between this tile and the tile you want to get a path to
	PathfindNode parent; //used in the pathfinding to retrace steps and give the final path

	Vector3 posInWorld;

	public List<TileMasterClass> myTiles = new List<TileMasterClass>();
	public List<PathfindNode> neighbors = new List<PathfindNode>();

	public void setG(int val)
	{
		gCost = val;
	}

	public void setH(int val)
	{
		hCost = val;
	}

	public int getH() //made g and h protected so that they can be accessed by inheriting classes
	{
		return hCost;
	}

	public int getG()
	{
		return gCost;
	}


	public bool isNodeWalkable() //pathfinding
	{
		return walkable;
	}

	public void setNodeWalkable(bool canWalk) //pathfinding
	{
		if (canWalk == false) {
			this.GetComponent<SpriteRenderer> ().color = Color.red;	
		}
		walkable = canWalk;
	}

	public PathfindNode getParent()
	{
		return parent;
	}

	public void setParent(PathfindNode val)
	{
		parent = val;
	}

	public void setPosInWorld(Vector3 pos)
	{
		posInWorld = pos;
	}

	public Vector3 getPosInWorld()
	{
		return posInWorld;
	}

	public virtual int fCost //made virtual to alter path finding
	{
		get{
			return gCost + hCost;//estimation of the total route to destination if this tile is used
		}
	}

	public void updateNode()
	{
		int walkable = 0, unwalkable = 0;

		foreach (TileMasterClass tm in myTiles) {
			if (tm.walkable == true) {
				walkable++;
			} else {
				unwalkable++;
			}
		}

		if (walkable > unwalkable) {
			setNodeWalkable (true);
		} else {
			setNodeWalkable (false);
		}
	}

	public bool isLocationValidForBuilding() // go through all neighboring tiles and check for unwalkable ones, to make sure that areas arent cut off by accident
	{
		foreach (PathfindNode n in neighbors) {
			if (n.walkable == false) {
				return false;
			}
		}

		return true;
	}
}
