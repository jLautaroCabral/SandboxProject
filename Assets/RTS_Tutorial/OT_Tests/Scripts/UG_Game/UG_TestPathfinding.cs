﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UG_TestPathfinding : MonoBehaviour
{
    public List<Vector3> path;
    public int counter = 0;

    private void Awake()
    {
        path = new List<Vector3>();
    }

    void Update()
    {
        getRandomPath();

        if(shouldWeMovedAlongPath())
        {
            moveAlognPath();
        }
    }

    private void getRandomPath()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            resetCount();
            Debug.Log("Testing pathfinding");
            float x1 = Random.Range(0, UG_GridGenerator.sharedInstance.gridDimensions.x);
            float x2 = Random.Range(0, UG_GridGenerator.sharedInstance.gridDimensions.x);
            float y1 = Random.Range(0, UG_GridGenerator.sharedInstance.gridDimensions.y);
            float y2 = Random.Range(0, UG_GridGenerator.sharedInstance.gridDimensions.y);
            path = UG_Pathfinding.sharedInstance.getPath(this.transform.position, new Vector3((int)x2, (int)y2, 0));
        }
    }
    private bool shouldWeMovedAlongPath()
    {
        return (path.Count > 0 && Vector3.Distance(this.transform.position, path[path.Count - 1]) > 0.5f && counter < path.Count - 1);
    }

    private void resetCount()
    {
        counter = 0;
    }

    private void moveAlognPath()
    {
        if(Vector3.Distance(this.transform.position, path[counter]) > 0.05f)
        {
            Vector3 dir = path[counter] - transform.position;
            transform.Translate(dir * 5 * Time.deltaTime);
        } else
        {
            if(counter < path.Count - 1)
            {
                counter++;
            } 
        }
    }
}
