using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_UnitOrderScript : MonoBehaviour
{
    void Update()
    {
        commandUnitsToMove();    
    }

    private void commandUnitsToMove()
    {
        if(areAnyUnitsSelected())
        {
            if(Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(mousePos);
                mouseInWorld.z = 0;

                foreach(GameObject obj in UG_SelectionManager.sharedInstance.getSelected()) 
                {
                    if(obj.GetComponent<UG_UnitMasterClass>() != null)
                    {
                        UG_UnitMasterClass um = obj.GetComponent<UG_UnitMasterClass>();
                        UG_Action a = obj.AddComponent<UG_Action_MoveToLocation>();
                        a.initaliseLocation(mouseInWorld);
                        um.actionsQueue.Add(a);
                    }
                }
            }
        }
    }

    private bool areAnyUnitsSelected()
    {
       return UG_SelectionManager.sharedInstance.selectionMode == SelectionMode.unit
              && UG_SelectionManager.sharedInstance.getSelected().Count > 0;
    }
}
