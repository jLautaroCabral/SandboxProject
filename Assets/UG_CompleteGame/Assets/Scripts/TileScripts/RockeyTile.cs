using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockeyTile :TileMasterClass {
	public GameObject[] iron,stone,gold;

	void Awake()
	{
		type="Rockey";

		int r = Random.Range (0, 100);
		if (r <= 5) {
			int r2 = Random.Range (0, 100);

			if (r2 < 50) {
				int r3 = Random.Range (0, stone.Length);
				GameObject g = (GameObject) Instantiate (stone [r3], this.transform.position, this.transform.rotation);
				g.transform.parent = this.gameObject.transform;
				ResourceStore.me.stone.Add (g);
			} else if (r2 < 90) {
				int r3 = Random.Range (0, iron.Length);
				GameObject g = (GameObject) Instantiate (iron [r3], this.transform.position, this.transform.rotation);
				g.transform.parent = this.gameObject.transform;
				ResourceStore.me.iron.Add (g);
			} else {
				int r3 = Random.Range (0, gold.Length);
				GameObject g = (GameObject) Instantiate (gold [r3], this.transform.position, this.transform.rotation);
				g.transform.parent = this.gameObject.transform;
				ResourceStore.me.gold.Add (g);
			}
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
