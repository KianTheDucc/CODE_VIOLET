using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsorBehaviour : MonoBehaviour
{
    public GameObject Player;
    public GameObject Repulsor;
    public Camera MainCam;

    // Update is called once per frame
    void Update()
    {
        Vector3 centerPoint = Player.GetComponent<SpriteRenderer>().bounds.center;
        Vector3 mousePos = MainCam.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - centerPoint.y, mousePos.x - centerPoint.x);
       Repulsor.transform.localPosition = new Vector3(1.5f * Mathf.Cos(angle), 1.5f * Mathf.Sin(angle), 0);
       Repulsor.transform.eulerAngles = new Vector3(0, 0, (angle * 180 / Mathf.PI) - 90);
    }
}