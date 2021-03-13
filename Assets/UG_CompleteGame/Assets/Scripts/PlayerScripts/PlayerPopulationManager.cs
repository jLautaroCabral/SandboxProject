using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopulationManager : MonoBehaviour {
	public static PlayerPopulationManager me;
	public List<GameObject> myHouses;
	public int populationLimit;
	public int currentPoplualtion;

	float timer = 1.0f;
	void Awake()
	{
		me = this;
		myHouses = new List<GameObject> ();
	}

	public void addHouse(GameObject house)
	{
		myHouses.Add (house);
		calculateMaxPopulation ();
	}

	public void removeHouse(GameObject house)
	{
		myHouses.Remove (house);
		calculateMaxPopulation ();
	}

	void calculateMaxPopulation()
	{
		int popMax = 0;

		foreach (GameObject g in myHouses) { 
			Building b = g.GetComponent<Building> ();
			if (b.built == true) {
				popMax += 5;
			}
		}
		populationLimit = popMax;
	}

	void calculateCurrentPopulation()
	{
		int popCount = 0;
		popCount += UnitManager.me.units.Count;
		currentPoplualtion = popCount;
	}

	public bool canWeConstructUnit()
	{
		return populationLimit >= currentPoplualtion;
	}

	void Update(){
		moniterPopulation ();
	}

	void moniterPopulation()
	{
		timer -= Time.deltaTime;
		if (timer <= 0) {
			calculateMaxPopulation ();
			calculateCurrentPopulation ();
			timer = 1.0f;
		}
	}

	float originalWidth = 1920.0f; //scaleing will make the gui proportional to what you see onscreen no matter the resolution, will cause stretching
	float originalHeight = 1080.0f;
	Vector3 scale;
	float dispWidth = 1920.0f / 6;
	void OnGUI()
	{
		GUI.depth = 0;
		scale.x = Screen.width/originalWidth;
		scale.y = Screen.height/originalHeight;
		scale.z =1;
		var svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);

		Rect pos = new Rect (0, 100, dispWidth, 100);
		GUI.Box (pos, "Population: " + currentPoplualtion + "/" + populationLimit);

		GUI.matrix = svMat;
	}
}
