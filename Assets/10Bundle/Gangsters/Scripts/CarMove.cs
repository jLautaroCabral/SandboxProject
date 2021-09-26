using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour {

	public Transform leftCar, rightCar;
	public Vector2 initialPos, finalPos;
	public float speed;

	void Update() {
		leftCar.position -= ((Vector3)Vector2.right * speed * Time.deltaTime);
        rightCar.position += ((Vector3)Vector2.right * speed * Time.deltaTime);

		if (leftCar.position.x <= initialPos.x) {
			leftCar.position = new Vector3(finalPos.x, leftCar.position.y, leftCar.position.z);
		} 

		if (rightCar.position.x >= finalPos.x) {
			rightCar.position = new Vector3(initialPos.x, rightCar.position.y, rightCar.position.z);
		} 
	}
}
