using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is where the enemy launch his arrow and calculate everything, in the documentation i explain
/// in detail how each method work
/// </summary>

public class EnemyBow : MonoBehaviour {

    [SerializeField] private GameObject target;
    [SerializeField] private GameObject origin;    
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arm;

    [SerializeField] private float angle = 45;
    [SerializeField] private float maxDistance2Shoot;
    [SerializeField] private float time2Anothershoot;

    private GameObject arrow;

    private float xDistance, yDistance;
    private float gravity;
    private float angleRadians;
    private float velocity = 0;
    private float xV, yV;
    private float time;

    private Rigidbody2D rg2d;

    List<GameObject> arrows = new List<GameObject>();
    int count;

    private void Update()
    {
        Decide_When_Shoot();

        EnemyBowRotation();
        BowPosition();

        if (arrow != null)
        {
            SetPlayerArrowColliderActive();
            ArrowRotation(arrow);
        }
            
    }

    void Decide_When_Shoot()
    {
        float distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);

        if (distanceFromTarget < maxDistance2Shoot)
        {
            time += Time.deltaTime;
            if (time > time2Anothershoot)
            {
                time = 0;
                Launch();
            }
        }
    }

    private void Launch()
    {
        InstantiateArrow();
        FindxDistanceAndyDistance();
        CalculateInitialVelocity();
        FindxVandyV();
        LauchProjectile();
        DestroyArrow();
    }

    private void InstantiateArrow()
    {
        arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
        rg2d = arrow.GetComponent<Rigidbody2D>();
        arrow.GetComponent<BoxCollider2D>().enabled = false;

        gravity = Mathf.Abs(Physics2D.gravity.y * rg2d.gravityScale);
        angleRadians = Mathf.Deg2Rad * angle;

        arrows.Add(arrow);
        count++;
    }

    /// <summary>
    /// This method will avoid that the player or enemy hit himself when shoot, the collider will be active after
    /// the arrow be far enough from player
    /// </summary>
    void SetPlayerArrowColliderActive()
    {
        float distance = Vector2.Distance(arrow.transform.position, origin.transform.position);
        if (distance > 5) arrow.GetComponent<BoxCollider2D>().enabled = true;
    }

    void FindxDistanceAndyDistance()
    {
        xDistance = Mathf.Abs(origin.transform.position.x - target.transform.position.x);
        yDistance = target.transform.position.y - origin.transform.position.y;
    }

    void CalculateInitialVelocity()
    {
        float a = (xDistance * xDistance * gravity);
        float b = xDistance * Mathf.Sin(2 * angleRadians);    
        float c = 2 * yDistance * Mathf.Cos(angleRadians) * Mathf.Cos(angleRadians);
        velocity = Mathf.Sqrt(a/(b - c));
    }

    void FindxVandyV()
    {
        xV = velocity * Mathf.Cos(angleRadians);
        yV = velocity * Mathf.Sin(angleRadians);

        if (origin.transform.position.x > target.transform.position.x)
        {
            xV *= -1;
        }        
    }

    void LauchProjectile()
    {
        if (xV.ToString() != "NaN")
            rg2d.velocity = new Vector2(xV, yV);
    }

    void ArrowRotation(GameObject arrow)
    {
        Rigidbody2D rig2d = arrow.GetComponent<Rigidbody2D>();
        Vector3 v = rig2d.velocity;
        float r = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Vector3 rotation = new Vector3(0, 0, r - 180);

        arrow.transform.localEulerAngles = rotation;
    }

    void BowPosition()
    {
        //The bow will be on the same position of the arm
        transform.position = arm.transform.position;
    }

    void EnemyBowRotation()
    {
        float off = (origin.transform.position.x < target.transform.position.x) ? 180 : 270;
        Vector3 rotation = new Vector3(0, 0, angle - off);
        transform.localEulerAngles = rotation;
    }

    void DestroyArrow()
    {
        GameObject arrow2Destroy = arrows[count - 1];
        Destroy(arrows[count - 1], 2);
    }
}
