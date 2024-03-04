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
            Movement(1);
        }
        startDash();
    }

    private void Update()
    {
        if (IsGrounded())
        {
            rb.gravityScale = 1;
            GetComponent<KnockbackWorking>().hasWallJumped = false;
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
    private void Movement(float lerpAmount)
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

            float targetSpeed = xDir * Data.runMaxSpeed;

            targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

            #region Calculate Acceleration
            float acceleration;

            acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDecelAmount;
            #endregion
            #region Conserve Momentum

            if (Data.ConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && IsGrounded())
            {
                acceleration = 0;
            }
            #endregion
            float speedDif = targetSpeed - rb.velocity.x;

            float movement = speedDif * acceleration;

            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
        else if (IsAgainstWallLeft() && xDir != -1 || IsWallJumpWallLeft() && xDir != -1)
        {
            float targetSpeed = xDir * Data.runMaxSpeed;

            targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

            #region Calculate Acceleration
            float acceleration;

            acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDecelAmount;
            #endregion
            #region Conserve Momentum

            if (Data.ConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && IsGrounded())
            {
                acceleration = 0;
            }
            #endregion
            float speedDif = targetSpeed - rb.velocity.x;

            float movement = speedDif * acceleration;

            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
        else if (IsAgainstWallRight() && xDir != 1 || IsWallJumpWallRight() && xDir != 1)
        {
            float targetSpeed = xDir * Data.runMaxSpeed;

            targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

            #region Calculate Acceleration
            float acceleration;

            acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDecelAmount;
            #endregion
            #region Conserve Momentum

            if (Data.ConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && IsGrounded())
            {
                acceleration = 0;
            }
            #endregion
            float speedDif = targetSpeed - rb.velocity.x;

            float movement = speedDif * acceleration;

            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
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
        else if (jumpInputReleased && rb.velocity.y > 0 && !Data.latchedToWall && !GetComponent<KnockbackWorking>().hasWallJumped|| rb.velocity.y < 0 && !Data.latchedToWall && !GetComponent<KnockbackWorking>().hasWallJumped || transform.position.y > Data.MaxJumpHeight && !GetComponent<KnockbackWorking>().hasWallJumped && !Data.latchedToWall)
        {
            Debug.Log("falling");



            if (!Data.latchedToWall && !Data.isWallJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
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
    #region Perform Walljump
    private void WallJump()
    {
        int dir = 0;
        var WJInput = Input.GetButtonDown("Jump");

        if (IsWallJumpWallLeft())
        {
            dir = 1;
        }
        else if(IsWallJumpWallRight())
        {
            dir = -1;
        }
        Vector2 force = new Vector2(Data.wallJumpX, Data.wallJumpY);

        force.x *= dir;
        if (IsWallJumpWall() && WJInput) 
        {
            if(Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
            {
                force.x -= rb.velocity.x;
            }

            if(rb.velocity.y < 0)
            {
                force.y -= rb.velocity.y;
            }

            GetComponent<KnockbackWorking>().hasWallJumped = true;
            Data.InitialPlayerYHeight = transform.position.y;
            Data.MaxJumpHeight = Data.InitialPlayerYHeight + Data.MaxHeight;
            Data.canWallJump = false;
            rb.AddForce(force, ForceMode2D.Impulse);
            //StartCoroutine(wallJumping());
            Data.canWallJump = true;
        }
    }
    #endregion

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