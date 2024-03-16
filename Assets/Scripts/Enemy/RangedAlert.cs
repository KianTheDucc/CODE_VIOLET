using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedHit : MonoBehaviour
{
    public float LOSDistance;
    public LayerMask player;
    public GameObject PlayerGO;
    private float angleIncrement;
    public float LOSAngle;
    public bool isAlert;

    void Start()
    {
        PlayerGO = GameObject.Find("Player");
    }
    void Update()
    {
        lookAtPlayer();

        if (RangedLOS())
        {
            Alerted();
        }
    }

    public void Alerted()
    {
        Debug.Log("alerted");
    }

    public void lookAtPlayer()
    {
        Vector2 Direction = PlayerGO.transform.position - transform.position;

        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        if (angle > 90 || angle < -175)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    #region CONDITIONS

    public bool RangedLOS()
    {
        isAlert = false;
        angleIncrement = 1f;
        if(transform.rotation.eulerAngles.y == 180f)
        {
            LOSAngle = 360f;
        }
        else if (transform.rotation.eulerAngles.y == 0f)
        {
            LOSAngle = 90f;
        }
            for (float angle = 0f; angle < LOSAngle; angle += angleIncrement)
            {
                if (transform.rotation.eulerAngles.y == 180f)
                {
                    angle += 270;
                }
                float angleRadians = angle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
                var hit = Physics2D.Raycast(transform.position, direction, LOSDistance, player);
                Debug.DrawLine(transform.position, hit.point);
                if (hit.collider != null)
                {
                    isAlert = true;
                }
            }



        return isAlert;
    }

    #endregion
}
