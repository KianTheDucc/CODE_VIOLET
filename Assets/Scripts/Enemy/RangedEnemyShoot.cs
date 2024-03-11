using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RangedEnemyShoot : MonoBehaviour
{
    private bool alert;
    public GameObject bullet;
    public GameObject RangedEnemy;
    private bool canShoot = true;
    [SerializeField] private float power;
    [SerializeField] private float shotDelay;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        alert = RangedEnemy.GetComponent<RangedHit>().isAlert;

        if (alert)
        {
            StartCoroutine(shootDelay());
        }
    }

    private IEnumerator shootDelay()
    {
        if (canShoot)
        {
            Shoot();
            canShoot = false;
            yield return new WaitForSeconds(shotDelay);
            canShoot = true;
        }
    }
    public void Shoot()
    {
        Vector2 Direction;

        Direction = transform.position - GameObject.Find("Player").transform.position;

        Direction.Normalize();

        float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;

        GameObject bulletProjectile = Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward), transform);
        Rigidbody2D rb = bulletProjectile.transform.GetComponent<Rigidbody2D>();
        rb.AddForce(-(Direction * power), ForceMode2D.Impulse);
    }
}
