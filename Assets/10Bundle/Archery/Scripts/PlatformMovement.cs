using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

    [SerializeField] private float speed;

    private ArcherControl player;

    public bool move;

    private void Awake()
    {
        player = FindObjectOfType<ArcherControl>();
    }

    private void FixedUpdate()
    {
        if (move) PlatfromGoUP();
    }

    private void PlatfromGoUP()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    
}
