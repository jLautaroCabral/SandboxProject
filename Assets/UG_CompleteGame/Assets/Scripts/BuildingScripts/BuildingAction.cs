using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAction : MonoBehaviour {
	//BUILDING FUNCTION NOTES
	//NEW FEATURES
	//added method to selection manager to automaticly switch between unit and building selection based on the object at the mouse cursor (If there isn't already anything selected), other two get switched to by worker actions
	//added a building action queue & a list of building actions we can perform to the building script + code to execute them
	//added method to resource manager to check if there are sufficient resources to do the building action
	//created example building actions (Create hoplite and worker)
	//created method to check for left click when selecting buildings (only one building at a time atm)
	//created method that draws buttons for actions + creates a new instance of the action on the buildign
	//created gui for displaying a buildings info and the actions queue

	//CHANGES
	//changed how the selection raycast by reference works
	//added check for if the object passed into the toSet single method has a tile script on it before trying to access it

	public GameObject prefabToSpawn;
	public int woodCost,stoneCost,foodCost,ironCost,goldCost;
	public bool callEachFrame = false;
	public float timer;
	public bool started = false;


	public virtual void doAction()//called while the action is being done, call each frame decides whether its called each frame or just once
	{

	}

	public virtual void startAction()
	{

	}
		

	public virtual void onComplete()//called when the action is done
	{

	}

	public virtual bool canWeDo()//called before to check if we can do the action
	{
		return false;
	}

	public virtual bool areWeDone() //is the action done
	{
		return false;
	}

	public virtual string getButtonText() //just what to display on the button to give an indication of what the action does
	{
		return "";
	}

	public virtual string getProgress() //give some idea of how close to completing the action you are
	{
		return "";
	}


}
