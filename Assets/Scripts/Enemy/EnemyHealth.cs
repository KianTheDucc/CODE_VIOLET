using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int curHealth = 0;
    public int MaxHealth = 10;
    public GameObject entity;
    // Start is called before the first frame update
    void Start()
    {
        curHealth = MaxHealth;
    }

    void Update()
    {

        if (curHealth <= 0)
        {
            entity.SetActive(false);
            Debug.Log("Enemy Death");
        }
    } 

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
    }
    // Update is called once per frame


}
