﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_WheatTile : UG_TileMasterClass
{
    public override int fCost
    {
        get
        {
            return (gCost + hCost) * 10;
        }
    }
}
