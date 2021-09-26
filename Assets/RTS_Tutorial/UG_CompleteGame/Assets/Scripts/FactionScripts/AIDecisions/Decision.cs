using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decision : MonoBehaviour {

	public int priority = 0;
	public FactionIdentifier myFaction;

	public void setFaction(FactionIdentifier faction)
	{
		myFaction = faction;
	}

	public virtual bool canWePerformAction()
	{
		return false;
	}

	public virtual void doAction()
	{

	}

	public virtual void onCantPerformAction()
	{

	}

	public virtual string endResult()
	{
		return "";
	}
}
