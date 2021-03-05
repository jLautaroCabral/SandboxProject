using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_RoadTile : UG_TileMasterClass
{
    private void Awake()
    {
        type = "Road";
    }
    public override int fCost
    {
        get
        {
            return (gCost + hCost) / 10;
        }
    }
}
