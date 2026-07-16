using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine;

public sealed class ScenesHandler : MonoBehaviour
{
    public static ScenesHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneByName(Levels level, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(level.ToString(), loadMode);
    }

    public void LoadSceneByIndex(Levels level, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene((int)level, loadMode);
    }

    public async Task LoadSceneByNameAsync(Levels level, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(level.ToString(), loadMode);
        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }

    public async Task LoadSceneByIndexAsync(Levels level, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)level, loadMode);
        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }
}