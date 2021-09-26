using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CorchoGames.NinjaEgg
{
    public class Grappling : MonoBehaviour
    {
        public bool isFixedDirection;
        public Vector2 direction;

        public TargetJoint2D tJ2D;
        public LineRenderer line;
        //public AudioSource gSound;

        private void Start()
        {
            tJ2D = GetComponent<TargetJoint2D>();
            line = GetComponent<LineRenderer>();

            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (!Application.isMobilePlatform)
            {
                if (Input.GetMouseButtonDown(0)) GrapplingOn();
                if (Input.GetMouseButtonUp(0)) GrapplingOff();
            }
            else
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) GrapplingOn();
                if (touch.phase == TouchPhase.Ended) GrapplingOff();
            }

            line.SetPosition(0, transform.position);
            line.SetPosition(1, tJ2D.target);
        }

        public void GrapplingOn()
        {
            tJ2D.enabled = true;
            line.enabled = true;
            /*
            Vector2 mousePos;
            Vector2 origin = transform.position;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ignore"));
            */
            Vector2 mousePos = Input.mousePosition;
            Vector2 origin = transform.position;

            Vector2 raycastDirection = mousePos - origin;

            RaycastHit2D hit2D = Physics2D.Raycast(origin, raycastDirection, Mathf.Infinity, LayerMask.GetMask("Default"));


            if (hit2D.collider != null)
            {
                tJ2D.target = hit2D.point;
            }
        }

        public void GrapplingOff()
        {
            tJ2D.enabled = false;
            line.enabled = false;
        }
    }
}

