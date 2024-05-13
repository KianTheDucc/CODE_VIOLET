using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFix : MonoBehaviour
{

    IEnumerator Start()
    {
        Light2D light = GetComponent<Light2D>();
        yield return new WaitForSeconds(0.1f);
        light.enabled = false;

        yield return new WaitForSeconds(0.1f);
        light.enabled = true;
    }
}
