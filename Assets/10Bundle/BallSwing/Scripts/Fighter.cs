using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fighter : MonoBehaviour {

    public float rotSpeed;
    public Transform shadow;
    public Manager manager;
    public AudioSource tapSound;

    void Update() {
        RotatePlayer();

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            rotSpeed = -rotSpeed;
            tapSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void RotatePlayer() {
        float direction = Input.GetAxisRaw("Horizontal");

        //transform.Rotate(0, 0 , direction * rotSpeed);

        transform.Rotate(0, 0, rotSpeed); 
        shadow.Rotate(0, 0, rotSpeed * 0.5f);
    }

    public virtual void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "enemy" && coll.otherCollider.tag == "Player") {
            manager.CallGameOver();
        }

        if (coll.gameObject.tag == "enemy" && coll.otherCollider.tag == "hand") {
            manager.AddScore(1);
        }
    }
}
