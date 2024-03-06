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

    // Update is called once per frame
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
        //var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //playerPos = player.transform.position;
        //if (input.x == -1 || input.x == 0 && player.GetComponent<SpriteRenderer>().flipX)
        //{
        //    pos.x = playerPos.x - 1.5f;
        //    pos.y = playerPos.y;
        //    transform.position = pos;

        //}
        //else if (input.x == 1 || input.x == 0 && !player.GetComponent<SpriteRenderer>().flipX)
        //{
        //    pos.x = playerPos.x + 1.5f;
        //    pos.y = playerPos.y;
        //    transform.position = pos;
        //}
        //else if (!player.GetComponent<PlayerController>().IsGrounded() && input.y == -1)
        //{
        //    pos.x = playerPos.x;
        //    pos.y = playerPos.y - 1.5f;
        //    transform.position = pos;
        //}
        //else if(input.y == 1)
        //{
        //    pos.x = playerPos.x;
        //    pos.y = playerPos.y + 1.5f;
        //    transform.position = pos;
        //}
    }
}
