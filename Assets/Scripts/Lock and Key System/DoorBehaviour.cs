using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
   public GameObject playerRef;
   private Animator doorAnim;
   private bool doorOpen = false;

   void OnCollisionEnter2D(Collision2D Other)
    {
        Debug.Log("Door Collision");

        if (playerRef.GetComponent<Inventory>().keyList.Contains(this.tag))
        {
            doorOpen = true;
            doorAnim = gameObject.GetComponent<Animator>();
            
            if(doorOpen == true)
            {
                doorAnim.Play("OpenDoor");
            }
            else
            {
                doorAnim.Play("CloseDoor");
            }
        }
    }
}
