using UnityEngine.SceneManagement;
using UnityEngine;

public sealed class LevelsMenu : MonoBehaviour
{
    public void LevelMenu()
    {
        SceneManager.LoadScene("Levels");
    }
}
