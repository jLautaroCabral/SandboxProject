using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// On this script, the arms of the enemy ragdoll will be visible only after he die
/// </summary>

public class RagdollArmsLogic : MonoBehaviour {

    private HealthSystem health;
    private SpriteRenderer armSprite;

    private void Awake()
    {
        health = GetComponentInParent<HealthSystem>();
        armSprite = GetComponent<SpriteRenderer>();

        armSprite.enabled = false;
    }

    private void Update()
    {
        MakeArmAppear();
    }

    void MakeArmAppear()
    {
        //if the enemy die his arm will be visible 
        if (health.died)
        {
            armSprite.enabled = true;
        }
    }
}
