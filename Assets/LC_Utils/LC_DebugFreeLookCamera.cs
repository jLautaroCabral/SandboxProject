using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LC_DebugFreeLookCamera : MonoBehaviour
{
    [Header("Look Sensibility")]
    public float sensX = 10;
    public float sensY = 10;

    [Header("Clamping")]
    public float minY = -90;
    public float maxY = 90;

    [Header("Spectator")]
    public float spectatorMoveSpeed = 10;

    private float rotX;
    private float rotY;

    private bool isSpectator = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        rotX += Input.GetAxis("Mouse X") * sensX;
        rotY += Input.GetAxis("Mouse Y") * sensY;

        rotY = Mathf.Clamp(rotY, minY, maxY);

        if(isSpectator)
        {
            transform.rotation = Quaternion.Euler(-rotX, rotY, 0);
            float x = Input.GetAxis("Horizontal"); 
            float z = Input.GetAxis("Vertical"); 
            float y = 0;

            if(Input.GetKey(KeyCode.E))
            {
                y = 1;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                y = -1;
            }

            Vector3 dir = transform.right * x + transform.up * y + transform.forward * z;
            transform.position += dir * spectatorMoveSpeed * Time.deltaTime;
        }
        else
        {

        }
    }

    public static void CreateSpectator()
    {
        GameObject cameraInstance = GameObject.Find("LC_DebugFreeLookCamera");
        Camera cam;
        if (cameraInstance == null)
        {
            cameraInstance = new GameObject("LC_DebugFreeLookCamera");
            cam = cameraInstance.AddComponent<Camera>();
            cameraInstance.AddComponent<LC_DebugFreeLookCamera>();
        } else
        {
            cam = cameraInstance.GetComponent<Camera>();
        }
        
        Camera.SetupCurrent(cam);
    }
    //private void 
}
