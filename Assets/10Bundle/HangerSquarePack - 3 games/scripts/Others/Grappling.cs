using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour {

    public bool isFixedDirection;
    public Vector2 direction;

    public TargetJoint2D tJ2D;
    public LineRenderer line;
    public AudioSource gSound;

    GameManager gameManager;

    private void Start()
    {
        tJ2D = GetComponent<TargetJoint2D>();
        gameManager = FindObjectOfType<GameManager>();

        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (!Application.isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0)) GrapplingOn();
            if (Input.GetMouseButtonUp(0)) GrapplingOff();
        }
        else
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) GrapplingOn();
            if (touch.phase == TouchPhase.Ended) GrapplingOff();
        }

        //tJ2D.anchor = transform.position;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, tJ2D.target);

        //if (Input.GetKeyDown(KeyCode.Space)) Debug.Log("Anchor: " + tJ2D.anchor + " Target: " + tJ2D.target);
    }

    public void GrapplingOn()
    {
        gSound.Play();
        tJ2D.enabled = true;
        line.enabled = true;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 origin = new Vector2(transform.position.x, transform.position.y + 0.5f);

        Vector2 pos = mousePos - origin;

        RaycastHit2D hit = isFixedDirection? 
            Physics2D.Raycast(origin, direction) : Physics2D.Raycast(origin, pos);

        //RaycastHit2D hit = Physics2D.Raycast(origin, pos);
        //RaycastHit2D hit = Physics2D.Raycast(origin, direction);

        if (hit.collider != null)
        {
            tJ2D.target = hit.point;
        }
    }

    public void GrapplingOff()
    {
        tJ2D.enabled = false;
        line.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameManager.GameOver();
    }

}
