using UnityEngine.SceneManagement;
using UnityEngine;

public sealed class StaticOS : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Levels");
        }
    }
}

