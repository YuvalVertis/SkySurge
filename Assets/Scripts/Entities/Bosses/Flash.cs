using System.Collections;
using UnityEngine;
public class Flash : MonoBehaviour
{
    Material[] material;
    SpriteRenderer[] sr;
    private void Awake()
    {
        sr = GetComponentsInChildren<SpriteRenderer>();
        Materials();
    }
    private void Materials()
    {
        material = new Material[sr.Length];
        for(int i =0; i < sr.Length; i++)
        {
            material[i] = sr[i].material;
        }
    }
    public IEnumerator FlashEffect()
    {
        for (int i = 0; i < material.Length; i++)
        {
            material[i].SetFloat("_FlashAmount", 0.5f);
        }
        float timer = 0f;
        while (timer < 0.4f)
        {
            for (int i = 0; i < material.Length; i++)
            {
                float startValue = 0.5f;  
                float currentValue = Mathf.Lerp(startValue, 0, timer / 0.4f);
                material[i].SetFloat("_FlashAmount", currentValue);
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
