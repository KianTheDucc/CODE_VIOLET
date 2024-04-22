using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{

    public GameObject Attack;
    public float damageTime = 1f;
    [SerializeField] float attackCD;

    public float AttackDamage;

    private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        Attack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            StartCoroutine(AttackTrigger());
        }
    }


    private IEnumerator AttackTrigger()
    {
        attacking = true;
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.x == -1 || input.x == 0 && input.y == 0 &&  GetComponent<SpriteRenderer>().flipX)
        {
            Attack.transform.position = new Vector3(transform.position.x - 1.5f,transform.position.y, transform.position.z);
            Attack.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
        }
        else if (input.x == 1 || input.x == 0 && input.y == 0 && !GetComponent<SpriteRenderer>().flipX)
        {
            Attack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
            Attack.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -90);
        }
        else if (!GetComponent<PlayerController>().IsGrounded() && input.y == -1)
        {
            Attack.transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
            Attack.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        }
        else if (input.y == 1)
        {
            Attack.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            Attack.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        }

        Attack.SetActive(true);
        yield return new WaitForSeconds(damageTime);
        Attack.SetActive(false);
        yield return new WaitForSeconds(attackCD);
        //hit = false;
        attacking = false;
    }

}
