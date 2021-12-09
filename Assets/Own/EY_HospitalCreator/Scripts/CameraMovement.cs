using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float scrollSpeed = 20.0f;
    public float minZoomLimit = 10.0f;
    public float maxZoomLimit = 75.0f;

    [SerializeField] private Camera cam = null;
    [SerializeField] private Transform target = null;

    private Vector3 previousPosition;
    private Transform thisT;

    [SerializeField] bool allowZoomIn, allowZoomOut;
    float currentZoomValue = 0.0f;

    private void Awake()
    {
        thisT = GetComponent<Transform>();
    }

    private void Start()
    {
        //currentZoomValue = 15.0f;
        

        thisT.Translate(new Vector3(0, 0, -20f));
/*
        float currentPosZ = thisT.localPosition.z;
        currentPosZ = Mathf.Clamp(currentPosZ, -maxZoomLimit, -minZoomLimit);
*/
        Vector3 supp = target.position - thisT.position;
        currentZoomValue = supp.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(2))
        {
            Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

            thisT.position = target.position;

            thisT.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            thisT.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            thisT.Translate(new Vector3(0, 0, -Mathf.Abs(currentZoomValue)));

            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll != 0)
        {
            float currentPosZ = thisT.localPosition.z;
            if (currentPosZ > 0)
                currentPosZ = Mathf.Clamp(currentPosZ, minZoomLimit, maxZoomLimit);
            else
                currentPosZ = Mathf.Clamp(currentPosZ, -maxZoomLimit, -minZoomLimit);


            if (currentPosZ == -minZoomLimit || currentPosZ == minZoomLimit)
                allowZoomIn = false;
            else
                allowZoomIn = true;

            if (currentPosZ == -maxZoomLimit || currentPosZ == maxZoomLimit)
                allowZoomOut = false;
            else
                allowZoomOut = true;

            if (scroll > 0 && allowZoomIn)
                thisT.Translate(new Vector3(0, 0, scrollSpeed * Time.deltaTime));
            else if (scroll < 0 && allowZoomOut)
                thisT.Translate(new Vector3(0, 0, -scrollSpeed * Time.deltaTime));

            Vector3 supp = target.position - thisT.position;
            currentZoomValue = supp.magnitude;
        }
    }
}
