using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeValues : MonoBehaviour {
	//created this script
	//added references to get values in the respective unit & building action scripts
	//made UnitMasterClass Awake public so it can be called from child classes awake

	//added example upgrades
	//did some gui work

	//added static bool to upgrades to show that its been got
	//added if statement to get text in order to show researched upgrades


	//changes
	//moved the multi part action execute in the unit action queue to stop the argument out of range exceptions
	//implemented canWeDo bool into building actions & GUI
	public static UpgradeValues me;

	void Awake()
	{
		me = this;
	}

	//worker
	float workerBaseHealth = 5.0f;
	float workerHealthMultiplier = 1.0f;
	float workerBuildTimeModifier = 1.0f;

	public float getWorkerHealth()
	{
		return workerBaseHealth * workerHealthMultiplier;
	}

	public void increaseWorkerBaseHealth(float val)
	{
		workerBaseHealth += val;
	}

	public void increaseWorkerHealthMultiplier(float val)
	{
		workerHealthMultiplier += val;
	}

	public float getWorkerBuildTimeMod()
	{
		return workerBuildTimeModifier;
	}

	public void decreaseWorkerBuildTimeModifier(float val)
	{
		workerBuildTimeModifier -= val;
	}

	//hoplite
	float hopliteBaseHealth = 5.0f;
	float hopliteHealthMultiplier = 1.0f;
	float hopliteBuildTimeModifier = 1.0f;
	public float getHopliteHealth()
	{
		return hopliteBaseHealth * hopliteHealthMultiplier;
	}

	public void increaseHopliteBaseHealth(float val)
	{
		hopliteBaseHealth += val;
	}

	public void increaseHopliteHealthMultiplier(float val)
	{
		hopliteHealthMultiplier += val;
	}

	public float getHopliteBuildTimeMod()
	{
		return hopliteBuildTimeModifier;
	}

	public void decreaseHopliteBuildTimeModifier(float val)
	{
		hopliteBuildTimeModifier -= val;
	}

	//archer
	float archerBaseHealth = 5.0f;

	float archerHealthMultiplier = 1.0f;

	float archerBuildTimeModifier = 1.0f;
	public float getArcherHealth()
	{
		return archerBaseHealth * archerHealthMultiplier;
	}

	public void increaseArcherBaseHealth(float val)
	{
		archerBaseHealth += val;
	}

	public void increaseArcherHealthMultiplier(float val)
	{
		archerHealthMultiplier += val;
	}
		
	public float getArcherBuildTimeMod()
	{
		return archerBuildTimeModifier;
	}

	public void decreaseArcherBuildTimeModifier(float val)
	{
		archerBuildTimeModifier -= val;
	}


}
