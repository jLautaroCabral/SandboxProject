using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _3OGS.Debug.Cameras
{
    public class _3OGS_RotateAroundCamera : MonoBehaviour
    {
        [Header("Camera settings")]
        public Transform target;
        public float scrollSpeed = 65f;
        [Header("Angle settings")]
        [Range(45, 80)]
        public float cameraXRotationMaxAnle = 80;
        [Range(-80, 35)]
        public float cameraXRotationminAnle = 35;
        [Header("Zoomb settings")]
        public float minZoomLimit = 8f;
        public float maxZoomLimit = 25f;

        private Camera cam;

        private Vector3 previousPosition;

        private Transform thisT;

        private bool allowZoomIn, allowZoomOut, allowRotateUp = true, allowRotateDown = true;

        private float currentZoomValue;

        private Vector3 lastCameraOffset;

        private static readonly string cameraGameObjName = "LC_Debug_RotateAroundCamera";

        private void Awake()
        {
            cam = GetComponent<Camera>();
            thisT = this.transform;
        }

        private void Start()
        {
            Vector3 supp = target.position - thisT.position;
            currentZoomValue = supp.magnitude;
        }

        // Update is called once per frame
        void Update()
        {
            HandleCameraPosition();
            HandleZoom();
        }

        void HandleCameraPosition()
        {
            if (Input.GetMouseButtonDown(2))
            {
                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(2))
            {
                Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

                thisT.position = target.position;

                float xRotation = this.transform.rotation.eulerAngles.x;
                xRotation = xRotation > 270f ? xRotation - 360 : xRotation;

                allowRotateDown = xRotation < cameraXRotationminAnle ? false : true;
                allowRotateUp = xRotation > cameraXRotationMaxAnle ? false : true;

                if (direction.y > 0f && allowRotateUp)
                    thisT.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                else if (direction.y < 0f && allowRotateDown)
                    thisT.Rotate(new Vector3(1, 0, 0), direction.y * 180);

                thisT.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);

                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);

                lastCameraOffset = target.position - this.transform.position;
            }
            else if (lastCameraOffset != null)
            {
                this.transform.position = target.position/* - lastCameraOffset*/;
            }

            thisT.Translate(new Vector3(0, 0, -Mathf.Abs(currentZoomValue)));
        }

        void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                float zoomDistance = Vector3.Distance(thisT.position, target.position);

                allowZoomOut = zoomDistance > maxZoomLimit ? false : true;
                allowZoomIn = zoomDistance < minZoomLimit ? false : true;

                if (scroll > 0 && allowZoomIn)
                    thisT.Translate(new Vector3(0, 0, scrollSpeed * Time.deltaTime));
                else if (scroll < 0 && allowZoomOut)
                    thisT.Translate(new Vector3(0, 0, -scrollSpeed * Time.deltaTime));

                Vector3 supp = target.position - thisT.position;
                currentZoomValue = supp.magnitude;
            }
        }

        public static Camera CreateRotateAroundToTargetCamera(Transform target)
        {
            GameObject cameraInstance = null;// = GameObject.Find(spectatorCameraGameObjName);
            Camera cam;
            if (cameraInstance == null)
            {
                cameraInstance = new GameObject(cameraGameObjName);
                cam = cameraInstance.AddComponent<Camera>();
                _3OGS_RotateAroundCamera rotCamera = cameraInstance.AddComponent<_3OGS_RotateAroundCamera>();
                rotCamera.target = target;
            }
            else
            {
                cam = cameraInstance.GetComponent<Camera>();
            }

            return cam;
        }
    }
}

