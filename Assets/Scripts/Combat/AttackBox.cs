using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public bool hit;
    Vector2 pos;
    public GameObject player;
    Vector2 playerPos;
    public int Damage = 1;


    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        pos = new Vector2(2.5f, 0f);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Enemy") && hit == false)
        {
            Debug.Log("Enemy hit");
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
            hit = true;
        }
    }

    void OnEnable()
    {
        hit = false;
    }

    void Update()
    {

    }
}
