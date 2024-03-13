using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardRespawn : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var respawnPoints = GameObject.FindGameObjectsWithTag("hazardRespawn");
            var pos = transform.position;

            float dist = float.PositiveInfinity;
            GameObject nearest = null;
            foreach (var p in respawnPoints)
            {
                var di = (p.transform.position - pos).sqrMagnitude;
                if (di < dist)
                {
                    nearest = p;
                    dist = di;
                }
            }
            collision.transform.position = nearest.transform.position;
        }
    }
}
