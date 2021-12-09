using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTo : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        this.transform.position = target.position;
    }
}
