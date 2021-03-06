using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UG_Action : MonoBehaviour
{
    // Start is called before the first frame update
    public bool actionStarted = false;

    public virtual void initaliseLocation(Vector3 location) { }
    public virtual void initaliseTarget(GameObject target) { }
    public virtual void initaliseTile(UG_TileMasterClass tm) { }
    public virtual void doAction() { }
    public virtual void doAction(GameObject me, GameObject target) { }
    public virtual void doAction(GameObject me) { }
    public virtual void doAction(GameObject me, Vector3 position) { }
    public virtual void doAction(GameObject me, UG_TileMasterClass tile) { }
    public virtual void onActionComplete() { }
    public virtual bool getIsActionComplete() { return false; }

}
