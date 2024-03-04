using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public BoxCollider2D AttackBox;
    public GameObject Attack;
    public float damageTime = 1f;
    private bool hit;

    // Start is called before the first frame update
    void Start()
    {
        Attack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(AttackTrigger());
        }

    }


    private IEnumerator AttackTrigger()
    {
        Attack.SetActive(true);
        yield return new WaitForSeconds(damageTime);
        Attack.SetActive(false);

    }

}
