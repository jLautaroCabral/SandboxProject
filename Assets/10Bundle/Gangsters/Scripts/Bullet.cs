using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[HideInInspector] public Vector3 direction;
	[HideInInspector] public float speed;

	void FixedUpdate() {
		transform.position += direction * speed;
	}
}
