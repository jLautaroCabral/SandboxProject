using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MotorController : MonoBehaviour
{

    private WheelJoint2D wheel;
    private JointMotor2D motor;
    private Rigidbody2D rg2D;
    private float motorInput;
    private float bodyInput; //Editor
    private bool motorON;

    public float speed; // if the value is negative, the is going forward
    public float acceleration;
    public float force;  //mobile, from gyro 
    public float maxSpeed;
    public float bodyForce;

    public bool automaticAccelerate;
    public bool accelereraateInAir;


    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        wheel = GetComponent<WheelJoint2D>();
        motorON = wheel.useMotor;

        motor = new JointMotor2D();
        motor.maxMotorTorque = 10000;
        wheel.motor = motor;
    }

    private void FixedUpdate()
    {
        GetInputs();
        Motor();
        BodyForce(bodyForce);

        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GetInputs()
    {
        motorInput = Input.GetAxisRaw("Horizontal");
        bodyInput = Input.GetAxisRaw("Vertical");

        if (automaticAccelerate && AirControl.isGrounded) motorInput = 1;        // 1: accelate forward
        else if (accelereraateInAir && automaticAccelerate) motorInput = 1;
        else return;

        MotorOnInput();
    }

    void Motor()
    {
        if (wheel.useMotor) 
        {
            if (motorInput > 0 || MobileInput.IsAccelareting) 
            {
                if (wheel.jointSpeed <= 0) //Get  the rotation direction
                {
                    if (speed > -maxSpeed) { speed = speed - acceleration; } //accelarate until achieve the max speed (forward)
                    motor.motorSpeed = wheel.jointSpeed + speed;
                }
                else
                {
                    //Brake
                    motor.motorSpeed = 0;
                    speed = 0;
                }
            }
            else if (motorInput < 0 || MobileInput.IsBraking)
            {
                if (wheel.jointSpeed >= 0)
                {
                    if (speed < maxSpeed) speed = speed + acceleration; //accelarate until achieve the max speed (backward)
                    motor.motorSpeed = wheel.jointSpeed + speed;
                }
                else
                {
                    //Brake
                    motor.motorSpeed = 0;
                    speed = 0;
                }
            }
            else
            {
                motor.motorSpeed = wheel.jointSpeed;
                wheel.useMotor = false;
            }
            wheel.motor = motor;
        }
    }

    void MotorOnInput()
    {
        if (motorInput != 0 || MobileInput.IsAccelareting || MobileInput.IsBraking)
        {
            wheel.useMotor = true;
        }
        else
        {
            wheel.useMotor = false;
        }

    }

    void BodyForce(float force)
    {
        //PC, unity editor
        if (AirControl.isGrounded) { rg2D.AddTorque(bodyInput * force); }
        if (!AirControl.isGrounded) { rg2D.AddTorque(bodyInput * force ); }

        //Mobile
        if (AirControl.isGrounded) { rg2D.AddTorque(Input.acceleration.x * -force); }
        if (!AirControl.isGrounded) { rg2D.AddTorque(Input.acceleration.x * -(force)); }

    }


    public void OnAddMoreForce()
    {
        force = force + 10;
        Debug.Log(force);
    }
}
