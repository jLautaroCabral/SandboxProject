using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_Hoplite : UG_UnitMasterClass
{
    private void Update()
    {
        queueMoniter();
    }

    public override bool canWePerformAction(UG_Action ac)
    {
        if (ac.getActionType().Equals("Movement"))
            return true;
        else if (ac.getActionType().Equals("Default"))
            return false;
        else
            return false;
    }
}
