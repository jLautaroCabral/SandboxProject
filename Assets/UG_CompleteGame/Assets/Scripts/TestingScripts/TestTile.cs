using UnityEngine;
using System.Collections;

public class TestTile : TileMasterClass{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnSelect()
	{
		Debug.Log ("Tile Test Class " + this.gameObject.transform.position);
	}
}
