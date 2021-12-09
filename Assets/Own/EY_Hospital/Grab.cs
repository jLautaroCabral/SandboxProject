using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EY_Hospital
{
    public class Grab : MonoBehaviour
    {
        [SerializeField]
        GameObject grabObject;
        [SerializeField]
        GameObject p_Accion;
        [SerializeField]
        Camera cam;

        bool grabbingObject;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LayerMask unitLayerMask = LayerMask.GetMask("Default");
                Ray ray = new Ray(cam.transform.position, cam.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 4f, unitLayerMask))
                {
                    //Equipamiento equipamiento = hit.collider.GetComponent<Equipamiento>();
                    p_Accion.SetActive(true);
                    p_Accion.SetActive(false);
                }
            }
        }
    }
}
