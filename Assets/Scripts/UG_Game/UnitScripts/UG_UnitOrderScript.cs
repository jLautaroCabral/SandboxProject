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
                Vector2 mousePosRay = new Vector2(mouseInWorld.x, mouseInWorld.x);

                string objectTag = "";
                RaycastHit2D raycast = Physics2D.Raycast(mousePosRay, Vector3.zero, 0f);

                try
                {
                    GameObject hitObj = raycast.collider.gameObject;
                    objectTag = hitObj.tag;
                } catch
                {
                    Debug.Log("Selection error");
                }

                if(objectTag == "Resource")
                {
                    Debug.Log("Hit a resource");
                    UG_TileMasterClass tm = UG_GridGenerator.sharedInstance.getTile((int)mouseInWorld.x, (int)mouseInWorld.y);
                    if(tm != null)
                    {
                        foreach(GameObject obj in UG_SelectionManager.sharedInstance.getSelected())
                        {
                            if(obj.GetComponent<UG_UnitMasterClass>() != null)
                            {
                                UG_UnitMasterClass um = obj.GetComponent<UG_UnitMasterClass>();
                                UG_Action action = obj.AddComponent<UG_Action_GhaterResource>();

                                if(um.canWePerformAction(action) == true)
                                {
                                    action.initaliseLocation(mouseInWorld);
                                    um.actionsQueue.Add(action);
                                    action.enabled = false;
                                } else
                                {
                                    Destroy(action);
                                    moveUnitToLocation(mouseInWorld, obj);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Did not hit a resource");
                    mouseInWorld.z = 0;
                    UG_TileMasterClass tm = UG_GridGenerator.sharedInstance.getTile((int)mouseInWorld.x, (int)mouseInWorld.y);

                    if(tm != null)
                    {
                        foreach(GameObject obj in UG_SelectionManager.sharedInstance.getSelected())
                        {
                            moveUnitToLocation(mouseInWorld, obj);
                        }
                    }
                }
            }
        }
    }

    private void moveUnitToLocation(Vector3 mouseInWorld, GameObject obj)
    {
        if(obj.GetComponent<UG_UnitMasterClass>() != null)
        {
            UG_UnitMasterClass um = obj.GetComponent<UG_UnitMasterClass>();
            UG_Action action = gameObject.AddComponent<UG_Action_MoveToLocation>();
            if(um.canWePerformAction(action))
            {
                action.initaliseLocation(mouseInWorld);
                um.actionsQueue.Add(action);
                action.enabled = false;
            } else
            {
                Destroy(action);
            }
        }
    }

    private bool areAnyUnitsSelected()
    {
       return UG_SelectionManager.sharedInstance.selectionMode == SelectionMode.unit
              && UG_SelectionManager.sharedInstance.getSelected().Count > 0;
    }
}
