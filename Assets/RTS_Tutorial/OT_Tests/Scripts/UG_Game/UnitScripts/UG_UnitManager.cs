using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_UnitManager : MonoBehaviour
{
    public List<GameObject> units;
    public static UG_UnitManager sharedInstance;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
    }

    public void addUnit(GameObject obj)
    {
        units.Add(obj);
    }

    public void removeUnit(GameObject obj)
    {
        units.Remove(obj);
    }

    internal List<GameObject> getUnitsWithinArea(Vector3 startPos, Vector3 endPos)
    {
        List<GameObject> retVal = new List<GameObject>();
        float xLow, yLow, xHigh, yHigh;

        if(startPos.x < endPos.x)
        {
            xLow = startPos.x;
            xHigh = endPos.x;
        } else
        {
            xLow = endPos.x;
            xHigh = startPos.x;
        }

        if(startPos.y < endPos.y)
        {
            yLow = startPos.y;
            yHigh = endPos.y;
        } else
        {
            yLow = endPos.y;
            yHigh = startPos.y;
        }

        foreach(GameObject obj in units)
        {
            Vector3 pos = obj.transform.position;

            if(pos.x > xLow && pos.x < xHigh && pos.y > yLow && pos.y < yHigh)
            {
                retVal.Add(obj);
            }
        }

        return retVal;
    }
}
