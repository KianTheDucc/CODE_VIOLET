using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TPplayerOnTrigger : MonoBehaviour
{
    public GameObject tpLocation;
    public GameObject tpPlayer;
    public Vector3 tpLocationCoordinates;
    void Start()
    {
      tpLocationCoordinates = tpLocation.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tpPlayer.transform.position = tpLocationCoordinates;
            Debug.Log("You have fallen!");
        }

    }
}
