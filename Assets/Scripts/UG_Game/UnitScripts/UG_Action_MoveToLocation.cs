using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_Action_MoveToLocation : UG_Action
{
    Vector3 positionWeAreMovingTo;

    public override void initaliseLocation(Vector3 position)
    {
        positionWeAreMovingTo = position;
    }

    public override void doAction()
    {
        UG_UnitMovement um = this.GetComponent<UG_UnitMovement>();
        um.moveToLocation(positionWeAreMovingTo);
    }

    public override bool getIsActionComplete()
    {
        return Vector3.Distance(positionWeAreMovingTo, this.transform.position) < 2.0f;
    }
}
