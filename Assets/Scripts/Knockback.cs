using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackWorking : MonoBehaviour
{
    public float knockbackForce = 10f; // The force of the knockback
    public float knockbackTime = 0.2f; // The duration of the knockback
    public float wallJumpForce = 5f;
    public float wallJumpTime = 0.2f;
    private float knockbackTimer; // Timer for the knockback duration
    public bool isKnockedBack = false; // Flag to indicate whether the object is currently being knocked back
    public bool hasWallJumped = false;

    private Rigidbody2D rb; // The rigidbody component of the object


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
        }
    }

    public void ApplyKnockback(float xdir)
    {
        Debug.Log("Knocking back");


        Vector2 direction = new Vector2(xdir, 1);
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        isKnockedBack = true;
        knockbackTimer = knockbackTime;

    }
    public void ApplyWallJump(float xdir)
    {
        Debug.Log("Knocking back");


        Vector2 direction = new Vector2(xdir, 1);
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * wallJumpForce, ForceMode2D.Impulse);

        hasWallJumped = true;
        knockbackTimer = wallJumpTime;
    }
}