using System.Collections;
using UnityEngine;

public sealed class SizeIn : MonoBehaviour
{
    [SerializeField] float duration = 0.25f;
    Vector3 initialScale;

    void Awake()
    {
        initialScale = transform.localScale;
        StartCoroutine(SizeInc());
    }

    IEnumerator SizeInc()
    {
        float timer = 0;
        Vector3 targetScaleVector = new Vector3(0.55f, 0.55f, 1f);
        while(timer < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScaleVector, timer / duration);
            timer+=Time.deltaTime;
            yield return null;
        }
    }
}
