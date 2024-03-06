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




    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsWall;
    public LayerMask whatIsWJW;

    [Header("Jumping")]
    [Space(5)]
    public float minJumpForce;

    public float jumpforce;

    public float MaxHeight;

    public float maxFallSpeed;

    public float gravity;

    [Header("Dashing")]
    [Space(5)]

    public float dashSpeed;

    public float dashTime;

    public float dashCooldown;

    [Header("WallJump & WallSlide")]
    [Space(5)]

    public float WallSlidingSpeed = 2f;

    public float wallJumpX;

    public float wallJumpY;

    [Header("don't touch")]

    [Space(5)]
    [Range(0f, 100f)] public float groundcastDistance = 0.7f;
    [Space(5)]
    [Range(0f, 100f)] public float kbcastDistance = 1.5f;
    [Space(5)]
    [Range(0f, 100f)] public float WJcastDistance = 0.7f;

    public bool hasJumped = false;

    public bool jumping;

    public float InitialPlayerYHeight;

    public float MaxJumpHeight;

    public bool dashing;

    public bool canDash = true;

    public bool canWallJump = true;

    public bool latchedToWall;

    public bool isWallJumping;

    public float WallJumpCooldown;
}
