using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnitManager : MonoBehaviour {
	public List<GameObject> units;
	public static UnitManager me;
	void Awake()
	{
		me = this;
		//units = new List<GameObject> ();
	}
	public void addUnit(GameObject g)
	{
		units.Add (g);
	}

	public void removeUnit(GameObject g)
	{
		units.Remove (g);
	}

	public List<GameObject> getUnitsWithinArea(Vector3 startPos, Vector3 endPos)
	{
		List<GameObject> retVal = new List<GameObject> ();
		float xLow, yLow, xHigh, yHigh;

		if (startPos.x < endPos.x) {
			xLow = startPos.x;
			xHigh = endPos.x;
		} else {
			xLow = endPos.x;
			xHigh = startPos.x;
		}

		if (startPos.y < endPos.y) {
			yLow = startPos.y;
			yHigh = endPos.y;
		} else {
			yLow = endPos.y;
			yHigh = startPos.y;
		}
		foreach (GameObject g in units) {
			Vector3 pos = g.transform.position;

			if (pos.x > xLow && pos.x < xHigh && pos.y > yLow && pos.y < yHigh) {
				retVal.Add (g);
			}

		}

		return retVal;
	}
}
