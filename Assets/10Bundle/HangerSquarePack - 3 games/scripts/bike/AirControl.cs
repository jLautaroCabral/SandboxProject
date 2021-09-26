using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirControl : MonoBehaviour {

    public Rigidbody2D rg2d;
    public static bool isGrounded;

    public bool isForce = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void FixedUpdate ()
    {
        if (!isForce)
        {
            float angular = rg2d.angularVelocity;
            rg2d.AddTorque(0.2f);
            //Debug.Log(angular);
        }
        
        if (isForce && !isGrounded)
        {
            float angular = rg2d.angularVelocity;

            if (angular > 0) rg2d.AddTorque(Torque(angular) * -1);
            else if (angular < 0) rg2d.AddTorque(Torque(angular));

            //Debug.Log("Torque: " + Torque(angular));
        }
	}

    int Torque(float aVelocity)
    {
        int v = (int)aVelocity;

        if (v < 0) v = v * -1;
        if (v > 0 && v <= 10) return 5; 
        if (v > 10 && v <= 20) return 15;
        if (v > 20 && v <= 30) return 25;
        if (v > 30 && v <= 40) return 35;
        if (v > 40 && v <= 50) return 45;
        else return 50;
    }
}
