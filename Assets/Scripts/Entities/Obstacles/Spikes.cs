using System.Collections;
using UnityEngine;

public sealed class Spikes : MonoBehaviour
{ 
    [SerializeField] float duration;
    [SerializeField] float range;
    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        float time;
        while(true)
        {
            time = 0;
            Vector2 position = transform.position;
            while(time < duration)
            {
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(position.y, position.y + range, time / duration));
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = new Vector2(transform.position.x, position.y + range);
            time = 0;
            yield return new WaitForSeconds(0.8f);
            while (time < duration)
            {
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(position.y + range, position.y, time / duration));
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = new Vector2(transform.position.x, position.y);
            yield return new WaitForSeconds(0.8f);
            if(this.enabled == false)
            {
                yield break;
            }
        }
    }
}
