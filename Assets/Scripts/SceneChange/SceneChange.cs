using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public Animator animator;

    public string nextSceneName;

    private GameObject sceneChangeTrigger;

    private int loadLevel;

    private void Start()
    {
        sceneChangeTrigger = GameObject.Find("BlackFade");
        animator = sceneChangeTrigger.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        fadeBetweenLevels();
    }

    //private int LoadNextScene()
    //{
    //    //int nextLevel = SceneManager.GetActiveScene().buildIndex - 1;
    //    //return nextLevel;
    //}
    public void fadeBetweenLevels ()
    {
        SceneManager.LoadScene(nextSceneName); // So the script knows which level to transition to
        animator.SetTrigger("FadeOut"); // Triggers the relevant animation
    }

    public void onFadeComplete ()
    {
        loadLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(loadLevel);
    }
}
