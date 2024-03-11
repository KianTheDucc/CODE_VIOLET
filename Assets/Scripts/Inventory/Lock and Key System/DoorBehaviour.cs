using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DoorBehaviour : MonoBehaviour
{
    public GameObject playerRef;
    private GameObject door;
    private Animator doorAnim;
    private void Start()
    {
        door = this.gameObject;
        doorAnim = GetComponent<Animator>();
        doorAnim.SetBool("doorOpen", false);
        doorAnim.transform.position = door.transform.position;
    }

    void OnCollisionEnter2D(Collision2D Other)
    {
        Debug.Log("Door Collision");
        
        if (playerRef.GetComponent<InventoryManager>().keyList.Contains(this.tag))
        {
            doorAnim.SetBool("doorOpen", true);
            
            if (doorAnim.GetBool("doorOpen") == true)
            {
                Debug.Log("Condition set to true.");
            }
        }
    }
}
