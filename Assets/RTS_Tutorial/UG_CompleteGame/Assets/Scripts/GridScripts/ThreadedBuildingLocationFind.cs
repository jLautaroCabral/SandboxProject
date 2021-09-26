using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class ThreadedBuildingLocationFind{

	private object myLock = new object();
	bool isDone=false;
	Thread myThread;
	AIBuildingStore myBuildingStore;
	int x,y,width,height;

	List<TileMasterClass> returnList;
	public void initialise(AIBuildingStore myBuild,Vector3 startPos, int width, int height)
	{
		myBuildingStore = myBuild;
		x = (int)startPos.x;
		y = (int)startPos.y;
		this.width = width;
		this.height = height;
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

	public void getLocation()
	{
		returnList = myBuildingStore.placeBuilding (x, y, width, height);
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
		getLocation ();
		IsDone = true;
	}


	protected void OnFinished() { 
		myBuildingStore.tilesFromThread = returnList;
	}
}
