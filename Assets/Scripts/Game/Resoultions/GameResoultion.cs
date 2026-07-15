using UnityEngine;
using TMPro;

public sealed class GameResoultion : MonoBehaviour
{
    TextMeshProUGUI resolutionText;

    private void Awake()
    {
        resolutionText= GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        resolutionText.text = "Resolution: " + screenWidth + "x" + screenHeight;
    }
}
