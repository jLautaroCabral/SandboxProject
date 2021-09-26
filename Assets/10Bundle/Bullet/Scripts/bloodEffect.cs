using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodEffect : MonoBehaviour {

    public ParticleSystem bloodPrefab;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        ParticleSystem blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);

        Destroy(blood, 2);
    }
}
