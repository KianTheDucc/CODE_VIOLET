using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Components
    private Rigidbody2D rb;
    public Rigidbody2D PlayerBody;

    public SpriteRenderer Player;

    public MovementData Data;
    #endregion

    #region OnStart
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region Update
    private void FixedUpdate()
    {

        if (!Data.isWallJumping)
        {
            Movement();
        }
        startDash();
    }

    private void Update()
    {
        if (IsGrounded())
        {
            rb.gravityScale = 1;
        }

        if (!Data.isWallJumping)
        {
            Jump();
        }
        WallSliding();
        WallJump();
    }
    #endregion

    #region Run
    private void Movement()
    {
        float xDir = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            Data.jumping = false;
            Data.hasJumped = false;
            GetComponent<KnockbackWorking>().hasWallJumped = false;
        }

        if (!IsAgainstWallLeft() && !IsAgainstWallRight() && !GetComponent<KnockbackWorking>().isKnockedBack && !IsWallJumpWallLeft() && !IsWallJumpWallRight())
        {
            rb.velocity = new Vector2(xDir * (Data.movementforce * Time.deltaTime), rb.velocity.y);
            rb.velocity.Normalize();
        }
        else if (IsAgainstWallLeft() && xDir != -1 || IsWallJumpWallLeft() && xDir != -1)
        {
            rb.velocity = new Vector2(xDir * (Data.movementforce * Time.deltaTime), rb.velocity.y);
            rb.velocity.Normalize();
        }
        else if (IsAgainstWallRight() && xDir != 1 || IsWallJumpWallRight() && xDir != 1)
        {
            rb.velocity = new Vector2(xDir * (Data.movementforce * Time.deltaTime), rb.velocity.y);
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
    #endregion

    #region Dash
    private void startDash()
    { 
        float xDir = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Dash") && Data.canDash)
        {
            Debug.Log("dashing");

            StartCoroutine(Dash(xDir));
        }
    }


    public IEnumerator Dash(float xDir)
    {
        Data.canDash = false;

        rb.velocity = new Vector2(xDir * Data.dashSpeed, rb.velocity.y);

        //yield return new WaitForSeconds(dashTime);

        yield return new WaitForSeconds(Data.dashCooldown);
        Data.canDash = true;
    }
    #endregion

    #region  Jump

    private void Jump()
    {
        var jumpInput = Input.GetButtonDown("Jump");
        var jumpInputReleased = Input.GetButtonUp("Jump");


        if (jumpInput)
        {

            if (IsGrounded() && !Data.hasJumped && !GetComponent<KnockbackWorking>().hasWallJumped)
            {
                Data.InitialPlayerYHeight = transform.position.y;
                Data.MaxJumpHeight = Data.InitialPlayerYHeight + Data.MaxHeight;
                Debug.Log("Jump Registered");

                rb.velocity = new Vector2(rb.velocity.x, Data.jumpforce);
                Data.hasJumped = true;

            }
        }
        else if (jumpInputReleased && rb.velocity.y > 0 && !Data.latchedToWall && !Data.isWallJumping|| rb.velocity.y < 0 && !Data.latchedToWall && !Data.isWallJumping|| transform.position.y > Data.MaxJumpHeight && !Data.isWallJumping && !Data.latchedToWall)
        {
            Debug.Log("falling");

            rb.velocity = new Vector2(rb.velocity.x, 0);

            if (!Data.latchedToWall)
            {

                rb.gravityScale = Data.gravity;
            }

        }



    }
    #endregion

    #region  WallSlide
    private void WallSliding()
    {
        var xDir = Input.GetAxisRaw("Horizontal");

        if (IsWallJumpWall()  && !IsGrounded() && xDir != 0)
        {
            Data.latchedToWall = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Data.WallSlidingSpeed, float.MaxValue));
        }
        else
        {
            Data.latchedToWall = false;
        }
    }
    #endregion

    #region  WallJump
    public IEnumerator wallJumping()
    {
        Data.isWallJumping = true;
        yield return new WaitForSeconds(Data.WallJumpDuration);
        Data.isWallJumping = false;

    }

    private void WallJump()
    {
        var xDir = Input.GetAxisRaw("Horizontal");
        var WJInput = Input.GetButtonDown("Jump");

        if (xDir == -1)
        {
            xDir = 1;
        }
        else if(xDir == 1)
        {
            xDir = -1;
        }

        if (IsWallJumpWall() && WJInput && Data.latchedToWall) 
        {
            GetComponent<KnockbackWorking>().hasWallJumped = true;
            Data.InitialPlayerYHeight = transform.position.y;
            Data.MaxJumpHeight = Data.InitialPlayerYHeight + Data.MaxHeight;
            Data.canWallJump = false;
            rb.velocity = new Vector2(xDir * Data.wallJumpX, Data.wallJumpY);
            StartCoroutine(wallJumping());
            Data.canWallJump = true;
        }
    }


    public IEnumerator wallJumpCooldownTimer()
    {
        yield return new WaitForSeconds(Data.WallJumpCooldown);
        Data.canWallJump = true;
    }
    #endregion

    #region Conditions

    public bool IsGrounded()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Data.groundcastDistance, Data.whatIsGround);
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Data.WJcastDistance, Data.whatIsWall);
        return hit.collider != null;
    }

    public bool IsWallJumpWallLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Data.WJcastDistance, Data.whatIsWJW);
        return hit.collider != null;
    }
    public bool IsWallJumpWallRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Data.WJcastDistance, Data.whatIsWJW);
        return hit.collider != null;
    }

    public bool IsAgainstWallRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Data.WJcastDistance, Data.whatIsWall);
        return hit.collider != null;
    }
    public bool IsAgainstEnemyRight()
    {
        Debug.Log("HiT! R");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Data.kbcastDistance, Data.whatIsEnemy);
        return hit.collider != null;
    }
    public bool IsAgainstEnemyLeft()
    {
        Debug.Log("HiT L!");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Data.kbcastDistance, Data.whatIsEnemy);
        return hit.collider != null;
    }
    #endregion
}