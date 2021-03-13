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
                //Vector3 mousePos = Input.mousePosition;
                //Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(mousePos);
                Vector2 mousePosRay = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

                string objectTag = "";
                RaycastHit2D raycast = Physics2D.Raycast(mousePosRay, Vector2.zero, 0f);

                try
                {
                    GameObject hitObj = raycast.collider.gameObject;
                    objectTag = hitObj.tag;
                    Debug.Log(objectTag);
                } catch
                {
                    Debug.Log("Selection error");
                }

                if(objectTag == "Resource")
                {
                    Debug.Log("Hit a resource");
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(mousePos);
                    UG_TileMasterClass tm = UG_GridGenerator.sharedInstance.getTile((int)mouseInWorld.x, (int)mouseInWorld.y);
                    if(tm != null)
                    {
                        foreach(GameObject obj in UG_SelectionManager.sharedInstance.getSelected())
                        {
                            if(obj.GetComponent<UG_UnitMasterClass>() != null)
                            {

                                Debug.Log("Añadiendo action");
                                UG_UnitMasterClass um = obj.GetComponent<UG_Worker>();
                                UG_Action action = obj.AddComponent<UG_Action_GhaterResource>();

                                if(um.canWePerformAction(action) == true)
                                {
                                    action.initaliseLocation(mouseInWorld);
                                    action.enabled = false;
                                    um.actionsQueue.Add(action);
                                } else
                                {
                                    Destroy(action);
                                    //moveUnitToLocation(mouseInWorld, obj);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Did not hit a resource");
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(mousePos);
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
            UG_Action action = obj.gameObject.AddComponent<UG_Action_MoveToLocation>();
            if(um.canWePerformAction(action))
            {
                Debug.Log("Mouse: " + mouseInWorld);
                action.initaliseLocation(mouseInWorld);
                action.enabled = false;
                um.actionsQueue.Add(action);
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
