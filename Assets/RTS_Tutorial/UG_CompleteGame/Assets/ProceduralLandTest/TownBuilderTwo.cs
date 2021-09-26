using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBuilderTwo : MonoBehaviour {
	public GameObject paved;
	public GameObject farm,house,temple,storehouse,barracks,wall,palisade;
	public int xBorder=2, yBorder=2;
	public List<GameObject> buildingsForTown;
	TownBuildingPlacement tb;
	// Use this for initialization
	void Start () {
		tb = this.GetComponent<TownBuildingPlacement> ();
		buildingsForTown = new List<GameObject> ();
		for (int x = 0; x < 10; x++) {
			buildingsForTown.Add (house);
		}
		buildingsForTown.Add (temple);
		buildingsForTown.Add (barracks);
		buildingsForTown.Add (storehouse);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			roguelikeTownDraw ();
		}
	}

	void drawRandomLine()
	{
		int xLow, xHigh, yLow, yHigh;
		xLow = Random.Range (0, (int)GridGenerator.me.gridDimensions.x / 2);
		xHigh = Random.Range ((int)GridGenerator.me.gridDimensions.x / 2, (int)GridGenerator.me.gridDimensions.x);
		yLow = Random.Range (0, (int)GridGenerator.me.gridDimensions.y / 2);
		yHigh = Random.Range ((int)GridGenerator.me.gridDimensions.y / 2, (int)GridGenerator.me.gridDimensions.y);
		drawLineBetweenTwoPoints (xLow, yLow,xHigh , yHigh);
	}

	void connectFourPoints()
	{
		drawLineBetweenTwoPoints (10, 10, 25, 25);
		drawLineBetweenTwoPoints (25, 25, 50, 50);
		drawLineBetweenTwoPoints (25, 50,50,50);
		drawLineBetweenTwoPoints (10, 10,25,50);
	}

	void drawLineBetweenTwoPoints(int x1,int y1,int x2,int y2)
	{
		

		int xIncrease = x2-x1;
		int yIncrease = y2-y1;

		int yPerX = yIncrease / xIncrease;
		float error1 = Mathf.Abs (yIncrease / xIncrease);
		float error2 = error1 - 0.5f;
		int y = y1;

		for (int x = x1; x < x2; x++) {
			GridGenerator.me.ChangeTilesInGrid (GridGenerator.me.getTile (x,y).gameObject, GridGenerator.me.forest);
			error1 += error2;

			if (error1 > 0.5f) {
				y++;
				error1 -= 1.0f;
			}

		}

				//GridGenerator.me.ChangeTilesInGrid (GridGenerator.me.getTile (x, y).gameObject, GridGenerator.me.forest);


	}
	public int x,y;

	void roguelikeTownDraw()
	{
		 x = (int)GridGenerator.me.gridDimensions.x/2;
		 y = (int)GridGenerator.me.gridDimensions.y / 2;

		Vector2 dir = new Vector2 (0, -1);
		int changeDir = 5;
		int makePlaza = 5;

		for(int c = 0;c<1000;c++)
		{
			dir = dirCheck (new Vector2(x,y),dir);
			int r = Random.Range (0, 100);

			if (changeDir > r) {
				dir = getRandomDirection ();
				changeDir = 0;
			}

			try{
				drawRoad (x,y);
			}
			catch{
				x = (int)GridGenerator.me.gridDimensions.x / 2;
				y = (int)GridGenerator.me.gridDimensions.y / 2;
			}

			x += (int)dir.x;
			y += (int)dir.y;

			changeDir++;
		}

		placeBuildings ();
	}

	void placeBuildings()
	{
		for (int x = 0; x < 20; x++) {
			GameObject toCreate = buildingsForTown [Random.Range (0, buildingsForTown.Count)];
			tb.addJobToConstructionQueue (new Vector3(GridGenerator.me.gridDimensions.x,GridGenerator.me.gridDimensions.y),toCreate.GetComponent<Building>());
		}
	}

	Vector2 dirCheck(Vector2 cur,Vector2 dir)
	{
		Vector2 retval = dir;

		if(cur.x >= GridGenerator.me.gridDimensions.x -xBorder/2)
		{
			retval.x = -1;
		}
		else if(cur.x <=xBorder/2)
		{
			retval.x = 1;
		}

		if(cur.y >= GridGenerator.me.gridDimensions.y -yBorder/2)
		{
			retval.y = -1;
		}
		else if(cur.x <=yBorder/2)
		{
			retval.y = 1;
		}

		return retval;
	}

	Vector2 getRandomDirection()
	{
		Vector2 retval = Vector2.zero;

		int r = Random.Range (0, 100);
		if (r < 50) {
			int r2 = Random.Range (0, 100);
			if (r2 < 50) {
				retval = new Vector2 (-1, 0);
			} else {
				retval = new Vector2 (1, 0);
			}
		} else {
			int r2 = Random.Range (0, 100);
			if (r2 < 50) {
				retval = new Vector2 (0, -1);
			} else {
				retval = new Vector2 (0, 1);
			}
		}
		return retval;
	}

	void drawRoad(int x,int y)
	{
		GridGenerator.me.ChangeTilesInGrid (GridGenerator.me.getTile (x, y).gameObject,paved);
		GridGenerator.me.ChangeTilesInGrid (GridGenerator.me.getTile (x+1, y).gameObject,paved);
		GridGenerator.me.ChangeTilesInGrid (GridGenerator.me.getTile (x, y+1).gameObject,paved);
		GridGenerator.me.ChangeTilesInGrid (GridGenerator.me.getTile (x+1, y+1).gameObject,paved);

	}
}
