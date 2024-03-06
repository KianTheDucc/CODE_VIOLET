using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class moveThroughGround : MonoBehaviour
{
    public float passThroughTime;
    private GameObject platform;
    public MovementData Data;

    void Update()
    {
        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            StartCoroutine(platformFallThrough());
        }
    }

    public IEnumerator platformFallThrough()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Data.groundcastDistance, Data.whatIsGround);
        hit.collider.enabled = false;
        Debug.Log("PlayerFallThrough");
        yield return new WaitForSeconds(passThroughTime);
        hit.collider.enabled = true;
        Debug.Log("Collider reenabled");
    }
}
