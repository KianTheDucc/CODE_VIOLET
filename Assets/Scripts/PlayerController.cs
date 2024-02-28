using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movementforce;
    public float jumpforce;

    [Space(5)]
    [Range(0f, 100f)] public float groundcastDistance = 1.5f;
    [Space(5)]
    [Range(0f, 100f)] public float kbcastDistance = 1.5f;

    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;

    private Rigidbody2D rb;
    public Rigidbody2D PlayerBody;

    public SpriteRenderer Player;

    public bool hasJumped = false;

    public float knockbackForce = 10f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();

        Jump();

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
        {
            hasJumped = false;
            GetComponent<KnockbackWorking>().hasWallJumped = false;
        }
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

        if (!IsAgainstWallLeft() && !IsAgainstWallRight() && !GetComponent<KnockbackWorking>().isKnockedBack)
        {
            rb.velocity = new Vector2(xDir * (movementforce * Time.deltaTime), rb.velocity.y);
        }
        else if (IsAgainstWallLeft() && xDir != -1)
        {
            rb.velocity = new Vector2(xDir * (movementforce * Time.deltaTime), rb.velocity.y);
        }
        else if (IsAgainstWallRight() && xDir != 1)
        {
            rb.velocity = new Vector2(xDir * (movementforce * Time.deltaTime), rb.velocity.y);
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
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {

            if (IsGrounded() && !hasJumped && !GetComponent<KnockbackWorking>().hasWallJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                hasJumped = true;
                //GetComponent<KnockbackWorking>().hasWallJumped = true;

            }
            else if (IsAgainstWallLeft() && !hasJumped && !GetComponent<KnockbackWorking>().hasWallJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                //GetComponent<KnockbackWorking>().ApplyWallJump(1);
                hasJumped = true;
                //GetComponent<KnockbackWorking>().hasWallJumped = true;

            }
            else if (IsAgainstWallLeft() && hasJumped && !GetComponent<KnockbackWorking>().hasWallJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                GetComponent<KnockbackWorking>().ApplyWallJump(1);
                hasJumped = true;
                GetComponent<KnockbackWorking>().hasWallJumped = true;

            }
            else if (IsAgainstWallRight() && !hasJumped && !GetComponent<KnockbackWorking>().hasWallJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                //GetComponent<KnockbackWorking>().ApplyWallJump(-1);
                hasJumped = true;
                //GetComponent<KnockbackWorking>().hasWallJumped = true;
            }
            else if (IsAgainstWallRight() && hasJumped && !GetComponent<KnockbackWorking>().hasWallJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                GetComponent<KnockbackWorking>().ApplyWallJump(-1);
                hasJumped = true;
                GetComponent<KnockbackWorking>().hasWallJumped = true;
            }
        }



    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundcastDistance, whatIsGround);
        return hit.collider != null;
    }

    public bool IsAgainstWallLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, groundcastDistance, whatIsGround);
        return hit.collider != null;
    }
    public bool IsAgainstWallRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, groundcastDistance, whatIsGround);
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
        //Vector2 direction = (transform.position - enemy.transform.position).normalized;
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
    //private void Knockback()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, kbcastDistance, whatIsEnemy);
    //    RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, kbcastDistance, whatIsEnemy);
    //    if (hit)
    //    {
    //        Debug.Log("Hit! Left");
    //        DamagePlayer(10);

    //        Vector2 force = new Vector2(1 * ((knockbackForce * 10) * Time.deltaTime), 0 * (knockbackForce * Time.deltaTime));
    //        PlayerBody.AddForce(force, ForceMode2D.Impulse);
    //    }

    //    else if (hit2)
    //    {
    //        Debug.Log("Hit! Right");
    //        DamagePlayer(10);
    //        Vector2 force = new Vector2(knockbackForce * 10 * Time.deltaTime, knockbackForce * Time.deltaTime);
    //        PlayerBody.AddForce(force, ForceMode2D.Impulse);
    //    }
    //}
}