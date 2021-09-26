using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingOffset : MonoBehaviour {

    public float velocity;

    bool started;

    BulletControl bullet;

    private void Awake()
    {
        bullet = FindObjectOfType<BulletControl>();
    }

    private void FixedUpdate()
    {
        if (!started && Input.GetMouseButtonDown(0)) started = true;

        if (started && !bullet.isGameOver) transform.Translate(Vector2.right * velocity);
    }
}
