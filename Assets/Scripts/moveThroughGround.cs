using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class moveThroughGround : MonoBehaviour
{
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            Debug.Log("PlayerFallThrough");
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") == 1f)
            {
                this.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}
