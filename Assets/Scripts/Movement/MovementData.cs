using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Data")]

public class MovementData : ScriptableObject
{
    [Header("Movement")]
    public float runMaxSpeed;

    public float runAccelAmount;

    public float runDecelAmount;

    public bool ConserveMomentum;


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

    [Header("Jumping")]
    [Space(5)]
    public float minJumpForce;

    public float jumpforce;

    public bool hasJumped = false;

    public bool jumping;

    public float gravity;

    public float InitialPlayerYHeight;

    public float MaxJumpHeight;

    public float MaxHeight;

    [Header("Dashing")]
    [Space(5)]
    public bool canDash = true;

    public float dashSpeed;

    public float dashTime;

    public float dashCooldown;

    public float knockbackForce = 10f;

    [Header("WallJump & WallSlide")]
    [Space(5)]
    public float WallJumpCooldown;

    public bool canWallJump = true;

    public bool latchedToWall;

    public float WallSlidingSpeed = 2f;

    public float WallJumpDuration = 1f;

    public bool isWallJumping;

    public float wallJumpX;

    public float wallJumpY;
}
