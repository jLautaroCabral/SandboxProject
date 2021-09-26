using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Pathfind : MonoBehaviour {
	

	public static Pathfind me;
	void Awake()
	{
		me = this;
		listOfNodes = new List<PathfindNode> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void getPath(Vector3 startPos, Vector3 endPos,ref List<Vector3> store) //passes by reference instead of returning a value
	{
		List<TileMasterClass> listOfTiles = new List<TileMasterClass> ();
		getPath (startPos, endPos, ref listOfTiles);
		List<Vector3> retVal = convertToVectorPath (listOfTiles);
		store = retVal;
	}

	public List<TileMasterClass> getPathAsTiles(Vector3 startPos, Vector3 endPos) //ADDED TO CREATE PROCEDURAL TOWNS, NOT NEEDED FOR TUTORIAL
	{
		List<TileMasterClass> listOfTiles = new List<TileMasterClass> ();
		getPath (startPos, endPos, ref listOfTiles);
		return listOfTiles;
	}

	public List<Vector3> getPath(Vector3 startPos, Vector3 endPos)
	{
		List<TileMasterClass> listOfTiles = new List<TileMasterClass> ();
		getPath (startPos, endPos, ref listOfTiles);
		List<Vector3> retVal = convertToVectorPath (listOfTiles);
		return retVal;
	}

	void getPath(Vector3 startPos, Vector3 endPos,ref List<TileMasterClass> store)
	{
		Vector2 sPos = new Vector2 ((int)startPos.x, (int)startPos.y);
		Vector2 ePos = new Vector2 ((int)endPos.x, (int)endPos.y);//converting the positions to ints in order to get the rough tile coords from them

		TileMasterClass startNode = GridGenerator.me.getTile ((int)sPos.x, (int)sPos.y); //gets the two tiles as TileMasterClass
		TileMasterClass targetNode = GridGenerator.me.getTile ((int)ePos.x, (int)ePos.y);

		if (startNode==null ||targetNode==null || targetNode.isTileWalkable () == false  ) {
			Debug.Log ("One of the tiles is not walkable");
			return;
		}

		List<TileMasterClass> openSet = new List<TileMasterClass>(); //open set is tiles we want to check
		List<TileMasterClass> closedSet = new List<TileMasterClass>();//closed set is tiles we have checked and don't want to inclide in the path
		openSet.Add(startNode);

		while (openSet.Count > 0) { //cycle through the open set
			TileMasterClass node = openSet[0];

			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) { //check to see if we can find a tile with lower or equal f cost(approximation of distance to target from start including this tile) than current tile
					if (openSet[i].getH() < node.getH())//if the tile has a lower distance to the target tile than the current one in 'node'
						node = openSet[i];//sets node to be this closer tile
				}
			}

			openSet.Remove(node);//takes node from the open set and puts it in the closed set
			closedSet.Add(node);

			if (node == targetNode) {//a check to see if the node is the tile we want to go to, if so we return the path after is has  been retraced
//				Debug.LogError ("Finished Path " + startNode.name + " " + targetNode.name);
				RetracePath(startNode,targetNode,ref store);
				return;
			}

			foreach (TileMasterClass neighbour in GridGenerator.me.getTileNeighbors(node)) { //goes through each of the neighbors of the node

				if (!neighbour.isTileWalkable()  || closedSet.Contains(neighbour) || neighbour==null || node==null) {//if the neighbor is not accessable or the node is null go onto next neighbor
					continue;
				}

				int newCostToNeighbour = node.getG () + GetDistance(node, neighbour);//calculates the cost of going to the neighbor from the start os the path
				if (newCostToNeighbour < neighbour.getG() || !openSet.Contains(neighbour)) {//if the cost is shorter and the  open set doesnt contain the neighbor
					
					neighbour.setG(newCostToNeighbour);
					neighbour.setH(GetDistance(neighbour, targetNode));//set the g and h values of the neighbor
					neighbour.setParent(node);//sets the parent of the neighbor to be the node signifying that in the path you'll go from the node to the neighbor
					if (!openSet.Contains(neighbour))//adds the neighbor to the open set if not already there
						openSet.Add(neighbour);
				}
			}
		}
	}

	List<Vector3> convertToVectorPath(List<TileMasterClass> tiles)//converts the path found to a list of vector3 as the objects moving don't need all the tile info
	{
		List<Vector3> retVal = new List<Vector3> ();
		foreach (TileMasterClass tile in tiles) {
			//retVal.Add (tile.gameObject.transform.position);
			retVal.Add(tile.getPosInWorld());
		}
		return retVal;
	}

	void RetracePath(TileMasterClass startNode,TileMasterClass targetNode,ref List<TileMasterClass> store) //goes through the path via the parent variable and puts it in a list 
	{
		List<TileMasterClass> path = new List<TileMasterClass>();
		TileMasterClass currentNode = targetNode;

		while (currentNode != startNode) {
			//Debug.Log ("Retracing path " + currentNode.gameObject.name);
			path.Add(currentNode);
			//if (highlightPathFound == true) {
			//currentNode.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			//}
			currentNode = currentNode.getParent();
		}
		path.Reverse();//have to reverse it cause it traces the path from finish to start using the parent variable
		store = path;
	}

	int GetDistance(TileMasterClass nodeA,TileMasterClass nodeB)//gets the distance between two grid coords and returns them multiplied
	{

		int dstX = Mathf.Abs((int)nodeA.getGridCoords().x - (int)nodeB.getGridCoords().x);
		int dstY = Mathf.Abs((int)nodeA.getGridCoords().y - (int)nodeB.getGridCoords().y);

		if (dstX > dstY)//to make sure that the final number is positive
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}

	//START OF PATHFINDNODE CONVERSION
	/*
	 * 
	 * 
	 * */

	public List<PathfindNode> listOfNodes;

	public void _getPath(Vector3 startPos, Vector3 endPos,ref List<Vector3> store) //passes by reference instead of returning a value
	{
		List<PathfindNode> listOfTiles = new List<PathfindNode> ();
		startPos.z = -2;
		endPos.z = -2;
		_getPath (startPos, endPos, ref listOfTiles);
		List<Vector3> retVal = convertToVectorPath (listOfTiles);
		store = retVal;
		store.Add (endPos);
	}

	public List<Vector3> _getPath(Vector3 startPos, Vector3 endPos)
	{
//		Debug.Log ("Called get path, goind from " + startPos + " to " + endPos );
		startPos.z = -2;
		endPos.z = -2;
		List<PathfindNode> listOfTiles = new List<PathfindNode> ();
		_getPath (startPos, endPos, ref listOfTiles);

		List<Vector3> retVal = convertToVectorPath (listOfTiles);
		retVal.Add (endPos);
		return retVal;
	}

	void _getPath(Vector3 startPos, Vector3 endPos,ref List<PathfindNode> store)
	{
		
		Vector2 sPos = new Vector2 ((int)startPos.x, (int)startPos.y);
		Vector2 ePos = new Vector2 ((int)endPos.x, (int)endPos.y);//converting the positions to ints in order to get the rough tile coords from them

		PathfindNode startNode = getNodeByLocation (startPos); //gets the two tiles as PathfindNode
		PathfindNode targetNode = getNodeByLocation(endPos);
		//PathfindNode previousNode = startNode; //NEW FOR ECONOMY

//		Debug.Log (startNode.getPosInWorld() +" || " + targetNode.getPosInWorld());

		if (startNode==null ||targetNode==null || targetNode.isNodeWalkable() == false  ) {
			Debug.Log ("One of the tiles is not walkable");
			return;
		}

		List<PathfindNode> openSet = new List<PathfindNode>(); //open set is tiles we want to check
		List<PathfindNode> closedSet = new List<PathfindNode>();//closed set is tiles we have checked and don't want to inclide in the path
		openSet.Add(startNode);

		while (openSet.Count > 0) { //cycle through the open set
			PathfindNode node = openSet[0];

			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) { //check to see if we can find a tile with lower or equal f cost(approximation of distance to target from start including this tile) than current tile
					if (openSet[i].getH() < node.getH())//if the tile has a lower distance to the target tile than the current one in 'node'
						node = openSet[i];//sets node to be this closer tile
				}
			}

			openSet.Remove(node);//takes node from the open set and puts it in the closed set
			closedSet.Add(node);

			if (node == targetNode) {//a check to see if the node is the tile we want to go to, if so we return the path after is has  been retraced
				//				Debug.LogError ("Finished Path " + startNode.name + " " + targetNode.name);
				RetracePath(startNode,targetNode,ref store);
				return;
			}

			foreach (PathfindNode neighbour in node.neighbors) { //goes through each of the neighbors of the node
				//|| neighbour == previousNode
				if (!neighbour.isNodeWalkable() || closedSet.Contains(neighbour) || neighbour==null || node==null) {//if the neighbor is not accessable or the node is null go onto next neighbor
					continue;
				}

				int newCostToNeighbour = node.getG () + GetDistance(node, neighbour);//calculates the cost of going to the neighbor from the start os the path
				if (newCostToNeighbour < neighbour.getG() || !openSet.Contains(neighbour)) {//if the cost is shorter and the  open set doesnt contain the neighbor

					neighbour.setG(newCostToNeighbour);
					neighbour.setH(GetDistance(neighbour, targetNode));//set the g and h values of the neighbor
					neighbour.setParent(node);//sets the parent of the neighbor to be the node signifying that in the path you'll go from the node to the neighbor
					//previousNode = node; //NEW FOR ECONOMY

					if (!openSet.Contains(neighbour))//adds the neighbor to the open set if not already there
						openSet.Add(neighbour);
				}
			}
		}
	}

	List<Vector3> convertToVectorPath(List<PathfindNode> tiles)//converts the path found to a list of vector3 as the objects moving don't need all the tile info
	{
		List<Vector3> retVal = new List<Vector3> ();
		foreach (PathfindNode tile in tiles) {
			Vector3 pos = tile.getPosInWorld ();
			pos.z = -2;

			retVal.Add(tile.getPosInWorld());
		}
		return retVal;
	}

	void RetracePath(PathfindNode startNode,PathfindNode targetNode,ref List<PathfindNode> store) //goes through the path via the parent variable and puts it in a list 
	{
		List<PathfindNode> path = new List<PathfindNode>();
		PathfindNode currentNode = targetNode;

		while (currentNode != startNode) {
			//Debug.Log ("Retracing path " + currentNode.gameObject.name);
			path.Add (currentNode);
			//if (highlightPathFound == true) {
			//currentNode.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			//}
			currentNode = currentNode.getParent ();
		
		}
		path.Add (startNode);
		path.Reverse();//have to reverse it cause it traces the path from finish to start using the parent variable
		store = path;
	}

	public PathfindNode getNodeByLocation(Vector3 pos)
	{
		TileMasterClass tm = GridGenerator.me.getTile ((int)pos.x, (int)pos.y);
		if (tm.walkable == true) {
			return tm.myNode;
		} else {
			foreach (PathfindNode node in tm.myNode.neighbors) {
				if (node.isNodeWalkable () == true) {
					return node;
				}
			}
		}
		return tm.myNode; //just a default case, returning an unwalkable node causes the path to be abandoned
	}

	int GetDistance(PathfindNode nodeA,PathfindNode nodeB)//gets the distance between two grid coords and returns them multiplied
	{

		int dstX = Mathf.Abs((int)nodeA.getPosInWorld().x - (int)nodeB.getPosInWorld().x);
		int dstY = Mathf.Abs((int)nodeA.getPosInWorld().y - (int)nodeB.getPosInWorld().y);

		if (dstX > dstY)//to make sure that the final number is positive
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}
