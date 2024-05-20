using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    //changes the health bar state
    public void healthBarChange()
    {
        switch(curHealth)
        {
            case 0:

                SceneManager.LoadSceneAsync("Main_Menu_Save_Load");
                break;
            case 1:
                Health1H.SetActive(true);
                Health1.SetActive(false);

                break;
            case 2:
                Health2H.SetActive(false);

                break;
            case 3:
                Health2.SetActive(false);
                Health2H.SetActive(true);

                break;
            case 4:
                Health3H.SetActive(false);

                break;
            case 5:
                Health3.SetActive(false);
                Health3H.SetActive(true);

                break;
            default: break;


        }
    }
}