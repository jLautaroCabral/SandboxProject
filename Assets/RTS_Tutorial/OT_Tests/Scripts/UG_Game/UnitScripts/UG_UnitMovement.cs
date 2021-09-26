using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_UnitMovement : MonoBehaviour
{
    public bool areWeMoving = false;
    public List<Vector3> curPath;
    int pathCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(areWeMoving)
        {
            moveAlongPath();
        }
    }

    internal void moveToLocation(Vector3 positionTo)
    {
        pathCounter = 0;
        curPath = UG_Pathfinding.sharedInstance.getPath(this.transform.position, positionTo);
        areWeMoving = true;

        if(curPath.Count == 0)
        {
            areWeMoving = false;
            this.GetComponent<UG_UnitMasterClass>().removeCurrentAction();
        }
    }

    void moveAlongPath()
    {
        Vector2 myPos = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 tarPos = new Vector2(curPath[pathCounter].x, curPath[pathCounter].y);

        if(Vector3.Distance(myPos, tarPos) > 0.1f)
        {
            Vector3 dir = curPath[pathCounter] - transform.position;
            dir.z = 0;
            transform.Translate(dir * 5 * Time.deltaTime);
        } else
        {
            if(pathCounter < curPath.Count - 1)
                pathCounter++;
            else
                areWeMoving = false;
        }
    }
}
