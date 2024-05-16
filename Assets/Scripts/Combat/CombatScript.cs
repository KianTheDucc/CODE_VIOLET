using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public int curHealth = 0;
    public int MaxHealth = 6;
    public GameObject entity;
    public GameObject Health1;
    public GameObject Health1H;
    public GameObject Health2;
    public GameObject Health2H;
    public GameObject Health3;
    public GameObject Health3H;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = MaxHealth;
        //GameObject.Find("Health 1H").SetActive(false);
        //GameObject.Find("Health 2H").SetActive(false);
        //GameObject.Find("Health 3H").SetActive(false);

        Health1H.SetActive(false);
        Health2H.SetActive(false);
        Health3H.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (curHealth == 0)
        {
            entity.SetActive(false);
            Debug.Log("Player Death");
        }
    }
    public void DamagePlayer(int damage)
    {
        curHealth -= damage;
        healthBarChange();


    }
    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        healthBarChange();
    }

    public void healthBarChange()
    {
        switch(curHealth)
        {
            case 0:

                // death screen
                break;
            case 1:
                Health1H.SetActive(true);
                Health1.SetActive(false);
                //GameObject.Find("Health 1H").SetActive(true);
                //GameObject.Find("Health 1").SetActive(false);
                // one bar is left on orange
                break;
            case 2:
                Health2H.SetActive(false);
                //GameObject.Find("Health 2H").SetActive(false);
                // one bar is white
                break;
            case 3:
                Health2.SetActive(false);
                Health2H.SetActive(true);
                //GameObject.Find("Health 2").SetActive(false);
                //GameObject.Find("Health 2H").SetActive(true);
                // one bar is left white and the other turned orange
                break;
            case 4:
                Health3H.SetActive(false);
                //GameObject.Find("Health 3H").SetActive(false);
                // two bars are white one is red
                break;
            case 5:
                Health3.SetActive(false);
                Health3H.SetActive(true);
                //GameObject.Find("Health 3").SetActive(false);
                //GameObject.Find("Health 3H").SetActive(true);
                // two bars are white one is orange
                break;
            default: break;


        }
    }
}