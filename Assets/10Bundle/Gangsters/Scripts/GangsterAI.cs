using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangsterAI : MonoBehaviour {

	public GPlayer player;
	public GManager manager;
	public Gun gun;
	public float fireRate;

	float time;

	void Update() {
		if (!manager.gameStarted) return;
		
		if (player.grounded) {
			player.Jump();
		}

		player.RotateArm();

		time += Time.deltaTime;
		if (time > fireRate) {
			gun.Shoot();
			time = 0;
		}
	}
}
