using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{

    Vector2[] points = new Vector2[100];
    public LineRenderer line;
    public EdgeCollider2D edge;

    public int maxRange; //The max height 
    public int minRange; //Minimum range
    public float xDistance; //The lenght of track;

    public Vector2 firstPosition; //The first position of the track;
    public bool totallyRandom; //no control on the parameters

    void Start()
    {
        line.positionCount = 100;
        int count = -1;
        int r = 0;

        line.SetPosition(0, firstPosition);

        if (totallyRandom)
        {
            maxRange = Random.Range(1, 20);
            minRange = Random.Range(-20, -1);
            xDistance = Random.Range(10, 15);
        }

        foreach (Vector2 num in points)
        {
            count++;
            if (count == 0)
            {
                points[count].x = firstPosition.x;
                points[count].y = firstPosition.y;
                line.SetPosition(count, points[count]);
            }
            else
            {
                points[count].x = count * xDistance;
                r = Random.Range(r + minRange, r + maxRange);
                points[count].y = r;
                line.SetPosition(count, points[count]);

                Debug.Log("r  " + r);
            }
        }

        edge.points = points; // The edge collider receive the same positions of the line renderer

    }
}

