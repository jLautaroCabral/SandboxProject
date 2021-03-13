using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class ThreadedPathfind {

	//THREADING
	//class adapted from code found at http://answers.unity3d.com/questions/357033/unity3d-and-c-coroutines-vs-threading.html

	//created this class
	//overload for getPath to work by reference
	//added pos in world vector 3 to tile master class + getter + it being set in the set grid coords
	//changed the convert to vector3 path method in pathfind to use the posInWorld rather than tile.gameobject.transform.position so it can be used in threading


	//REDUCING NUMBER OF NODES
	//Created node optimiser
	//created clone of pathfind to use the new nodes
	//added myNode to the TileMasterClass as a reference to the node its on
	//new path getting methods for nodes have an extra bit to add the end pos as the last element of the list
	//added code to make sure the start and end pos z coord is -2 then _getPath is called & in convert to vector path and where the path nodes are created


	//CURSOR COLLISION CHECK
	//added a cursor collision check script to make sure that you can't build a building ontop of units
	//added code to selection manager to use this so set the building place cursors script and take into account whether a unit is colliding with it when allowing to build
	//changed the method for getting the tiles for the building to use the cursors x & y coord rather than a raycast
	//added method to selection manager to check if adgacent nodes to ones being built on are walkable and added it to the check when creating a building


	//EDITS
	//Changed the unit movement in Action_moveToLocation to be a global variable + made it check for if the unit is moving before checking if the action was complete
	//added update node to the tile master class set walkable method
	//changed scout & patrol to check for looking for path and are we moving when checking for if the unit is moving

	private object myLock = new object();
	bool isDone=false;
	Vector3 posToGoTo,posFrom;
	Thread myThread;
	List<Vector3> finalPath;
	UnitMovement unitWantingPath;
	public void initialise(Vector3 to,Vector3 from, UnitMovement um)
	{
		posToGoTo = to;
		posFrom = from;
		unitWantingPath = um;
	}


	public bool IsDone
	{
		get
		{
			bool tmp;
			lock (myLock)
			{
				tmp = isDone;
			}
			return tmp;
		}
		set
		{
			lock (myLock)
			{
				isDone = value;
			}
		}
	}

	public void Start()
	{
		myThread = new Thread (Run);
		myThread.IsBackground = true;
		myThread.Start ();
	}

	public void Stop()
	{
		myThread.Abort ();
	}

	public void getPath()
	{
		finalPath = Pathfind.me.getPath (posToGoTo, posFrom);
	}

	public bool Update()
	{
		if (IsDone)
		{
			OnFinished();
			return true;
		}
		return false;
	}
		
	private void Run()
	{
		getPath();
		IsDone = true;
	}


	protected void OnFinished() { 
		finalPath.Reverse ();
		unitWantingPath.moveToLocation (finalPath);
	}
}

