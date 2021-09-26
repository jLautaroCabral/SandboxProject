using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollCheck : MonoBehaviour {

    private PlatformMovement platform;

    private void Awake()
    {
        platform = FindObjectOfType<PlatformMovement>();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            platform.move = true;
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            platform.move = false;
        }
    }
}
