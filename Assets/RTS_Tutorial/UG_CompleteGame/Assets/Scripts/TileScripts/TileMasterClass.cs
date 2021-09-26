using UnityEngine;
using System.Collections;

public class TileMasterClass : MonoBehaviour {
	public float gridX,gridY;

	//pathfinding
	public bool walkable = true; //decides if the tile can be walked on
	protected int gCost;//cost for moving from the start tile to this tile
	protected int hCost;//an estimate of the distance between this tile and the tile you want to get a path to
	TileMasterClass parent; //used in the pathfinding to retrace steps and give the final path
	public string type;
	public bool hasResource = false;

	public Vector3 posInWorld;


	public PathfindNode myNode;


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

	public virtual void OnSelect()
	{
		//this.GetComponent<SpriteRenderer>().color=Color.red;
	}

	public virtual void OnDeselect()
	{
		//if (walkable == true) {
			//if (Input.GetKey (KeyCode.T)) {
				//this.GetComponent<SpriteRenderer> ().color = Color.yellow;
			//} else {
				//this.GetComponent<SpriteRenderer> ().color = Color.white;
			//}
		//} else {
			//this.GetComponent<SpriteRenderer> ().color = Color.white;
		//}
	}

	public bool isTileWalkable() //pathfinding
	{
		return walkable;
	}
		
	public void setTileWalkable(bool canWalk) //pathfinding
	{
		walkable = canWalk;
		if (myNode != null) {
			myNode.updateNode ();
		}
	}

	public TileMasterClass getParent()
	{
		return parent;
	}

	public void setParent(TileMasterClass val)
	{
		parent = val;
	}

	public Vector2 getGridCoords()
	{
		return new Vector2 (gridX, gridY);
	}

	public Vector3 getPosInWorld()
	{
		return posInWorld;
	}

	public void setGridCoords(Vector2 coords)
	{
		posInWorld = transform.position;
		gridX = coords.x;
		gridY = coords.y;
	}





	public virtual int fCost //made virtual to alter path finding
	{
		get{
			return gCost + hCost;//estimation of the total route to destination if this tile is used
		}
	}
}
