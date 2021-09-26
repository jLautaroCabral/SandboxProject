using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainTile :TileMasterClass {



	void Awake()
	{
		type="Mountain";
		setTileWalkable (false);
	}



	public override int fCost //new for pathfinding
	{
		get{
			return (gCost + hCost) * 10;//estimation of the total route to destination if this tile is used
		}
	}
}
