using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    #region PLAYER STATES

    //Timers for player states

    private float LastOnGroundTime;
    private float LastOnWallTime;
    private float LastOnWallRightTime;
    private float LastOnWallLeftTime;
    private float LastJumpPressedTime;
    private float LastPressedDashTime;
    private float wallJumpStartTime;

    //States for player jump cancel
    private bool isJumpCut;
    private bool isJumpFalling;

    #endregion

    #region COMPONENTS
    private Rigidbody2D rb;
    public Rigidbody2D PlayerBody;

    public SpriteRenderer Player;

    public MovementData Data;
    #endregion

    #region START
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region UPDATE
    private void FixedUpdate()
    {
        if (!Data.dashing)
        {
            if (!Data.isWallJumping)
            {
                Movement(1);
            }
            else
            {
                Movement(Data.wallJumpRunLerp);
            }
        }
        else
        {
            Movement(Data.dashEndRunLerp);
        }

        startDash();
    }

    private void Update()
    {

        Timers();
        CollisionChecks();
        DashChecks();
        Gravity();
        JumpChecks();

        if (!Data.isWallJumping)
        {
            Jump();
        }
        WallSliding();
        WallJump();
    }
    #endregion

    #region GRAVITY

    private void Gravity()
    {
        if (!Data.dashing)
        {
            if (rb.velocity.y < 0)
            {
                SetGravityscale(Data.baseGravityScale * Data.fallGravityMult);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFallSpeed));
            }
            else if (isJumpCut)
            {
                SetGravityscale(-Data.baseGravityScale * Data.jumpCutGravityMult);
            }
            else if (Data.jumping || Data.isWallJumping || isJumpFalling && Mathf.Abs(rb.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityscale(Data.baseGravityScale * Data.jumpHangGravityMult);
            }
            else
            {
                SetGravityscale(Data.baseGravityScale);
            }
        }
        else
        {
            SetGravityscale(0);
        }
    }

    public void SetGravityscale(float scale)
    {
        rb.gravityScale = scale;
    }

    #endregion

    #region RUN
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

    #region DASH CHECKS

    private void DashChecks()
    {
        Data.dashing = true;
        Data.jumping = false;
        Data.isWallJumping = false;
        isJumpCut = false;

        StartCoroutine(nameof(startDash));
    }

    #endregion

    #region DASH
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

        Data.dashing = false;

        //yield return new WaitForSeconds(dashTime);

        yield return new WaitForSeconds(Data.dashCooldown);
        Data.canDash = true;
    }
    #endregion

    #region JUMP CHECKS
    private void JumpChecks()
    {
        if (Data.jumping && rb.velocity.y < 0)
        {
            Data.jumping = false;

            isJumpFalling = true;
        }
        if (Data.isWallJumping && Time.time - wallJumpStartTime > Data.wallJumpTime)
        {
            Data.isWallJumping = false;
        }
        if(LastOnGroundTime > 0  && !Data.jumping && !Data.isWallJumping)
        {
            isJumpCut = false;
            isJumpFalling = false;
        }
        if (!Data.dashing)
        {
            if (canJump() && LastJumpPressedTime > 0)
            {
                Data.jumping = true;
                Data.isWallJumping = false;
                isJumpCut = false;
                isJumpFalling = false;
                Jump();
            }
            else if (canWallJump() && LastJumpPressedTime > 0)
            {
                Data.isWallJumping = true;
                Data.jumping = false;
                isJumpCut = false;
                isJumpFalling = false;

                wallJumpStartTime = Time.time;
                WallJump();
            }
        }
    }
    #endregion

    #region  JUMP

    private void Jump()
    {


        var jumpInput = Input.GetButtonDown("Jump");
        var jumpInputReleased = Input.GetButtonUp("Jump");


        if (jumpInput)
        {
            LastJumpPressedTime = 0;
            LastOnGroundTime = 0;
            //Data.InitialPlayerYHeight = transform.position.y;
            //Data.MaxJumpHeight = Data.InitialPlayerYHeight + Data.MaxHeight;
            Debug.Log("Jump Registered");

            float force = Data.jumpforce;
            if(rb.velocity.y  < 0)
            {
                force -= rb.velocity.y;
            }

            rb.velocity = new Vector2(rb.velocity.x, force);
            Data.hasJumped = true;          
        }
        //else if (jumpInputReleased && rb.velocity.y > 0 && !Data.latchedToWall && !Data.isWallJumping|| rb.velocity.y < 0 && !Data.latchedToWall && !Data.isWallJumping|| transform.position.y > Data.MaxJumpHeight && !Data.isWallJumping && !Data.latchedToWall)
        //{
        //    Debug.Log("falling");

        //    rb.velocity = new Vector2(rb.velocity.x, 0);

        //    if (!Data.latchedToWall)
        //    {

        //        rb.gravityScale = Data.gravity;
        //    }

        //}



    }
    #endregion

    #region  WALLSLIDE
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

    #region  WALLJUMP
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

    #region CONDITIONS

    public bool IsGrounded()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Data.groundcastDistance, Data.whatIsGround);
        return hit.collider != null;
    }

    private bool canJump()
    {
        return LastOnGroundTime > 0 && !Data.jumping;
    }

    private bool canWallJump()
    {
        return LastJumpPressedTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && !Data.isWallJumping;
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

    #region TIMERS
    private void Timers()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
    }
    #endregion

    #region COLLISION CHECKS
    private void CollisionChecks()
    {
        if(!Data.jumping && !Data.dashing)
        if (IsGrounded())
        {
            LastOnGroundTime = Data.CoyoteTime;
        }
        if (IsWallJumpWallLeft())
        {
            LastOnWallLeftTime = Data.CoyoteTime;
        }
        if (IsWallJumpWallRight())
        {
            LastOnWallRightTime = Data.CoyoteTime;
        }
        LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
    }
    #endregion
}