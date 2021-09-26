using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPooling : MonoBehaviour {

    public enum PoolingDirection { Horizontal, Vertical }
    public PoolingDirection direction;
    public GameObject[] obstacles;
    public GameObject player;
    public float obsjectSize;
    private int count; 
    Vector2 limitPos, lastPos, firstPos;
    Vector2 pos;
    


    private void Start()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (direction == PoolingDirection.Horizontal)
            {
                pos = new Vector2(i * obsjectSize, 0);
            } 
            else
            {
                pos = new Vector2(0, i * obsjectSize);
            }

            obstacles[i].transform.position = pos;
        }

        count = 0;

        limitPos = obstacles[obstacles.Length - 2].transform.position;
        lastPos = obstacles[0].transform.position;
        firstPos = obstacles[obstacles.Length - 1].transform.position;
    }

    private void Update()
    {
        if (direction == PoolingDirection.Horizontal)
            HorizontalPooling();
        else
            VerticalPooling();
    }

    void HorizontalPooling()
    {
        if (player.transform.position.x > limitPos.x)
        {
            limitPos = new Vector2(limitPos.x + obsjectSize, 0);
            firstPos = new Vector2(firstPos.x + obsjectSize, 0);
            lastPos = new Vector2(lastPos.x + obsjectSize, 0);

            obstacles[count].transform.position = firstPos;

            count++;
        }

        if (count >= obstacles.Length) count = 0;
    }

    void VerticalPooling()
    {
        if (player.transform.position.y > limitPos.y)
        {
            limitPos = new Vector2(0, limitPos.y + obsjectSize);
            firstPos = new Vector2(0, firstPos.y + obsjectSize);
            lastPos = new Vector2(0, lastPos.y + obsjectSize);

            obstacles[count].transform.position = firstPos;

            count++;
        }

        if (count >= obstacles.Length) count = 0;
    }
}
