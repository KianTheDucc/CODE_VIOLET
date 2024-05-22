using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinishScript : MonoBehaviour
{  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        SceneManager.LoadSceneAsync("EndScene");
    }
}
