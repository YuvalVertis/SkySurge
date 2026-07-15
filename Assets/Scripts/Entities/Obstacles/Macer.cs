using System.Collections;
using UnityEngine;

public sealed class Macer : MonoBehaviour
{
    [SerializeField] float duration;

    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while(true)
        {
            float time = 0;
            Vector3 position = transform.position;
            while (time < duration * 1.5)
            {
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(position.y, position.y - 4.61f, time / (duration * 1.5f)));
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = new Vector2(transform.position.x, position.y - 4.61f);
            time = 0;
            yield return new WaitForSeconds(1f);
            while (time < duration)
            {
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(position.y - 4.61f, position.y, time / duration));
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = new Vector2(transform.position.x, position.y);
            yield return new WaitForSeconds(0.9f);

        }
    }
}
