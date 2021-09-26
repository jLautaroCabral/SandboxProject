using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour {

    public UnityEngine.GameObject player;
    public bool yConstant;
    public float xoff;
    public float yoff;

    private void FixedUpdate()
    {
        Vector3 pos = player.transform.position;
        if (!yConstant) gameObject.transform.position = new Vector3(0, pos.y - yoff, -10);
        if (yConstant) gameObject.transform.position = new Vector3(pos.x + xoff, yoff, -10);
    }
}
