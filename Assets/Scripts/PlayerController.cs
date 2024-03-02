using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movementforce;


    [Space(5)]
    [Range(0f, 100f)] public float groundcastDistance = 1.5f;
    [Space(5)]
    [Range(0f, 100f)] public float kbcastDistance = 1.5f;
    [Space(5)]
    [Range(0f, 100f)] public float WJcastDistance = 1.5f;

    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsWall;
    public LayerMask whatIsWJW;

    private Rigidbody2D rb;
    public Rigidbody2D PlayerBody;

    public SpriteRenderer Player;

    public float minJumpForce;

    public float jumpforce;

    public bool hasJumped = false;

    public bool jumping;

    public bool canDash = true;

    public float dashSpeed;

    public float dashTime;

    public float dashCooldown;

    public float knockbackForce = 10f;

    public float gravity;

    public float WallJumpCooldown;

    public bool canWallJump = true;

    public Vector2 _vecGravity;

    public float InitialPlayerYHeight;

    public float MaxJumpHeight;

    public float MaxHeight;

    public bool latchedToWall;

    public float WallSlidingSpeed = 2f;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        Movement();

        

        

        startDash();
    }

    private void Update()
    {
        if (IsGrounded())
        {
            rb.gravityScale = 1;
        }

        Jump();

        WallSliding();

        float angleIncrement = 1f;
        for (float angle = 0f; angle < 360f; angle += angleIncrement)
        {
            float angleRadians = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, kbcastDistance, whatIsEnemy);
            if (hit.collider != null)
            {
                Debug.Log("Raycast hit " + hit.collider.gameObject.name);
                KnockbackCheck(hit.collider.gameObject);
                // Checks for an entity to knock back
            }
        }

    }

    private void Movement()
    {
        float xDir = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            jumping = false;
            hasJumped = false;
            GetComponent<KnockbackWorking>().hasWallJumped = false;
        }

        if (!IsAgainstWallLeft() && !IsAgainstWallRight() && !GetComponent<KnockbackWorking>().isKnockedBack && !IsWallJumpWallLeft() && !IsWallJumpWallRight())
        {
            rb.velocity = new Vector2(xDir * (movementforce * Time.deltaTime), rb.velocity.y);
            rb.velocity.Normalize();
        }
        else if (IsAgainstWallLeft() && xDir != -1 || IsWallJumpWallLeft() && xDir != -1)
        {
            rb.velocity = new Vector2(xDir * (movementforce * Time.deltaTime), rb.velocity.y);
            rb.velocity.Normalize();
        }
        else if (IsAgainstWallRight() && xDir != 1 || IsWallJumpWallRight() && xDir != 1)
        {
            rb.velocity = new Vector2(xDir * (movementforce * Time.deltaTime), rb.velocity.y);
            rb.velocity.Normalize();
        }

        if (xDir == -1)
        {
            Player.flipX = true;
        }
        else if (xDir == 1)
        {
            Player.flipX = false;
        }

    }
    private void startDash()
    { 
        float xDir = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Dash") && canDash)
        {
            Debug.Log("dashing");

            StartCoroutine(Dash(xDir));
        }
    }


    public IEnumerator Dash(float xDir)
    {
        canDash = false;

        rb.velocity = new Vector2(xDir * dashSpeed, rb.velocity.y);

        //yield return new WaitForSeconds(dashTime);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    private void Jump()
    {
        var jumpInput = Input.GetButtonDown("Jump");
        var jumpInputReleased = Input.GetButtonUp("Jump");


        if (jumpInput)
        {

            if (IsGrounded() && !hasJumped && !GetComponent<KnockbackWorking>().hasWallJumped)
            {
                InitialPlayerYHeight = transform.position.y;
                MaxJumpHeight = InitialPlayerYHeight + MaxHeight;
                Debug.Log("Jump Registered");

                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
                hasJumped = true;

            }
        }
        else if (jumpInputReleased && rb.velocity.y > 0 && !latchedToWall|| rb.velocity.y < 0 && !latchedToWall || transform.position.y > MaxJumpHeight)
        {
            Debug.Log("falling");

            rb.velocity = new Vector2(rb.velocity.x, 0);

            if (!latchedToWall)
            {

                rb.gravityScale = gravity;
            }

        }



    }   

    public IEnumerator GravityStall()
    {
        yield return new WaitForSeconds(1);
        rb.gravityScale = gravity;
    }

    private void WallSliding()
    {
        var xDir = Input.GetAxisRaw("Horizontal");

        if (IsWallJumpWall()  && !IsGrounded() && xDir != 0)
        {
            latchedToWall = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -WallSlidingSpeed, float.MaxValue));
        }
        else
        {
            latchedToWall = false;
        }
    }

    //private void WallJump()
    //{


    //    var jumpInput = Input.GetButtonDown("Jump");
    //    var jumpInputReleased = Input.GetButtonUp("Jump");

    //    if (jumpInput)
    //    {
    //        if (IsWallJumpWallLeft() && canWallJump)
    //        {
    //            canWallJump = false;
    //            rb.velocity = new Vector2(rb.velocity.x  * -1, jumpforce);
    //            GetComponent<KnockbackWorking>().ApplyWallJump(1);
    //            StartCoroutine(wallJumpCooldownTimer());
    //        }
    //        else if (IsWallJumpWallRight() && canWallJump)
    //        {
    //            canWallJump = false;
    //            rb.velocity = new Vector2(rb.velocity.x *1, jumpforce);
    //            GetComponent<KnockbackWorking>().ApplyWallJump(1);
    //            StartCoroutine(wallJumpCooldownTimer());
    //        }
    //    }
    //    else if(jumpInputReleased && rb.velocity.y  > 0)
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, 0);
    //        rb.velocity.Normalize();
    //    }
    //}


    public IEnumerator wallJumpCooldownTimer()
    {
        yield return new WaitForSeconds(WallJumpCooldown);
        canWallJump = true;
    }


    public bool IsGrounded()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundcastDistance, whatIsGround);
        return hit.collider != null;
    }

    public bool IsWallJumpWall()
    {
        if  (IsWallJumpWallLeft()  || IsWallJumpWallRight())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAgainstWallLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, WJcastDistance, whatIsWall);
        return hit.collider != null;
    }

    public bool IsWallJumpWallLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, WJcastDistance, whatIsWJW);
        return hit.collider != null;
    }
    public bool IsWallJumpWallRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, WJcastDistance, whatIsWJW);
        return hit.collider != null;
    }

    public bool IsAgainstWallRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, WJcastDistance, whatIsWall);
        return hit.collider != null;
    }
    public bool IsAgainstEnemyRight()
    {
        Debug.Log("HiT! R");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, kbcastDistance, whatIsEnemy);
        return hit.collider != null;
    }
    public bool IsAgainstEnemyLeft()
    {
        Debug.Log("HiT L!");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, kbcastDistance, whatIsEnemy);
        return hit.collider != null;
    }


    private void KnockbackCheck(GameObject enemy)
    {
        float xdir = 0;
        Rigidbody2D enemyrb = enemy.GetComponent<Rigidbody2D>();
        if (enemyrb.rotation >= 90)
        {
            xdir = -1;
        }
        else if (enemyrb.rotation < 90)
        {
            xdir = 1;
        }
        if (enemy.CompareTag("enemy"))
        {
            GetComponent<KnockbackWorking>().ApplyKnockback(xdir);
            GetComponent<CombatScript>().DamagePlayer(10);
        }


    }
}