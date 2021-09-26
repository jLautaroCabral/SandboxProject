using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraQuickMove : MonoBehaviour {
	int counter = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inputDetect ();
	}

	void inputDetect()
	{
		if (Input.GetKeyDown (KeyCode.O)) {
			Vector3 pos = FactionController.me.factionsInGame [counter].spawnLocation;
			pos.z = -10;
			this.transform.position = pos;
			if (counter < FactionController.me.factionsInGame.Count-1) {
				counter++;
			} else {
				counter = 0;
			}
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}
}
