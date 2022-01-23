using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EY_HospitalCreator
{
    public class CameraTargetController : MonoBehaviour
    {
        public float speed = 200.0f;
        public Transform target;

        [field: SerializeField]
        private bool enableMovement;
        public bool EnableMovement { get { return enableMovement; } }

        [SerializeField]
        private Vector3 Limit = new Vector3(30, 30, 30);

        private Vector3 lastMovement;
        private Rigidbody rg;
        private bool move;
        private readonly string horizontal = "Horizontal";
        private readonly string vertical = "Vertical";

        private void Awake()
        {
            rg = GetComponent<Rigidbody>();
        }

        void Update()
        {
            move = false;

            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = rotation;

            if(EnableMovement)
            {
                if (Mathf.Abs(Input.GetAxisRaw(horizontal)) > 0.5f || Mathf.Abs(Input.GetAxisRaw(vertical)) > 0.5)
                {
                    rg.velocity = Vector3.zero;
                    lastMovement = new Vector3(-Input.GetAxisRaw(horizontal), 0, -Input.GetAxisRaw(vertical));
                    rg.AddRelativeForce(lastMovement.normalized * speed * Time.deltaTime, ForceMode.VelocityChange);
                    move = true;
                }
                CorregirPosicionSegunLimites();
            }

            if (!move)
            {
                rg.velocity = Vector2.zero;
            }
        }

        private void CorregirPosicionSegunLimites()
        {
            Vector3 pos = transform.position;

            pos.x = Mathf.Clamp(pos.x, -Limit.x / 2, Limit.x / 2);
            pos.y = Mathf.Clamp(pos.y, -Limit.y / 2, Limit.y / 2);
            pos.z = Mathf.Clamp(pos.z, -Limit.y / 2, Limit.y / 2);

            transform.position = pos;
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, Limit);
        }
    }
}
