using UnityEngine;
using UnityEngine.SceneManagement;

public class SubSceneLoader : MonoBehaviour
{
    public string sceneName;
    public Transform spawnPoint;

    private void Start()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)
            .completed += (operation) =>
            {
                Scene loadedScene = SceneManager.GetSceneByName(sceneName);

                foreach (GameObject rootObject in loadedScene.GetRootGameObjects())
                {
                    rootObject.transform.position = spawnPoint.position;
                    rootObject.transform.rotation = spawnPoint.rotation;
                }
            };
    }

    void OnSceneLoaded(AsyncOperation op)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            obj.transform.position += spawnPoint.position;
        }
    }
}
