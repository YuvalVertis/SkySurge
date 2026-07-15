using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public sealed class StartGame : MonoBehaviour
{
    [SerializeField] Button[] buttons;

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < unlockedLevel)
            {
                buttons[i].interactable = true;
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color32(168, 80, 64, 250); 
            }
            else
            {
                buttons[i].interactable = false;
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 100);
            }
        }
    }

    public void ChooseLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
    }
}
