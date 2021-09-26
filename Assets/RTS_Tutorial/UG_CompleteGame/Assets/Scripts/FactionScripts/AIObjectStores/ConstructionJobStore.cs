using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionJobStore {
	public List<GameObject> workersToBuild;

	//Since building aren't implemented for AI we'll just use random values temporarily
	public Building toBuild;

	//didnt end up needing the instance of the building when it was built here

	//testing values
	public int width = 0;
	public int height = 0;

	public Vector3 locationInWorld;

	public ConstructionJobStore(Vector3 locToBuild, Building toBuild,List<GameObject> workers)
	{
		locationInWorld = locToBuild;
		this.toBuild = toBuild;
		width = toBuild.widthInTiles;
		height = toBuild.heightInTiles;
		workersToBuild = workers;
	}
}
