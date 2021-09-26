using UnityEngine;
using System.Collections;

public class RoadTile : TileMasterClass {

	void Awake()
	{
		type="Road";
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override int fCost //new for pathfinding
	{
		get{
			return (gCost + hCost) / 10;//estimation of the total route to destination if this tile is used
		}
	}
}
