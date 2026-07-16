using UnityEngine;

public sealed class Triggers : MonoBehaviour
{
    [SerializeField] GameObject[] saws;
    [SerializeField] SpriteRenderer sprite;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SawFunction();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SawFunction();
        }
    }

    void SawFunction()
    {
        if (saws[0] != null && saws[0].activeSelf)
        {
            EffectsManager.Instance.FadeOut(sprite, 1f);
        }
        if (saws.Length > 1 && saws[1] != null && !saws[1].activeSelf)
        {
            saws[1].SetActive(true);
        }
        if (saws.Length > 2 && saws[2] != null && !saws[2].activeSelf)
        {
            saws[2].SetActive(true);
        }
    }
}
