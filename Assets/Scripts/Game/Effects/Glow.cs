using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine;

public sealed class Glow : MonoBehaviour
{
    Light2D glower;
    [SerializeField] float targetInt;

    void Start()
    {
        glower = GetComponent<Light2D>();
        StartCoroutine(Glows());
    }

    IEnumerator Glows()
    {
        float time;
        while(true)
        {
            time = 0;
            float startInt = glower.intensity;
            while (time < 0.75f)
            {
                glower.intensity = Mathf.Lerp(startInt, targetInt, time/.75f);
                time += Time.deltaTime;
                yield return null;
            }
            glower.intensity = targetInt;
            time = 0; 
            yield return new WaitForSeconds(1f);
            while (time < 1)
            {
                glower.intensity = Mathf.Lerp(targetInt, startInt, time/0.75f);
                time += Time.deltaTime;
                yield return null;
            }
            glower.intensity = startInt;
            yield return new WaitForSeconds(1f);
        }
    }
}
