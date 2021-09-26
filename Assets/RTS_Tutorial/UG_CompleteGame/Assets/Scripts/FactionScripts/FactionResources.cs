using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionResources : MonoBehaviour {
	public int wood=0,food=0,stone=0,iron=0,gold=0;

	public void ReduceResources(string resource,int amount)
	{
		alterResourceVal (resource, amount * -1);
	}

	public void IncreaseResources(string resource,int amount)
	{
		alterResourceVal (resource, amount);
	}

	public bool canWeDoBuildingAction(BuildingAction ba)
	{
		return (resourceAmountCheck ("wood", ba.woodCost)
			&& resourceAmountCheck ("food", ba.foodCost)
			&& resourceAmountCheck ("stone", ba.stoneCost)
			&& resourceAmountCheck ("iron", ba.ironCost)
			&& resourceAmountCheck ("gold", ba.goldCost));
	}

	public bool canWeConstructBuilding(Building toCheck)
	{ //single and means that all have to return true for it to return true
		return (resourceAmountCheck ("wood", toCheck.woodCost)
			&& resourceAmountCheck ("food", toCheck.foodCost)
			&& resourceAmountCheck ("stone", toCheck.stoneCost)
			&& resourceAmountCheck ("iron", toCheck.ironCost)
			&& resourceAmountCheck ("gold", toCheck.ironCost));
	}

	public bool resourceAmountCheck(string resource,int amount)//just a check for if we have enough resources of a type, should be called before allowing for construction and recruitment
	{
		switch (resource) {
		case "food":
			if (food >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		case "wood":
			if (wood >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		case "stone":
			if (stone >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		case "iron":
			if (iron >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		case "gold":
			if (gold >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		default:
			break;
		}

		return false;//if something goes wrong and an invalid resource is passed in
	}

	void alterResourceVal(string resource,int amount) //have one method to alter the value, just have to multiply decreases by -1 when passing it through
	{
		switch (resource) {
		case "food":
			food += amount;
			break;
		case "wood":
			wood += amount;
			break;
		case "stone":
			stone += amount;
			break;
		case "iron":
			iron += amount;
			break;
		case "gold":
			gold+=amount;
			break;
		default:
			break;
		}
	}

}
