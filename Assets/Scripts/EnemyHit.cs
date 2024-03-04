using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyHitScript : MonoBehaviour
{
    public GameObject HitBox;
    public BoxCollider2D AttackBox;
    public SpriteRenderer AttackSprite;
    private bool playerHit;
    private bool cooldown = false;
    public float windupTime = 3f;
    public float damageTime = 0.1f;
    public float cooldownTime = 3f;


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && cooldown == false)
        {
            StartCoroutine(EnemyAttack());
        }
        if (other.tag == "Player" && playerHit == true)
        {
            Debug.Log("Player Hit");
            playerHit = false;
        }

    }

    public IEnumerator EnemyAttack()
    {
        cooldown = true;
        AttackSprite.color = new Color(1f, 0f, 0f, 0.5f);
        yield return new WaitForSeconds(windupTime);
        AttackSprite.color = new Color(1f, 0f, 0f, 1f);
        playerHit = true;
        yield return new WaitForSeconds(damageTime);
        playerHit = false;
        AttackSprite.color = new Color(1f, 0f, 0f, 0f);
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;

    }
}
