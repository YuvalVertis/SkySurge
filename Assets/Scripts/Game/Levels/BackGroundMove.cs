using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackGroundMove : MonoBehaviour
{
    RectTransform background;
    [SerializeField] float duration;
    private void Start()
    {
        background = GetComponent<RectTransform>();
        StartCoroutine(Paralax());
    }
    IEnumerator Paralax()
    {
        float startX = background.anchoredPosition.x;
        float time;
        while (true)
        {
            time = 0;
            yield return new WaitForSeconds(0.5f);
            while (time < duration)
            {
                float t = time / duration;
                t = Mathf.SmoothStep(0f, 1f, t);
                background.anchoredPosition = new Vector2(Mathf.Lerp(startX, startX + (startX * -1.05f), t), background.anchoredPosition.y);
                time += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(3);
            background.anchoredPosition = new Vector2(startX + (startX * -1.05f), background.anchoredPosition.y);
            time = 0;
            float currentX = startX + (startX * -1.05f);
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
