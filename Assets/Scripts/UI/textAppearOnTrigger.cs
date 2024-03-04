using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textAppearOnTrigger : MonoBehaviour
{
    public GameObject UI_Text;
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
        if (UI_Text != null)
        {
            UI_Text.SetActive(true);
            Debug.Log("Collision");
        }
    }
}
