using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public sealed class StartGame : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Color lockedColor = new Color32(255, 255, 255, 125);
    [SerializeField] Color unlockedColor = new Color32(168, 80, 64, 250);

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            var txt = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (txt == null) continue;

            if (i < unlockedLevel)
            {
                buttons[i].interactable = true;
                txt.color = unlockedColor;
            }
            else
            {
                buttons[i].interactable = false;
                txt.color = lockedColor;
            }
        }
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
