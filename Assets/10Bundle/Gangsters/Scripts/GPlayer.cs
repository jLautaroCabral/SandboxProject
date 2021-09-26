using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPlayer : MonoBehaviour {

	public Rigidbody2D rg2d, armRg;
	public float jumpForce, moveForce, armForce;
	public float maxInc, minInc;
	public float bulletImpact;
	public int playerIndex;
	public Transform shoe, head;
	public GManager manager;
	public bool isAI;
	public ParticleSystem blood;
	[HideInInspector] public bool isDead, grounded;

	int moveDirection = 1;



	void Update() {
        Move();

		if (isAI) return;

		if ((Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) && grounded && manager.gameStarted ) {
			Jump();
		}

		if ((Input.GetButton("Jump") || Input.GetMouseButton(0)) && manager.gameStarted) {
            RotateArm();
		}
	}

	public void Jump() {
		Vector2 direction = head.position - shoe.position;

		rg2d.AddForce(direction * jumpForce);

		grounded = false;
	}

	public void RotateArm() {
        armRg.AddTorque(armForce * Time.deltaTime);
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

	void BloodEffect(Vector3 pos) {
		blood.transform.position = pos;
		blood.Play();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Bullet") {
			rg2d.AddForce(Vector2.right * bulletImpact);
			BloodEffect(coll.collider.transform.position);
		}

		if (coll.collider.tag == "DeathArea" && !isDead) {
			isDead = true;
			manager.PlayerScores(playerIndex);
		}

		if (coll.collider.tag == "Ground") {
			grounded = true;
		}

	}
}
