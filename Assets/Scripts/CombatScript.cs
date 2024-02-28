using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public int curHealth = 0;
    public int MaxHealth = 100;
    public GameObject entity;
    // Start is called before the first frame update
    void Start()
    {
        curHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (curHealth == 0)
        {
            entity.SetActive(false);
        }
    }
    public void DamagePlayer(int damage)
    {
        curHealth -= damage;


    }
    public void TakeDamage(int damage)
    {
        curHealth -= damage;
    }
}