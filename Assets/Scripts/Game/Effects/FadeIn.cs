using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] float duration;
    SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        StartCoroutine(FadeStart());
    }
    IEnumerator FadeStart()
    {
        Color startColor = sr.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1);
        float time = 0;
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        while (time < duration)
        {
            yield return null;
            sr.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
        }
        sr.color = endColor;
        collider.enabled = true;
    }
}
