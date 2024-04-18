using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private GameObject Player;
    private GameObject Boss;
    public GameObject BossAttack;
    public GameObject BossLaser;
    public GameObject groundCheck;

    public LayerMask groundMask;

    public bool BossAttacking;

    public bool isGrounded;

    private int possibleBossChoices;
    private float BossChoice;

    public float SlamJumpHeight;
    public float SlamTelegraphTime;
    public float MeleeTelegraphTime;
    public float laserTelegraphTime;
    public float bossCooldownTime;

    [Header("For JumpAttack")]
    [SerializeField] private Vector2 BoxSize;

    [Header("For Laser")]
    [SerializeField] private float power;

    Rigidbody2D rb;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Boss = this.gameObject;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        BossBehaviourTree();        
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, BoxSize,0, groundMask);
        if (Boss.transform.position.x == Player.transform.position.x && BossAttacking)
        {
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, -SlamJumpHeight), ForceMode2D.Impulse);
        }
    }

    public void BossBehaviourTree()
    {
        if (!BossAttacking && isGrounded)
        {


            Vector2 playerdis = playerDistance();
            float disx, disy;
            disx = playerdis.x;
            disy = playerdis.y;
            float dis = playerdis.x + playerdis.y;

            if (dis < 5)
            {
                possibleBossChoices = 2;
            }
            else
            {
                possibleBossChoices = 1;
            }
            System.Random rand = new System.Random();
            BossChoice = rand.Next(0, possibleBossChoices);

            switch (BossChoice)
            {
                case 0:
                    Debug.Log("1");
                    StartCoroutine(SlamAttack());
                    break;
                case 1:
                    BossAttacking = true;
                    Debug.Log("2");
                    StartCoroutine(laserAttack());
                    break;
                case 2:
                    Debug.Log("3");
                    BossAttacking = true;
                    StartCoroutine(MeleeAttack());
                    break;  
                default:
                    //Debug.Log(BossChoice);
                    break;
            }
        }
    }
    #region Attacks
    public IEnumerator MeleeAttack()
    {
        Debug.Log("2");
        BossAttacking = true;
        yield return new WaitForSeconds(MeleeTelegraphTime);

        // insert boss melee telegraph here

        BossAttack.SetActive(true);

        yield return new WaitForSeconds(2);

        BossAttack.SetActive(false);

        yield return new WaitForSeconds(bossCooldownTime);

        BossAttacking = false;
    }

    public IEnumerator SlamAttack()
    {
        Debug.Log("0");
        BossAttacking = true;
        // add boss slam telegraph here
        yield return new WaitForSeconds(SlamTelegraphTime);
        float distance = Player.transform.position.x - Boss.transform.position.x;

        rb.AddForce(new Vector2(distance/2, SlamJumpHeight/2), ForceMode2D.Impulse);

        if (Boss.transform.position.x == Player.transform.position.x)
        {
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, -SlamJumpHeight), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(bossCooldownTime);
        BossAttacking = false;
    }

    public IEnumerator laserAttack()
    {
        Debug.Log("1");

        BossAttacking = true;

        yield return new WaitForSeconds(laserTelegraphTime);

        float angle = Mathf.Atan2(Player.transform.position.x - Boss.transform.position.x, Player.transform.position.y - Player.transform.position.y);

        Vector2 Direction = transform.position - Player.transform.position;

        GameObject bulletProjectile = Instantiate(BossLaser, transform.position, Quaternion.AngleAxis(angle, Vector3.forward), transform);

        Rigidbody2D rbb = bulletProjectile.transform.GetComponent<Rigidbody2D>();

        rbb.AddForce(-(Direction * power), ForceMode2D.Impulse);

        yield return new WaitForSeconds(bossCooldownTime);

        BossAttacking = false;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<CombatScript>().DamagePlayer(2);
        }
    }

    #region BossState
    public Vector2 playerDistance()
    {
        if (Player == null)
        {
            return Vector2.zero;
        }
        Vector2 distance = Boss.transform.position - Player.transform.position;

        return distance;

    }
    #endregion
}
