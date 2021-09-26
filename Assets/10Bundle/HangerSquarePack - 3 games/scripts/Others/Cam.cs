using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    public GameObject player;
    public bool yConstant, xConstant;
    public float xoff;
    public float yoff;

    private void FixedUpdate()
    {
        Vector3 pos = player.transform.position;
        if (!yConstant && !xConstant) gameObject.transform.position = new Vector3(pos.x + xoff, pos.y - yoff, -10);
        if (yConstant && !xConstant) gameObject.transform.position = new Vector3(pos.x + xoff, 0f, -10);
        if (!yConstant && xConstant) gameObject.transform.position = new Vector3(0f, pos.y - yoff, -10);
        if (yConstant && xConstant) gameObject.transform.position = new Vector3(0f, 0f, -10);
    }
}
