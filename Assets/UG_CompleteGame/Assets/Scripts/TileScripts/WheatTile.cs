using UnityEngine;
using System.Collections;

public class WheatTile : TileMasterClass{


	void Awake()
	{
		type="Wheat";
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnSelect()
	{
		//give some kind of options for wheat

	}

	public override int fCost //new for pathfinding
	{
		get{
			return (gCost + hCost) * 10;//estimation of the total route to destination if this tile is used
		}
	}
}
