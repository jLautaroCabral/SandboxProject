using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour {

	public Vector2 finalPos;
	public float speed;

    Vector2 initialPos;

	void Start() {
		initialPos = transform.position;
	}

	void Update() {
		transform.position += (Vector3)Vector2.up * speed * Time.deltaTime;
		if (transform.position.y > finalPos.y) {
			transform.position = new Vector3(transform.position.x, initialPos.y, transform.position.y);
		}
	}
}
