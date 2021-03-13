using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeOptimiser : MonoBehaviour {
	public static NodeOptimiser me;
	public GameObject nodeVisaliser;
	int nodeSavings=0;
	public int savingsDisplay=0;
	void Awake()
	{
		me = this;
	}

	public void optimiseNodes()
	{
		TileMasterClass[,] tiles = GridGenerator.me.getTiles ();

		for (int x = 0; x < (int)GridGenerator.me.gridDimensions.x ; x = x + 2) {
			for (int y = 0; y < (int)GridGenerator.me.gridDimensions.y ; y = y + 2) {
				
				TileMasterClass tm1, tm2, tm3, tm4;
				tm1 = tiles [x, y];
				tm2 = tiles [x + 1, y];
				tm3 = tiles [x, y + 1];
				tm4 = tiles [x + 1, y + 1];

				string type1 = tm2.type;
				bool allSameType = true;
				List<TileMasterClass> tileList = new List<TileMasterClass> ();
				tileList.Add (tm1);
				tileList.Add (tm2);
				tileList.Add (tm3);
				tileList.Add (tm4);

				foreach (TileMasterClass tm in tileList) {
					if (tm.type.Equals (type1) == false) {
						allSameType = false;
					}
				}

				if (allSameType == false) {
					GameObject g1 = (GameObject)Instantiate (nodeVisaliser, new Vector3 (tm1.transform.position.x, tm1.transform.position.y, -2), Quaternion.Euler (0, 0, 0)); //get gameobject of node
					GameObject g2 = (GameObject)Instantiate (nodeVisaliser, new Vector3 (tm2.transform.position.x, tm2.transform.position.y, -2), Quaternion.Euler (0, 0, 0));
					GameObject g3 = (GameObject)Instantiate (nodeVisaliser, new Vector3 (tm3.transform.position.x, tm3.transform.position.y, -2), Quaternion.Euler (0, 0, 0));
					GameObject g4 = (GameObject)Instantiate (nodeVisaliser, new Vector3 (tm4.transform.position.x, tm4.transform.position.y, -2), Quaternion.Euler (0, 0, 0));


					PathfindNode p1 = g1.GetComponent<PathfindNode> (); //gets pathfinding node script
					PathfindNode p2 = g2.GetComponent<PathfindNode> ();
					PathfindNode p3 = g3.GetComponent<PathfindNode> ();
					PathfindNode p4 = g4.GetComponent<PathfindNode> ();

					p1.setPosInWorld (tm1.gameObject.transform.position); //sets the nodes position in the world
					p2.setPosInWorld (tm2.gameObject.transform.position);
					p3.setPosInWorld (tm3.gameObject.transform.position);
					p4.setPosInWorld (tm4.gameObject.transform.position);

					p1.setNodeWalkable (tm1.isTileWalkable ());
					p2.setNodeWalkable (tm2.isTileWalkable ());
					p3.setNodeWalkable (tm3.isTileWalkable ());
					p4.setNodeWalkable (tm4.isTileWalkable ());

					p1.myTiles.Add (tm1);//adds the tile master class t the pathfind nodes neighbor tiles,  used to work out which tiles are neighbors
					p2.myTiles.Add (tm2);
					p3.myTiles.Add (tm3);
					p4.myTiles.Add (tm4);

					Pathfind.me.listOfNodes.Add (p1); //adds the nodes to the list of nodes in the pathfind script, used to get paths
					Pathfind.me.listOfNodes.Add (p2);
					Pathfind.me.listOfNodes.Add (p3);
					Pathfind.me.listOfNodes.Add (p4);

					tm1.myNode = p1;
					tm2.myNode = p2;
					tm3.myNode = p3;
					tm4.myNode = p4;


				} else {
					nodeSavings++;
					savingsDisplay = nodeSavings * 3;
					Vector3 tilePos = tm2.getPosInWorld ();
					Vector3 pos = new Vector3 (tilePos.x -= 0.5f, tilePos.y += 0.5f, -2);
					GameObject g = (GameObject)Instantiate (nodeVisaliser, pos, Quaternion.Euler (0, 0, 0));
					g.transform.localScale = new Vector3 (2, 2, 1);//make it a 2*2 grid

					PathfindNode p = g.GetComponent<PathfindNode> ();
					p.myTiles.Add (tm1);
					p.myTiles.Add (tm2);
					p.myTiles.Add (tm3);
					p.myTiles.Add (tm4);
					Pathfind.me.listOfNodes.Add (p);

					p.setPosInWorld (pos);
					p.setNodeWalkable (tm1.isTileWalkable());

					tm1.myNode = p;
					tm2.myNode = p;
					tm3.myNode = p;
					tm4.myNode = p;

				}



			}
		}
			

		for (int x = 0; x < (int)GridGenerator.me.gridDimensions.x ; x++) {
			for (int y = 0; y < (int)GridGenerator.me.gridDimensions.y ; y++) {
				TileMasterClass tm = tiles [x, y];
			 //should create links between nodes
				List<TileMasterClass> neighbors = GridGenerator.me.getTileNeighbors (tm);

				foreach (TileMasterClass tm2 in neighbors) {
					if (tm2.myNode.neighbors.Contains (tm.myNode) == false && tm.myNode.neighbors.Contains(tm2.myNode)==false) {
						if (tm2.myNode != tm.myNode) {
							tm2.myNode.neighbors.Add (tm.myNode);
							tm.myNode.neighbors.Add (tm2.myNode);
						}
					}
				}
			}
		}
	}
}
