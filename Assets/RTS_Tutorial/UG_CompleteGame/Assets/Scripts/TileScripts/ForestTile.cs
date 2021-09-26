using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTile :TileMasterClass {
	public GameObject tree;
	void Awake()
	{
		type="Forest";

		int r = Random.Range (0, 100);
		if (r <= 5) {
			GameObject g = (GameObject) Instantiate (tree, this.transform.position, this.transform.rotation);
			g.transform.parent = this.gameObject.transform;
			ResourceStore.me.wood.Add (g);
			hasResource = true;
		}
	}



	public override int fCost //new for pathfinding
	{
		get{
			return (gCost + hCost) * 10;//estimation of the total route to destination if this tile is used
		}
	}
}
