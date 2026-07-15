using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameResoultion : MonoBehaviour
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
