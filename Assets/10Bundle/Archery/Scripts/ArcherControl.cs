using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will make player move, jump and make collisions detection, works on pc and mobile
/// </summary>

public class ArcherControl : MonoBehaviour {

    [SerializeField] private float runVelocity, jumpHeight, fallMultiplier, gravity;

    //Variables that don't need be modified outside this script
    private Rigidbody2D rg2d;
    private float verticalVelocity;
    private bool isJumping;
    private float direction;
    private bool right, left;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();

        rg2d.gravityScale = gravity;
    }

    private void Update()
    {
        ReadInputs();
    }

    private void FixedUpdate()
    {
        MoveCharacterOnXaxis();
        FallMoreQuickAfterJump();
    }

    private void ReadInputs()
    {
        if (!Application.isMobilePlatform)
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
                Jump();

            direction = Input.GetAxisRaw("Horizontal");
        }

        else
        {
            if (right) direction = 1;
            else if (left) direction = -1;
            else direction = 0;
        }
        
    }

    private void MoveCharacterOnXaxis()
    {
        rg2d.velocity = new Vector2(direction * runVelocity, rg2d.velocity.y);

        anim.SetFloat("speed", Mathf.Abs(rg2d.velocity.x));
    }

    private void Jump()
    {
        verticalVelocity = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * rg2d.gravityScale * jumpHeight);

        rg2d.velocity = new Vector2(direction * runVelocity, verticalVelocity);

    }

    private void FallMoreQuickAfterJump()
    {
        if (rg2d.velocity.y < 0)
        {
            rg2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    /// <summary>
    /// Check if is jumping or not
    /// </summary>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        isJumping = false;
        anim.SetBool("grounded", true);
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        isJumping = true;
        anim.SetBool("grounded", false);
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        isJumping = false;
        anim.SetBool("grounded", true);
    }

    /// <summary>
    /// This methods are for inputs on mobile platform
    /// </summary>

    #region
    public void DownRight()
    {
        right = true;
    }

    public void DownLeft()
    {
        left = true;
    }

    public void UpRight()
    {
        right = false;
    }

    public void UpLeft()
    {
        left = false;
    }

    public void MobileJumpInput()
    {
        if (!isJumping) Jump();
    }
#endregion
}
