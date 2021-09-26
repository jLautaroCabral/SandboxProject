using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This class require a event system component in the scene
/// This class will lauch the arrow
/// This script is attached on playerbow game object
/// </summary>

public class BowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab, bow, player;
    [SerializeField] private float maxForce, maxVelocity;
    public int numberOfArrows;

    private int count;
    private bool isTouching;
    private GameObject arrow;

    [SerializeField] private LineRenderer line;
    private Vector2 mousePosFinal, initialMousePos, direction;
    private Vector3 aim;

    List<GameObject> arrows = new List<GameObject>();

    private void Start()
    {
        line.enabled = false;

        if (EventSystem.current == null)
        {
            Debug.LogError("You need include a EventSystem in the scene to this scripts works");
        }
    }

    private void Update()
    {
        bow.transform.position = player.transform.position; 

        //Read the input
        if (Input.GetMouseButtonDown(0) && numberOfArrows > 0)
        {
            if (Application.isMobilePlatform)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    isTouching = true;
                }
            }

            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isTouching = true;
            }
            
        }

        //When release the finger or mouse button the arrow will be instanted ans launched
        if (Input.GetMouseButtonUp(0) && numberOfArrows > 0 && isTouching)
        {
            arrow = Instantiate(arrowPrefab, bow.transform.position, bow.transform.rotation);
            arrow.GetComponent<BoxCollider2D>().enabled = false;
            arrows.Add(arrow);
            LauchArrow(arrow);

            isTouching = false;
            count++;
        }

        //Will draw the red line that you see in the screen when shooting
        if (isTouching)
        {
            mousePosFinal = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            line.enabled = true;
            DrawLine();
            BowRotation();
        }
        else
        {
            line.enabled = false;
        }

        if (arrow != null)
        {
            ArrowRotationInTheAir(arrow);
            SetPlayerArrowColliderActive();
        }
    }

    /// <summary>
    /// This method will avoid that the player hit himself when shoot, the collider will be active after
    /// the arrow be far from player
    /// </summary>
    void SetPlayerArrowColliderActive()
    {
        float distance = Vector2.Distance(arrow.transform.position, player.transform.position);
        if (distance > 5) arrow.GetComponent<BoxCollider2D>().enabled = true;
    }

    void LauchArrow(GameObject arrow)
    {        
        float distance = Vector2.Distance(initialMousePos, mousePosFinal);

        direction *= maxForce;

        //Appy proportional velocity with how much the player drag the finger
        while (direction.magnitude > maxVelocity)
        {
            direction = direction / 1.2f;
        }

        arrow.GetComponent<Rigidbody2D>().velocity = -direction;

        DestroyArrow();
        numberOfArrows--;
    }

    void DestroyArrow()
    {
        GameObject arrow = arrows[count];
        Destroy(arrows[count], 5);
    }

    void DrawLine()
    {
        line.positionCount = 2;
        line.SetPosition(0, initialMousePos);
        line.SetPosition(1, mousePosFinal);
    }

    void BowRotation()
    {
        direction = mousePosFinal - initialMousePos;

        Vector3 d = mousePosFinal - initialMousePos;
        d.Normalize();
        float rotation = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
        aim = new Vector3(0, 0, rotation - 180);
        bow.transform.localEulerAngles = aim;
    }

    void ArrowRotationInTheAir(GameObject arrow)
    {
        Rigidbody2D rig2d = arrow.GetComponent<Rigidbody2D>();
        Vector3 v = rig2d.velocity;
        float r = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Vector3 rotation = new Vector3(0, 0, r - 180);

        arrow.transform.localEulerAngles = rotation;
    }
}
