using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathExample : MonoBehaviour {
	public int number=6;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			maths ();
		}
	}


	void maths()
	{
		for (float x = 0; x < 1.0f; x += 0.1f) {
			Debug.Log (x+" X = " + x);

			float x2 = x * number;

			Debug.Log (x+" X * " + number + " = " + x2);

			x2 = Mathf.Round (x2);

			Debug.Log (x+" X rounded is " + x2);

			x2 /= number;

			Debug.Log (x+ " X is now " + x2);

		}
	}
}
