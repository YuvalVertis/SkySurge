using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLevelFade : MonoBehaviour
{
    Level level;
    private void Awake()
    {
        level = FindObjectOfType<Level>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !level.fade)
        {
            level.fade = true;
        }
    }
}
