using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private GameObject Player;
    private GameObject Boss;
    private GameObject BossAttack;

    bool BossAttacking;

    private float possibleBossChoices;
    private float BossChoice;

    public float SlamJumpHeight;
    public float SlamTelegraphTime;
    public float MeleeTelegraphTime;
    public float bossCooldownTime;


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


    public void BossBehaviourTree()
    {
        if (!BossAttacking)
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
            BossChoice = Random.Range(0, possibleBossChoices);
            switch (BossChoice)
            {
                case 0:
                    StartCoroutine(SlamAttack());
                    break;
                case 1:
                    BossAttacking = true;

                    StartCoroutine(laserAttack());
                    break;
                case 2:
                    BossAttacking = true;
                    StartCoroutine(MeleeAttack());
                    break;  
            }
        }
    }
    #region Attacks
    public IEnumerator MeleeAttack()
    {
        BossAttacking = true;
        //TBA
        yield return new WaitForSeconds(MeleeTelegraphTime);

        BossAttack.SetActive(true);
        yield return new WaitForSeconds(2);
        BossAttack.SetActive(false);
        yield return new WaitForSeconds(bossCooldownTime);
        BossAttacking = false;
    }

    public IEnumerator SlamAttack()
    {
        BossAttacking = true;
        // add boss slam telegraph here
        yield return new WaitForSeconds(SlamTelegraphTime);
        float distance = Boss.transform.position.x - Player.transform.position.x;

        rb.AddForce(new Vector2(distance, SlamJumpHeight), ForceMode2D.Impulse);

        yield return new WaitForSeconds(bossCooldownTime);
        BossAttacking = false;
    }

    public IEnumerator laserAttack()
    {
        //TBA
        yield return null;
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
