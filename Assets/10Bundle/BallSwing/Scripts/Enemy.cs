using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float velocity, delay;
    public Fighter fighter;
    public TrailRenderer trail, otherTrail;

    Vector3 currentVelocity;
    Rigidbody2D rg2d;
    Vector3 target;

    SpriteRenderer mySprite;
    CircleCollider2D coll;
    Vector3 direction;

    private void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<CircleCollider2D>();

        int t = Random.Range(0, 5);
        if (t >= 4) {
            Color color = Camera.main.backgroundColor;
            color.a = 0.2f;
            otherTrail.startColor = color;
        }
        else {
            otherTrail.enabled = false;
        }
    }

    private void Update()
    {
        AttackPlayer();
    }

    void AttackPlayer()
    {
        direction = fighter.transform.position - transform.position;

        //target = Vector3.SmoothDamp(target, direction , ref currentVelocity, Time.deltaTime * delay);
        target = direction;

        rg2d.velocity = target.normalized * velocity * Time.deltaTime * 10;
    }

    IEnumerator Die() {
        float a = 1;
        velocity = 0;
        coll.enabled = trail.enabled = otherTrail.enabled = false;
 
        for (int i = 0; i < 15; i++) {
            mySprite.color = new Color(0, 0, 0, a);
            a -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        //if (coll.collider.tag == "hand") Destroy(gameObject);
        StartCoroutine(Die());
    }
}
