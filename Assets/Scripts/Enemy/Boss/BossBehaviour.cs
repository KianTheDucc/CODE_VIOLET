using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    GameObject Player;
    GameObject Boss;
    Vector2 BossDistance;
    bool BossAttacking;
    private float possibleBossChoices;
    private float BossChoice;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Boss = this.gameObject;
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
                    BossAttacking = true;
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
        //TBA
        yield return null;
    }

    public IEnumerator SlamAttack()
    {
        //TBA
        yield return null;
    }

    public IEnumerator laserAttack()
    {
        //TBA
        yield return null;
    }
    #endregion

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
