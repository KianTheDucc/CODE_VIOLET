using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    public string keyColor;
    public GameObject playerRef;

    void OnCollisionEnter2D(Collision2D Other)
    {
        Debug.Log("Obtained key");
        playerRef.GetComponent<InventoryManager>().keyList.Add(keyColor);
        Destroy(this.gameObject);
    }
}

