using UnityEngine;

public sealed class StopLevelFade : MonoBehaviour
{
    Level level;

    void Awake()
    {
        level = FindObjectOfType<Level>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !level.fade)
        {
            level.fade = true;
        }
    }
}
