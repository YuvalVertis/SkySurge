using System.Collections;
using UnityEngine;

public sealed class SpikesAdv : MonoBehaviour
{
    // Spikes systems with a lot of spikes behaviours.
    [Header("Horizontal Switching")]
    [SerializeField] float duration;
    [SerializeField] Transform[] spikes;
    [SerializeField] bool toSwitch;

    void Start()
    {
        if(toSwitch)
        {
            StartCoroutine(Switch());
        }
    }

    IEnumerator Switch()
    {
        float time;
        while(true)
        {
            Vector2 first = spikes[0].position;
            Vector2 second = spikes[1].position;
            time = 0;
            while(time < duration)
            {
                spikes[0].position = new Vector2(Mathf.Lerp(first.x, second.x, time / duration), spikes[0].position.y);
                spikes[1].position = new Vector2(Mathf.Lerp(second.x, first.x, time / duration), spikes[1].position.y);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
