using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fade : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] float duration;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeEffect());
    }
    IEnumerator FadeEffect()
    {
        Color defaultColor = sr.color;
        Color endColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0f);
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        float time;
        while (true)
        {
            time = 0;
            while(time < duration)
            {
                sr.color = Color.Lerp(defaultColor, endColor , time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            sr.color = endColor;
            time = 0;
            if(colliders.Length > 0)
            {
                colliders[0].enabled = false;
                if(colliders.Length >1)
                {
                    colliders[1].enabled = false;
                }
            }
            yield return new WaitForSeconds(0.8f);
            while (time < duration)
            {
                sr.color = Color.Lerp(endColor, defaultColor, time / duration);
                time+= Time.deltaTime;  
                yield return null;
            }
            sr.color = defaultColor;
            if(colliders.Length > 0)
            {
                colliders[0].enabled = true;
                if (colliders.Length > 1)
                {
                    colliders[1].enabled = true;
                }
            }
            yield return new WaitForSeconds(0.8f);
        }
    }
}
