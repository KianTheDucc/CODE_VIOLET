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
        if (collision.gameObject.layer == wall.value || collision.gameObject.layer == WJW.value || collision.gameObject.layer == ground.value)
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CombatScript>().TakeDamage(Damage);
        }
        else if (collision.gameObject.name == "Attack")
        {
            rb.velocity = -rb.velocity;
        }
    }
}
