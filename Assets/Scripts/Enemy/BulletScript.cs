using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public LayerMask wall;
    public LayerMask ground;
    public LayerMask WJW;
    [SerializeField] private int Damage;
    private Rigidbody2D rb;

    private bool reflected;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("WJW"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CombatScript>().TakeDamage(Damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Attack"))
        {
            reflected = true;
            rb.velocity = -rb.velocity;
        }
        if (reflected && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
            Destroy(this.gameObject);
        }
    }
}
