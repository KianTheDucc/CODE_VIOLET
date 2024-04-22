using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public float EnemyHealth;
    public float MaxHealth;

    public float knockbackAmount;

    [SerializeField] GameObject Enemy;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        EnemyHealth = MaxHealth;
        rb = Enemy.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DamageEnemy(float damage)
    {
        EnemyHealth -= damage;
        Debug.Log("Damaged");
        rb.AddForce(new Vector2(knockbackAmount, 0));
        Debug.Log("Knocked back");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            DamageEnemy(2);
        }    
    }
}
