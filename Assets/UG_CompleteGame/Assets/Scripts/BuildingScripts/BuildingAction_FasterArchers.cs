using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAction_FasterArchers : BuildingAction {
	public static bool gotUpgrade = false;
	public override void doAction ()
	{
		if (timer >= 0) {
			timer -= Time.deltaTime;
		}
	}

	public override void startAction ()
	{
		ResourceManager.me.ReduceResources ("food", foodCost);
		started = true;
	}

	public override void onComplete ()
	{
		UpgradeValues.me.decreaseArcherBuildTimeModifier (0.1f);
	}

	public override bool areWeDone ()
	{
		if (timer <= 0) {
			gotUpgrade = true;
			return true;
		} else {
			return false;
		}
	}

	public override bool canWeDo ()
	{
		return ResourceManager.me.canWeDoBuildingAction (this)&& gotUpgrade==false;
	}

	public override string getButtonText ()
	{
		if (gotUpgrade == false) {
			
			if (canWeDo () == true) {
				return "Improve Archer Build Speed : " + foodCost + " Food";
			} else {
				return "Insufficient resources to improve archer build speed " + foodCost + " Food";
			}
		} else {
			return"Improve Archer Build Speed Researched";
		}
	}

	public override string getProgress ()
	{
		return "Improve Archer Build Speed : " + timer;
	}
}