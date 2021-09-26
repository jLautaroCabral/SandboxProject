using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyColl : MonoBehaviour {

    HealthSystem heath;

    private void Awake()
    {
        heath = GetComponentInParent<HealthSystem>();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "playerArrow" || coll.gameObject.tag == "enemyArrow")
        {
            heath.OnBodyDamage();
        }
    }
}
