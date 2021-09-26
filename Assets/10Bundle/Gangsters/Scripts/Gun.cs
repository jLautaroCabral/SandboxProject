using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public float shootForce;

	public Bullet[] projectiles;
	public Transform arm, hand;
	public Transform bulletInitialPosition;
	public GManager manager;
	public GPlayer player;
    public AudioSource shootSound;

	int shootCount;

	void Update() {
		if (Input.GetButtonDown("Jump") && manager.gameStarted && !player.isAI) {
			Shoot();
		}
	}

	public void Shoot() {
		Vector2 direction = hand.position - arm.position;

		projectiles[shootCount].transform.position = bulletInitialPosition.position;
		projectiles[shootCount].direction = direction;
		projectiles[shootCount].speed = shootForce;

		shootCount++;
		if (shootCount >= projectiles.Length) {
			shootCount = 0;
		}

		shootSound.Play();
	}
}
