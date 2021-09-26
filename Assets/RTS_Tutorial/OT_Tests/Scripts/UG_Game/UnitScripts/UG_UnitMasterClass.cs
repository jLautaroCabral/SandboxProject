using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UG_UnitMasterClass : MonoBehaviour
{
    public List<UG_Action> actionsQueue;

    private void Awake()
    {
        actionsQueue = new List<UG_Action>();
    }
    void Update()
    {
        queueMoniter();
    }

    protected void queueMoniter()
    {
        if(actionsQueue.Count() > 0)
        {
            if (actionsQueue[0].actionStarted == false)
            {
                actionsQueue[0].enabled = true;
                actionsQueue[0].actionStarted = true;
                actionsQueue[0].doAction();
            } else
            {
                if(actionsQueue[0].getIsActionComplete())
                    removeCurrentAction();
            }

            if (actionsQueue[0].multiPartAction == true)
                actionsQueue[0].doAction();
        } 
    }

    internal void removeCurrentAction()
    {
        UG_Action a = actionsQueue[0];
        actionsQueue.Remove(a);
        Destroy(a);
    }

    public virtual bool canWePerformAction(UG_Action ac)
    {
        if (ac.getActionType().Equals("Movement"))
            return true;
        else if (ac.getActionType().Equals("Default"))
            return false;
        else
            return false;
    }
}
