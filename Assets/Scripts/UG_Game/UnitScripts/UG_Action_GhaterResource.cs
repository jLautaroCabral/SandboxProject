using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_Action_GhaterResource : UG_Action
{
    public Vector3 positionOfResource;
    public float resourceTimer = 5.0f;
    public Vector3 positionOfStoreHouse;

    public bool movingToResource = false;
    public bool gatheredResource = false;
    public bool movingToStorehouse = false;
    public bool storeResource = false;
    public bool loop = true;

    public bool atRes = false;
    public bool atStore = false;

    public override void initaliseLocation(Vector3 location)
    {
        multiPartAction = true;
        positionOfResource = location;
        positionOfStoreHouse = GameObject.FindGameObjectWithTag("Building").transform.position;
    }

    public override void doAction()
    {
        atRes = atResource();
        atStore = atStorehouse();

        if (!atResource() && !movingToResource)
            moveToResource();
        if (atResource() && !gatheredResource)
            gatherResource();
        if (!asStorehouse() && gatheredResource)
            moveToStoreHouse();
        if (atStoreHouse() && gatheredResource)
            storeResource();
    }

    private void moveToResource()
    {
        if(!movingToResource)
        {
            UG_UnitMovement um = this.GetComponent<UG_UnitMovement>();
            um.moveToLocation(positionOfResource);
            movingToResource = true;
        }
    }

    bool atResource()
    {
        Vector2 myPos = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 targetPos = new Vector2(positionOfResource.x, positionOfResource.y);

        if (Vector2.Distance(myPos, targetPos) < 2.0f)
        {
            movingToResource = false;
            return true;
        }
        return false;
    }

    bool aStoreHouse()
    {
        Vector2 myPos = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 targetPos = new Vector2(positionOfStoreHouse.x, positionOfStoreHouse.y);

        if(Vector2.Distance(myPos, targetPos) < 2.0f)
        {
            movingToStorehouse = false;
            return true;
        }
        return false;
    }

    void gatherResource()
    {
        resourceTimer -= Time.deltaTime;
        if(resourceTimer <= 0)
        {
            gatheredResource = true;
            resourceTimer = 5.0f;
        }
    }
}
