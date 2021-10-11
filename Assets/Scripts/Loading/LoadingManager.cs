using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static int nextLevel = 0;
    public static int nextLevelIndex = 0;
    public static string nextScene;
    public LevelAsset levelAsset;
    private void Start()
    {
        nextLevel = levelAsset.levelIndex[nextLevelIndex];
        nextLevelIndex++;
        StartCoroutine(LoadScene(nextScene));
    }

    private IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(0.65f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
