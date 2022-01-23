using LC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LC.Utils
{

    public class LookToCamera : MonoBehaviour
    {
        Camera target;
        public bool RotateY;
        private void Start()
        {
            StartCoroutine(nameof(Rotate));
        }

        IEnumerator Rotate()
        {
            
            Quaternion rotation;
            while (true)
            {
                target = CameraSwitcher.CurrentCamera;

                if(target != null)
                {
                    Vector3 direction = target.transform.position - transform.position;
                    if (RotateY)
                        rotation = Quaternion.LookRotation(-direction);
                    else
                        rotation = Quaternion.LookRotation(new Vector3(-direction.x, transform.rotation.y, -direction.z));

                    transform.rotation = rotation;
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}