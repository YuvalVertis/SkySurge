using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine;

public static class ScenesHandler 
{
    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadSceneByName(Levels level, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(level.ToString(), loadMode);
    }

    public static void LoadSceneByIndex(Levels level, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene((int)level, loadMode);
    }

    public static async Task LoadSceneByNameAsync(Levels level, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(level.ToString(), loadMode);
        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }

    public static async Task LoadSceneByIndexAsync(Levels level, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)level, loadMode);
        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }

    public static void LoadLevelsMenu()
    {
        LoadSceneByIndex(Levels.Levels);
    }

    public static void NextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if(unlockedLevel >= 8 || unlockedLevel < 0)
        {
            return;
        }

        int currentLevel = SceneManager.GetActiveScene().buildIndex - 1;

        if (currentLevel == unlockedLevel)
        {
            unlockedLevel++;
            PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
            PlayerPrefs.Save();
        }
    }
}