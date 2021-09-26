using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Rigidbody2D rg2d, armRg;
	public float jumpForce, moveForce, armForce;
	public float maxInc, minInc;
	public float punchImpact;
	public int playerIndex;
	public Transform shoe, head;
	public BoxingManager manager;
	public bool isAI;
	public AudioSource jumpSound, hitSound; 
	[HideInInspector] public bool isHitted, grounded;

	int moveDirection = 1;


	void Update() {
        Move();

		if (isAI) return;

		if ((Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) && grounded && manager.gameStarted ) {
			Jump();
		}
	}

	public void Jump() {
		Vector2 direction = head.position - shoe.position;

		rg2d.AddForce(direction * jumpForce);

		grounded = false;

		jumpSound.Play();
	}

	void Move() {
		float rot = (transform.rotation.eulerAngles.z < 200)? transform.rotation.eulerAngles.z : (transform.rotation.eulerAngles.z - 360);

		if (rot > maxInc) {
			moveDirection = -1;
		}

		if (rot  < minInc) {
			moveDirection = 1;
		}

        rg2d.AddTorque(moveDirection * moveForce);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Ground") {
			grounded = true;
		}

		if (coll.collider.tag == "Glove" &&  !isHitted && coll.otherCollider.tag == "Body") {
			isHitted = true;
			hitSound.Play();
            rg2d.AddForce(Vector2.right * punchImpact);
		}

		if (coll.otherCollider.tag == "Glove" && !isHitted && coll.collider.tag == "Body") {
			manager.PlayerScores(playerIndex);
		}
	}
}
