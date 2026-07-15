using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    [SerializeField] GameObject[] saws;
    [SerializeField] SpriteRenderer sr;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (saws[0] != null && saws[0].activeSelf)
            {
                StartCoroutine(Fade());
            }
            if (saws.Length > 1 && saws[1] != null && !saws[1].activeSelf) // Check if saws[1] exists
            {
                saws[1].SetActive(true);
            }
            if (saws.Length > 2 && saws[2] != null && !saws[2].activeSelf) // Check if saws[2] exists
            {
                saws[2].SetActive(true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (saws[0] != null && saws[0].activeSelf)
            {
                StartCoroutine(Fade()); 
            }
            if (saws.Length > 1 && saws[1] != null && !saws[1].activeSelf) // Check if saws[1] exists
            {
                saws[1].SetActive(true);
            }
            if (saws.Length > 2 && saws[2] != null && !saws[2].activeSelf) // Check if saws[2] exists
            {
                saws[2].SetActive(true);
            }
        }
    }
    IEnumerator Fade()
    {
        float duration = 1;
        Color currentColor = sr.color;
        float timer = 0f;
        saws[0].GetComponentInChildren<Collider2D>().enabled = false;
        while (timer < duration)
        {
            yield return null;
            sr.color = Color.Lerp(currentColor, new Color(currentColor.r, currentColor.g, currentColor.b, 0f), timer / duration);
            timer += Time.deltaTime;
        }
        saws[0].SetActive(false);
    }
}
