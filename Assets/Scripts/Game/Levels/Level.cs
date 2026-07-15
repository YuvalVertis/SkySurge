using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject[] sCloud;
    [SerializeField] float fallCords;
    bool tracker;
    public bool fade;
    private void Update()
    {
        if (target.position.y <= fallCords)
        {
            SceneManager.LoadScene("Levels");
        }
        if (!tracker)
        {
            StartCoroutine(Fade()); 
            tracker = true;
        }
    }
    IEnumerator Fade()
    {
        float duration = 1f;
        while (true)
        {
            foreach (GameObject cloud in sCloud)
            {
                if (fade)
                {
                    yield break;
                }
                SpriteRenderer currentSr = cloud.GetComponent<SpriteRenderer>();
                Color currentColor = currentSr.color;
                float timer = 0f;
                while (timer < duration)
                {
                    currentSr.color = Color.Lerp(currentColor, new Color(currentColor.r, currentColor.g, currentColor.b, 0f), timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }
                cloud.SetActive(false);
                yield return new WaitForSeconds(.75f); 
                cloud.SetActive(true);
                timer = 0f; 
                while (timer < duration)
                {
                    currentSr.color = Color.Lerp(new Color(currentColor.r, currentColor.g, currentColor.b, 0f), currentColor, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }
                yield return new WaitForSeconds(.75f); 
            }
        }
    }
}
