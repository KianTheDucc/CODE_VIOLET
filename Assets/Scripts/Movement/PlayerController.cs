using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    #region Components
    private Rigidbody2D rb;
    public Rigidbody2D PlayerBody;

    public SpriteRenderer Player;

    public MovementData Data;

    public GameObject GroundCheck;
    #endregion

    #region OnStart
    private void Start()
    {
        CoyoteTime();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {

        string spawnLocation = PlayerPrefs.GetString("SpawnLocation");

        Debug.Log(spawnLocation);
        if (spawnLocation != "none" && GameEventsManager.instance.oldscene.buildIndex != 0)
        {
            print("transporting player");

            StartCoroutine(teleportOnSceneChange(spawnLocation));
        }
    }

    private IEnumerator teleportOnSceneChange(string spawnLocation)
    {
        yield return new WaitForSeconds(0.002f);
        transform.position = GameObject.Find(spawnLocation).transform.position;
    }
    #endregion

    #region Update
    private void FixedUpdate()
    {

        if (!Data.dashing)
        {
            Movement(1);
            startDash();
        }
    }

    private void Update()
    {
        Timers();

        if (rb.velocity.y < 0 && !Data.latchedToWall && !GetComponent<KnockbackWorking>().hasWallJumped)
        {
            //Sets the gravity for when the player is falling
            rb.gravityScale = Data.gravity;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFallSpeed));
        }
        CoyoteTime();

        if (IsGrounded())
        {

            //Resets gravity if on the ground
            rb.gravityScale = 1;
            GetComponent<KnockbackWorking>().hasWallJumped = false;
        }



        if (!Data.dashing)
        {
            Jump();
            WallSliding();
            WallJump();
        }

    }
    #endregion

    #region Coyote Time

    public void CoyoteTime()
    {
        if (IsGrounded())
        {
            //Sets the ground timer to coyote time for more forgiving jumps
            Data.LastOnGroundTime = Data.CoyoteTime;
        }
        if (IsWallJumpWall())
        {
            //Sets the WallJumpTimer to coyote time for more forgiving wall jumps
            Data.LastOnWallTime = Data.CoyoteTime;
        }
    }

    #endregion

    #region PlayerState Timers

    public void Timers()
    {
        Data.LastOnGroundTime -= Time.deltaTime;
        Data.LastOnWallTime -= Time.deltaTime;
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
            // the top speed for the movement
            float targetSpeed = xDir * Data.runMaxSpeed;

            targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

            #region Calculate Acceleration
            float acceleration;

            //Calculates the Acceleration/Decceleration of the player 
            acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDecelAmount;
            #endregion
            #region Conserve Momentum

            if (Data.ConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && IsGrounded())
            {
                acceleration = 0;
            }
            #endregion

            //Difference between current speed and top speed
            float speedDif = targetSpeed - rb.velocity.x;


            //The force to add to the player each frame
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

        Data.dashing = true;

        rb.gravityScale = 0;

        rb.velocity = new Vector2(xDir * Data.dashSpeed, 0);

        yield return new WaitForSeconds(Data.dashTime);

        rb.velocity = new Vector2(0, 0);

        rb.gravityScale = Data.gravity;

        Data.dashing = false;

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

            if (Data.LastOnGroundTime > 0 && !Data.hasJumped && !GetComponent<KnockbackWorking>().hasWallJumped)
            {
                Data.LastOnGroundTime = 0;
                Data.InitialPlayerYHeight = transform.position.y;
                Data.MaxJumpHeight = Data.InitialPlayerYHeight + Data.MaxHeight;
                Debug.Log("Jump Registered");

                rb.velocity = new Vector2(rb.velocity.x, Data.jumpforce);
                Data.hasJumped = true;

            }
        }
        else if (
            (jumpInputReleased && rb.velocity.y > 0 && !Data.latchedToWall && !GetComponent<KnockbackWorking>().hasWallJumped) || 
            (rb.velocity.y < 0 && !Data.latchedToWall && !GetComponent<KnockbackWorking>().hasWallJumped) || 
            (transform.position.y > Data.MaxJumpHeight && !GetComponent<KnockbackWorking>().hasWallJumped && !Data.latchedToWall)
            )
        {
            if (!Data.latchedToWall && !Data.isWallJumping)
            {
                //rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.gravityScale = Data.gravity;
            }

        }



    }
    #endregion

    #region  WallSlide
    private void WallSliding()
    {
        var xDir = Input.GetAxisRaw("Horizontal");

        //Checks that the player isn't grounded and it is a wall they can slide on
        if (IsWallJumpWall() && !IsGrounded() && xDir != 0 && rb.velocity.y < 0)
        {
            
            Data.latchedToWall = true;

            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * Data.WallSlidingSpeed);
        }
        else
        {
            Data.latchedToWall = false;
        }
    }
    #endregion

    #region  WallJump
    //public IEnumerator wallJumping()
    //{
    //    Data.isWallJumping = true;
    //    yield return new WaitForSeconds(Data.WallJumpDuration);
    //    Data.isWallJumping = false;

    //}
    #region Perform Walljump
    private void WallJump()
    {
        int dir = 0;
        var WJInput = Input.GetButtonDown("Jump");

        //gets the direction to wall jump
        if (IsWallJumpWallLeft())
        {
            dir = 1;
        }
        else if (IsWallJumpWallRight())
        {
            dir = -1;
        }

        //defining what speed in both directions to make the player
        Vector2 force = new Vector2(Data.wallJumpX, Data.wallJumpY);

        //Makes the force apply in the required direction
        force.x *= dir;

        if (Data.LastOnWallTime > 0 && WJInput)
        {
            Data.LastOnWallTime = 0;

            if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
            {
                force.x -= rb.velocity.x;
            }


            //if (rb.velocity.y < 0 && !Data.latchedToWall)
            //{
            //    force.y -= rb.velocity.y;
            //}

            GetComponent<KnockbackWorking>().hasWallJumped = true;

            Data.InitialPlayerYHeight = transform.position.y;

            Data.MaxJumpHeight = Data.InitialPlayerYHeight + Data.MaxHeight;

            Data.canWallJump = false;

            rb.velocity = force;

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

        return Physics2D.OverlapBox(GroundCheck.transform.position, new Vector2(1, 1), 0f, Data.whatIsGround);
    }

    public bool IsWallJumpWall()
    {
        if (IsWallJumpWallLeft() || IsWallJumpWallRight())
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

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("SpawnLocation", "none");
    }
}