using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public Animator animator;

    public GameObject Player;

    public string nextSceneName;

    public string nextSpawnLocationName;

    private GameObject sceneChangeTrigger;

    private int loadLevel;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player");
        sceneChangeTrigger = GameObject.Find("BlackFade");
        animator = sceneChangeTrigger.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetString("SpawnLocation", nextSpawnLocationName);

        string spawnLocation = PlayerPrefs.GetString("SpawnLocation");

        Debug.Log(spawnLocation);
        StartCoroutine(fadeBetweenLevels());

    }

    //private int LoadNextScene()
    //{
    //    //int nextLevel = SceneManager.GetActiveScene().buildIndex - 1;
    //    //return nextLevel;
    //}

    public IEnumerator fadeBetweenLevels()
    {
        AsyncOperation asyncLevelLoad = SceneManager.LoadSceneAsync(nextSceneName); // So the script knows which level to transition to
        animator.SetTrigger("FadeOut"); // Triggers the relevant animation

        string spawnLocation = PlayerPrefs.GetString("SpawnLocation");

        Debug.Log(spawnLocation);

        while (!asyncLevelLoad.isDone)
        {
            yield return null;
        }

        Player.transform.position = GameObject.Find(spawnLocation).transform.position;
        yield return null;
    }

    public void onFadeComplete ()
    {
        fadeBetweenLevels();
        //loadLevel = SceneManager.GetActiveScene().buildIndex + 1;
        //SceneManager.LoadScene(loadLevel);
    }
}
