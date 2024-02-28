using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject HitBox;
    public BoxCollider2D Alert;
    public SpriteRenderer AttackSprite;

    // Start is called before the first frame update
    void Start()
    {
        HitBox.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter2D()
    {
        HitBox.SetActive(true);
        //wait time amount
        AttackSprite.Color = new Color(1f, 0f, 0f, 1f);
    }
}
