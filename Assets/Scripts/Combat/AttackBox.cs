using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public bool hit;
    Vector2 pos;
    public GameObject player;

    private Rigidbody2D rb;

    [SerializeField] float knockbackAmount;

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
        Vector2 knockback = new Vector2(knockbackAmount * GameObject.Find("Player").GetComponent<PlayerAttack>().attackDirection, 0);
        if (other.tag == ("Enemy") && hit == false)
        {
            Debug.Log("Enemy hit");
            if (!other.gameObject.GetComponent<EnemyAlert>().isAlert)
            {
                other.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage*2);
                other.gameObject.GetComponent<EnemyAlert>().damaged();
                rb = other.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(knockback, ForceMode2D.Impulse);

            }
            else
            {
                other.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
                rb = other.gameObject.GetComponent<Rigidbody2D>();

                rb.AddForce(knockback, ForceMode2D.Impulse);
            }
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
