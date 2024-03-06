using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public bool hit;
    Vector2 pos;
    public GameObject player;
    Vector2 playerPos;


    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        pos = new Vector2(2.5f, 0f);

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy" && hit == false)
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
