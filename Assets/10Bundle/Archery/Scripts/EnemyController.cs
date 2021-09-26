using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private GameObject bowArm;

    private HealthSystem health;

    private void Awake()
    {
        health = GetComponentInParent<HealthSystem>();
    }

    private void Update()
    {
        SetOffBowArm();
    }

    void SetOffBowArm()
    {
        if (health.died)
        {
            bowArm.SetActive(false);
        }
    }
}
