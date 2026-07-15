 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{
    public void LevelMenu()
    {
        SceneManager.LoadScene("Levels");
    }
}
