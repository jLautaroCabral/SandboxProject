using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHManager : MonoBehaviour {

    public Color[] colors;
    public float timeToChangeColor;
    public Camera cam;
    float time = 0f;
    bool can;
    private void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time > 2 && !can)
        {
            Color c = colors[Random.Range(0, colors.Length)];
            if (c == cam.backgroundColor) c = colors[Random.Range(0, colors.Length)];
            //else return;
            StartCoroutine(ChangeColor(c));
            can = true;
            //time = 0;
            Debug.Log("yeah");
        }
    }

    public IEnumerator ChangeColor(Color c)
    {
        float t = 0f;
        while (t < timeToChangeColor)
        {
            t += Time.deltaTime;
            cam.backgroundColor = Color.Lerp(cam.backgroundColor, c, t/20);
            yield return null;
        }
        cam.backgroundColor = c;
        time = 0f;
        can = false;
    }
}
