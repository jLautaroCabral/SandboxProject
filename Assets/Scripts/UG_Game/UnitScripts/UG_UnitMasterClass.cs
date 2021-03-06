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

    private void queueMoniter()
    {
        if(actionsQueue.Count() > 0)
        {
            if (actionsQueue[0].actionStarted == false)
            {
                actionsQueue[0].doAction();
                actionsQueue[0].actionStarted = true;
            } else
            {
                if(actionsQueue[0].getIsActionComplete())
                {
                    removeCurrentAction();
                }
            }
        } 
    }

    internal void removeCurrentAction()
    {
        UG_Action a = actionsQueue[0];
        actionsQueue.Remove(a);
        Destroy(a);
    }
}
