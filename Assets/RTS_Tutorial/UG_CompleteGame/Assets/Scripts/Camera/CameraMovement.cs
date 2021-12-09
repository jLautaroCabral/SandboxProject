using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EY_HospitalCreator
{
	public class CameraMovement : MonoBehaviour
	{
		void Update()
		{
			moveCamera();
		}

		void moveCamera()
		{
			Vector3 moveDir = Vector3.zero;

			if (Input.GetKey(KeyCode.W))
			{
				moveDir += Vector3.up;
			}

			if (Input.GetKey(KeyCode.A))
			{
				moveDir += Vector3.left;
			}

			if (Input.GetKey(KeyCode.S))
			{
				moveDir += Vector3.down;
			}

			if (Input.GetKey(KeyCode.D))
			{
				moveDir += Vector3.right;
			}

			transform.Translate(moveDir * 10.0f * Time.deltaTime);
		}
	}
}
