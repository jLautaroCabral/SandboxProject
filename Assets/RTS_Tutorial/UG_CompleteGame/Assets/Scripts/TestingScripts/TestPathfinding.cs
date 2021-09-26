using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TestPathfinding : MonoBehaviour {
	public List<Vector3> path;
	int counter = 0;
	// Use this for initialization
	void Awake()
	{
		path = new List<Vector3> ();
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		getRandomPath ();

		if (shouldWeMoveAlongPath () == true) {
			moveAlongPath ();
		}
	}

	void getRandomPath()
	{
		
		if (Input.GetKeyDown (KeyCode.R)) {
			resetCount ();
			Debug.Log ("Testing Pathfinding");
			//while (tilePath.Count == 0) {
				float x1 = Random.Range (0, GridGenerator.me.gridDimensions.x);
				float x2 = Random.Range (0, GridGenerator.me.gridDimensions.x);
				float y1 = Random.Range (0, GridGenerator.me.gridDimensions.y);
				float y2 = Random.Range (0, GridGenerator.me.gridDimensions.y);
				Debug.Log (x1 + " " + y1 + " | " + x2 + " " + y2);
			path = Pathfind.me.getPath(this.transform.position, new Vector3 ((int)x2, (int)y2, 0));
			//Pathfind.me.getPath (new Vector3 ((int)x1, (int)y1, 0), new Vector3 ((int)x2, (int)y2, 0), ref tilePath);
			//Pathfind.me.getPath (new Vector3 (2, 2, 0), new Vector3 (10, 10, 0), ref tilePath);
			//}
		}
	}

	bool shouldWeMoveAlongPath()
	{
		if (path.Count > 0 && Vector3.Distance (this.transform.position, path [path.Count - 1]) > 0.5f && counter < path.Count - 1) {
			return true;
		} else {
			return false;
		}

	}

	void resetCount()
	{
		counter = 0;
	}

	void moveAlongPath()
	{
		if (Vector3.Distance (this.transform.position, path [counter]) > 0.5f) {
			Vector3 dir = path [counter] - transform.position;
			transform.Translate (dir * 5 * Time.deltaTime);
		} else {
			if (counter < path.Count - 1) {
				counter++;
			}
		}
	}
			
}
