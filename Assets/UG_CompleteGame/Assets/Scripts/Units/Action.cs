using UnityEngine;
using System.Collections;

public class UGAction : MonoBehaviour {
	//Ep 3
	//added type virtual to allow checking of the types of action before they are added (if statement in unit order, virtual bool for getting if an action can be performed)
	//created individual types for the units

	public bool actionStarted = false;
	public bool multiPartAction = false;
	public virtual void initaliseLocation(Vector3 position)
	{
		
	}

	public virtual void initaliseTarget(GameObject target)
	{

	}

	public virtual void initialiseTile(TileMasterClass tm)
	{

	}


	public virtual void doAction()
	{

	}

	public virtual void doAction(GameObject me ,GameObject target)
	{

	}

	public virtual void doAction(GameObject me)
	{

	}

	public virtual void doAction(GameObject me, Vector3 position)
	{

	}

	public virtual void doAction(GameObject me, TileMasterClass tile)
	{

	}
		
	public virtual void onActionComplete()
	{

	}

	public virtual void reinitialiseAction()
	{

	}

	public virtual bool getIsActionComplete()
	{
		return false;
	}

	public virtual string getActionType()
	{
		return "Default";
	}
}
