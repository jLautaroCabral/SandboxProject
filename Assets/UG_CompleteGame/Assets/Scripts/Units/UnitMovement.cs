using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnitMovement : MonoBehaviour {
	//just a basic class to move the unit along a path provided by the pathfinding class
	public bool areWeMoving = false;
	public bool waitingForPath = false;
	public List<Vector3> curPath;
	int pathCounter = 0;
	ThreadedPathfind tp;
	public Vector3 finalLocation;
	float abandonThreadTimer = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (waitingForPath == true) {
			//Debug.LogError ("Waiting For Path");
			if (tp == null) {
				return;
			}else{
				if (tp.Update ()) {
				//	Debug.LogError ("Got Path");
					tp = null;
				} else {
					abandonThreadTimer += Time.deltaTime;

					if (abandonThreadTimer >= 5.0f) {
						tp.Stop ();
						tp = null;
						waitingForPath = false;
						areWeMoving = true;
					}
				}
			}
			return;
		}

		if (areWeMoving == true) {
			moveAlongPath ();
		}



	}

	public Vector3 getFinalPosition()
	{
		return curPath [curPath.Count - 1];
	}

    public void moveToLocation(Vector3 positionTo)
	{
		abandonThreadTimer = 0.0f;
		finalLocation = positionTo;
//		Debug.LogError ("Moving to location");
		//new version using threading
		tp = new ThreadedPathfind ();
		//Debug.LogError ("Created Threaded Pathfind");
		//Debug.LogError ("Creating threaded pathfind to go from " + this.transform.position + " to " + positionTo);
		tp.initialise (positionTo, this.transform.position, this);
		tp.Start ();
		//Debug.LogError ("Started Thread");
		waitingForPath = true;

	}

	public void moveToLocation(List<Vector3> path) //will need to add some kind of checking to make sure that the path is valid from the current location of the unit
	{
		pathCounter = 0;
		curPath = path;
		areWeMoving = true;
		waitingForPath = false;

	}

	void moveAlongPath()
	{
		Vector2 myPos = new Vector2 (this.transform.position.x, this.transform.position.y);
		Vector2 tarPos = new Vector2 (0,0);


		try{
			tarPos = new Vector2 (curPath [pathCounter].x,curPath [pathCounter].y);
		}
		catch{
			tarPos = new Vector2 (finalLocation.x,finalLocation.y);
		}

		if (curPath.Count <= 1 || curPath.Count>1000) {

			if (curPath.Count > 1000) {
				Debug.LogError ("Excessivly long path generated, check over A* code at some point");
				curPath.Clear ();
			}

			Vector3 dir = finalLocation - this.transform.position;
			transform.Translate (dir.normalized * 5 * Time.deltaTime);

			if (Vector3.Distance (myPos, finalLocation) < 0.25f) {
				areWeMoving = false;
			}
			//areWeMoving = false;//need some way to get rid of errant action
			//this.GetComponent<UnitMasterClass>().removeCurrentAction();
		} else {

			if (Vector2.Distance (myPos, tarPos) > 0.5f) {
				Vector3 dir = curPath [pathCounter] - transform.position;
				dir.z = 0;
				transform.Translate (dir.normalized * 5 * Time.deltaTime);
			} else {
				if (pathCounter < curPath.Count - 1) {
					pathCounter++;
				} else {
					pathCounter = 0;//New for economy
					areWeMoving = false;
				}
			}
		}
	}

	
}
