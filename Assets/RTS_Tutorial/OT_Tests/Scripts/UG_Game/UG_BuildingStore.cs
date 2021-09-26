using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_BuildingStore : MonoBehaviour
{
    public static UG_BuildingStore sharedInstance;
    [SerializeField] private GameObject selectedBuilding;
    public GameObject[] buildings;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    public GameObject getToBuild()
    {
        return selectedBuilding;
    }

    private void OnGUI()
    {
        if(UG_SelectionManager.sharedInstance.selectionMode == SelectionMode.creatingBuilding)
        {
            int yMod = 0;

            foreach(GameObject obj in buildings)
            {
                try
                {
                    UG_Building buildScript = obj.GetComponent<UG_Building>();
                    Rect pos = new Rect(50, 50 + (50 * yMod), 100, 50);
                    if(GUI.Button(pos, buildScript.name))
                    {
                        selectedBuilding = obj;
                    }
                    yMod++;
                } catch
                {
                    Debug.LogError("Build is missing a component");
                }
            }
        }
    }
}
