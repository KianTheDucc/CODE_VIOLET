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

    // Start is called before the first frame update

    void OnTriggerStay2D(Collider2D other)
    {
        if (cooldown == false)
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
        yield return new WaitForSeconds(3);
        AttackSprite.color = new Color(1f, 0f, 0f, 1f);
        playerHit = true;
        yield return new WaitForSeconds(0.1f);
        AttackSprite.color = new Color(1f, 0f, 0f, 0.5f);
        playerHit = false;
        AttackSprite.color = new Color(1f, 0f, 0f, 0f);
        yield return new WaitForSeconds(3f);
        cooldown = false;

    }
}
