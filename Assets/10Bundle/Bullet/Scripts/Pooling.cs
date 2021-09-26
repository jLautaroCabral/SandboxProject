using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour {

    public GameObject[] obstacles;
    public GameObject player;

    private int count; 

    Vector2 limitPos, lastPos, firstPos;

    public float obsjectSize;
    public float yPos;

    public bool isParallaxing;

    private void Start()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            Vector2 pos = new Vector2(i * obsjectSize, yPos);

            obstacles[i].transform.position = pos;
        }

        count = 0;

        limitPos = obstacles[obstacles.Length - 2].transform.position;
        lastPos = obstacles[0].transform.position;
        firstPos = obstacles[obstacles.Length - 1].transform.position;
    }

    private void Update()
    {
        if (player.transform.position.x > limitPos.x)
        {
            limitPos = new Vector2(limitPos.x + obsjectSize, yPos);
            firstPos = new Vector2(firstPos.x + obsjectSize, yPos);
            lastPos = new Vector2(lastPos.x + obsjectSize, yPos);

            obstacles[count].transform.position = firstPos;

            count++;
        }

        if (count >= obstacles.Length)
        {
            count = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isParallaxing) Parallax();
    }

    void Parallax()
    {
        //Only used for parallax effect in the backgrounds
        limitPos = new Vector2(limitPos.x + 0.1f, yPos);
        firstPos = new Vector2(firstPos.x + 0.1f, yPos);
        lastPos = new Vector2(lastPos.x + 0.1f, yPos);
    }
}
