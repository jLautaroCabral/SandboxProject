using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Farm : MonoBehaviour {

	Building thisBuilding;
	public GameObject wheat;
	void Awake(){
		thisBuilding = this.gameObject.GetComponent<Building> ();
	}

	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeInHierarchy == true) {
			isBuildingBuilt ();
		}
	}

	void isBuildingBuilt()
	{
		if (thisBuilding.built == true) {
			Vector2 start = new Vector2 ((int)this.transform.position.x + 6, (int)this.transform.position.y + 6);
			Vector2 end = new Vector2 ((int)this.transform.position.x - 6, (int)this.transform.position.y - 6);

			if (start.x > GridGenerator.me.gridDimensions.x ) {
				start.x = GridGenerator.me.gridDimensions.x - 1;
			}

			if(start.x < 0)
			{
				start.x = 0;
			}

			if (start.y > GridGenerator.me.gridDimensions.y ) {
				start.y = GridGenerator.me.gridDimensions.y - 1;
			}

			if(start.y < 0)
			{
				start.y = 0;
			}
			//end
			if (end.x > GridGenerator.me.gridDimensions.x ) {
				end.x = GridGenerator.me.gridDimensions.x - 1;
			}

			if(end.x < 0)
			{
				end.x = 0;
			}

			if (end.y > GridGenerator.me.gridDimensions.y ) {
				end.y = GridGenerator.me.gridDimensions.y - 1;
			}

			if(end.y < 0)
			{
				end.y = 0;
			}

			List<GameObject> tm = GridGenerator.me.getTiles (start,end);

			foreach (GameObject tl in tm) {


				TileMasterClass t = tl.GetComponent<TileMasterClass> ();


				if (t.type.Equals ("Rockey") == false && t.type.Equals ("Mountain") == false && t.type.Equals ("Water") == false && t.walkable==true) {
					GridGenerator.me.ChangeTilesInGrid (tl, wheat);
				}
			}

			Destroy (this);
		}


	}
}
