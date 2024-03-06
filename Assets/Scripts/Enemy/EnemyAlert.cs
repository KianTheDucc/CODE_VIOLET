using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    public BoxCollider2D Alert;
    public GameObject HitBox;
    public bool isAlert;
    public GameObject Player;
    public float speed;
    private float distance;
    public float patrolTime = 3f;
    private float patrolDir = -1f;
    private float patrolTimer;
    // Start is called before the first frame update
    void Start()
    {
        HitBox.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HitBox.SetActive(true);
            isAlert = true;
            patrolTimer = patrolTime;
        }

    }
    void Update()
    {

        if (isAlert)
        {
            distance = Vector2.Distance(transform.position, Player.transform.position);
            Vector2 direction = Player.transform.position - transform.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, speed * Time.deltaTime);
            if (angle < 90)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            //patrol left and right
            transform.position = transform.position + new Vector3(patrolDir * speed * Time.deltaTime, 0, 0);
        }

        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0.0f)
        {
            timerEnded();
        }

    }

    void timerEnded()
    {
        patrolTimer = patrolTime;
        patrolDir = patrolDir * -1f;
        if (patrolDir < 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

    }
    public void damaged()
    {
        isAlert = true;
    }

}
