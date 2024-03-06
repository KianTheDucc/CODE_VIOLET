using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class moveThroughGround : MonoBehaviour
{
    public float passThroughTime;

    void Update()
    {
        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            StartCoroutine(disableCollider());
        }
    }

    private IEnumerator disableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        Debug.Log("PlayerFallThrough");
        yield return new WaitForSeconds(passThroughTime);
        GetComponent<BoxCollider2D>().enabled = true;
        Debug.Log("Collider reenabled");
    }
}
