using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPplayerOnTrigger : MonoBehaviour
{
    public GameObject tpLocation;
    public GameObject tpPlayer;
    public Vector3 tpLocationCoordinates;
    // Start is called before the first frame update
    void Start()
    {
      tpLocationCoordinates = tpLocation.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        tpPlayer.transform.position = tpLocationCoordinates;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
