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

    public float wallJumpRunLerp;

    public float dashEndRunLerp;

    public bool ConserveMomentum;

    [Range(0.01f, 0.5f)] public float CoyoteTime;

    [Header("Collision Raycasts (DONT TOUCH)")]
    [Space(5)]
    [Range(0f, 100f)] public float groundcastDistance = 0.7f;
    [Space(5)]
    [Range(0f, 100f)] public float kbcastDistance = 1.5f;
    [Space(5)]
    [Range(0f, 100f)] public float WJcastDistance = 0.7f;

    [Header("Layers & Tags")]
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsWall;
    public LayerMask whatIsWJW;

    [Header("Gravity")]
    [Space(5)]
    public float baseGravityScale;
    public float fallGravityMult;
    public float jumpHangGravityMult;
    public float jumpCutGravityMult;
    public float maxFallSpeed;



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

    public float jumpHangTimeThreshold;

    [Header("Dashing")]
    [Space(5)]
    public bool canDash = true;

    public float dashSpeed;

    public float dashTime;

    public bool dashing;

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

    public float wallJumpTime;

    public float wallJumpX;

    public float wallJumpY;
}
