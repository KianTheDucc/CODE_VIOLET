using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedHit : MonoBehaviour
{
    public float LOSDistance;
    public LayerMask player;
    private float angleIncrement;
    public float LOSAngle;
    public bool isAlert;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (RangedLOS())
        {
            Alerted();
        }
    }

    public void Alerted()
    {
        Debug.Log("alerted");
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
