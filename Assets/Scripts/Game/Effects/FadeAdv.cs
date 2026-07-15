using System.Collections;
using UnityEngine;

public sealed class FadeAdv : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] float duration;
    bool tracker, notActive;

    public IEnumerator FadeEffect()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            SpriteRenderer sr = spriteRenderers[i];
            Collider2D collider = sr.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            float elapsedTime = 0f;
            Color defaultColor = sr.color;
            Color endColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0f);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                sr.color = Color.Lerp(defaultColor, endColor, elapsedTime / duration);
                yield return null;
            }
            if (notActive)
            {
                sr.gameObject.SetActive(false);
            }
        }
        // Ensure the last sprite is deactivated regardless of whether it was faded out or not
        spriteRenderers[spriteRenderers.Length - 1].gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !tracker)
        {
            StartCoroutine(FadeEffect());
            tracker = true;
        }
    }
}
