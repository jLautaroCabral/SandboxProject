using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LC.Debug.Cameras
{
    public class LC_FreeLookCamera : MonoBehaviour
    {
        [Header("Camera Speed")]
        public float cameraSpeed = 10;

        [Header("Look Sensibility")]
        public float sensX = 10;
        public float sensY = 10;

        [Header("Clamping")]
        public float minY = -90;
        public float maxY = 90;

        private float rotX;
        private float rotY;

        private bool isSpectator = true;
        private static readonly string cameraGameObjName = "LC_Debug_FreeLookCamera";

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            if(LockMode())
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        bool LockMode()
        {
            return Input.GetMouseButton(1);
        }

        private void LateUpdate()
        {
            if(!LockMode())
            {
                rotX += Input.GetAxis("Mouse X") * sensX;
                rotY += Input.GetAxis("Mouse Y") * sensY;

                rotY = Mathf.Clamp(rotY, minY, maxY);

                if (isSpectator)
                {
                    transform.rotation = Quaternion.Euler(-rotY, rotX, 0);
                    float x = Input.GetAxis("Horizontal");
                    float z = Input.GetAxis("Vertical");
                    float y = 0;

                    if (Input.GetKey(KeyCode.E))
                    {
                        y = 1;
                    }
                    else if (Input.GetKey(KeyCode.Q))
                    {
                        y = -1;
                    }

                    Vector3 dir = transform.right * x + transform.up * y + transform.forward * z;
                    transform.position += dir * cameraSpeed * Time.deltaTime;
                }
            }
        }

        public static Camera CreateFreeLookCamera()
        {
            GameObject cameraInstance = null;// = GameObject.Find(spectatorCameraGameObjName);
            Camera cam;
            if (cameraInstance == null)
            {
                cameraInstance = new GameObject(cameraGameObjName);
                cam = cameraInstance.AddComponent<Camera>();
                cameraInstance.AddComponent<LC_FreeLookCamera>();
            }
            else
            {
                cam = cameraInstance.GetComponent<Camera>();
            }
            return cam;
        }
    }
}