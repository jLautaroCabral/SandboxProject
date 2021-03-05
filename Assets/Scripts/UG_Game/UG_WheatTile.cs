using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_WheatTile : UG_TileMasterClass
{
    private void Awake()
    {
        type = "Wheat";
    }
    public override int fCost
    {
        get
        {
            return (gCost + hCost) * 10;
        }
    }
}
