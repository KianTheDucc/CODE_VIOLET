using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public bool hit;
    public float damageTime = 1f;
    Vector2 pos;
    public GameObject player;
    Vector2 playerPos;


    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        pos = new Vector2(2.5f, 0f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && hit == false)
        {
            Debug.Log("Enemy hit");
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
