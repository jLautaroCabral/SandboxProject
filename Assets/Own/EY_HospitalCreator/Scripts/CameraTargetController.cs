using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EY_HospitalCreator
{
    public class CameraTargetController : MonoBehaviour
    {
        public float speed = 200.0f;
        public Transform target;

        private Rigidbody rg;
        private bool move;
        private string horizontal = "Horizontal";
        private string vertical = "Vertical";
        private Vector2 Limit = new Vector2(100, 100);
        private Vector3 lastMovement;

        private float limitY = 0.0f;

        private void Awake()
        {
            rg = GetComponent<Rigidbody>();
        }

        void Update()
        {
            move = false;

            //float scroll = Input.GetAxis("Mouse ScrollWheel");
            //Camera.main.fieldOfView -= scroll * 20;
            //Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, MinZoom, MaxZoom);

            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = rotation;

            if (Mathf.Abs(Input.GetAxisRaw(horizontal)) > 0.5f || Mathf.Abs(Input.GetAxisRaw(vertical)) > 0.5)
            {
                rg.velocity = Vector3.zero;
                lastMovement = new Vector3(-Input.GetAxisRaw(horizontal), 0, -Input.GetAxisRaw(vertical));
                rg.AddRelativeForce(lastMovement.normalized * speed * Time.deltaTime, ForceMode.VelocityChange);
                move = true;
            }

            if (!move)
            {
                rg.velocity = Vector2.zero;
            }

            //CorregirPosicionSegunLimites();
        }

        private void CorregirPosicionSegunLimites()
        {
            Vector3 pos = transform.position;

            pos.x = Mathf.Clamp(pos.x, -Limit.x, Limit.x);
            pos.y = Mathf.Clamp(pos.y, -limitY, limitY);
            pos.z = Mathf.Clamp(pos.z, -Limit.y, Limit.y);

            transform.position = pos;
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
