using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> units = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal bool IsMyUnit(GameObject unitToSearch)
    {
        foreach(GameObject unit in units)
        {
            if(unit == unitToSearch)
            {
                return true;
            }
        }

        return false;
    }
}
