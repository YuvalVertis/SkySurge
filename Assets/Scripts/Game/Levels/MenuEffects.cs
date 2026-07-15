using System.Collections;
using UnityEngine;
public sealed class MenuEffects : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform target;
    [SerializeField] RectTransform background;
    [SerializeField] float duration;
    void Start()
    {
        StartCoroutine(Paralax());
        StartCoroutine(Blink());
        StartCoroutine(Look());
    }

    IEnumerator Blink()
    {
        float delay;
        while(true)
        {
            delay = Random.Range(6f, 11f);
            yield return new WaitForSeconds(delay); 
            anim.SetBool("Blink", true);
            yield return new WaitForSeconds(1f);
            anim.SetBool("Blink", false);
        }
    }
    IEnumerator Look()
    {
        float time;
        float startY = target.position.y;
        while (true)
        {
            time = 0;
            yield return new WaitForSeconds(5);
            while (time < 0.6f)
            {
                target.position = new Vector2(target.position.x, Mathf.Lerp(startY, startY + 2.1f, time / 0.6f));
                time += Time.deltaTime;
                yield return null;
            }
            time = 0;
            float currentY = target.position.y;
            yield return new WaitForSeconds(3.5f);
            while (time < 0.65f)
            {
                target.position = new Vector2(target.position.x, Mathf.Lerp(currentY, startY, time / 0.65f));
                time += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(8);
        }
    }
    IEnumerator Paralax()
    {
        float startX = background.anchoredPosition.x;
        float time;
        while (true)
        {
            time = 0;
            yield return new WaitForSeconds(0.75f);
            while (time < duration)
            {
                float t = time / duration;
                t = Mathf.SmoothStep(0f, 1f, t); 
                background.anchoredPosition = new Vector2(Mathf.Lerp(startX, startX + 2051f, t), background.anchoredPosition.y);
                time += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(3);
            background.anchoredPosition = new Vector2(startX + 2051f, background.anchoredPosition.y);
            time = 0;
            float currentX = startX + 2051f; 
            while (time < duration)
            {
                float t = time / duration;
                t = Mathf.SmoothStep(0f, 1f, t); 
                background.anchoredPosition = new Vector2(Mathf.Lerp(currentX, startX, t), background.anchoredPosition.y);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
