using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	FactionIdentifier myFaction;
	float timerLimit = 15.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * 10 * Time.deltaTime);
		timerLimit -= Time.deltaTime;
		if (timerLimit <= 0) {
			Destroy (this.gameObject);
		}
	}

	public void initialise(FactionIdentifier fi)
	{
		myFaction = fi;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (myFaction == null) {
			if (other.GetComponent<FactionIdentifier> () == true) {
				if (other.gameObject.GetComponent<UnitHealth> () == true) {
					Debug.Log ("Arrow hit unit");
					other.gameObject.GetComponent<UnitHealth> ().dealDamage (2.0f);
					Destroy (this.gameObject);
				}
			}
		} else {
			if (other.GetComponent<FactionIdentifier> () == true) {
				if (other.gameObject.GetComponent<FactionIdentifier> () != myFaction) {
					if (other.gameObject.GetComponent<UnitHealth> () == true) {
						Debug.Log ("Arrow hit unit");
						other.gameObject.GetComponent<UnitHealth> ().dealDamage (2.0f);
						Destroy (this.gameObject);
					}
				}
			} else {
				if (other.gameObject.GetComponent<UnitHealth> () == true) {
					Debug.Log ("Arrow hit unit");
					other.gameObject.GetComponent<UnitHealth> ().dealDamage (2.0f);
					Destroy (this.gameObject);
				}
			}
		}
	}
}
