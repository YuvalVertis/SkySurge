using System.Collections;
using UnityEngine;

public sealed class FadeMusic : MonoBehaviour
{
    [SerializeField] float duration;
    AudioSource music;
    bool use;

    void Awake()
    {
        music = GetComponent<AudioSource>();
    }
    void Update()
    {
       if(!use)
       {
            StartCoroutine(Fade());
            use = true;
       }
    }

    IEnumerator Fade()
    {
        float time = 0;
        float currentV = music.volume;
        while(time < duration)
        {
            yield return null;
            music.volume = Mathf.Lerp(currentV, 0, time / duration);
            time+= Time.deltaTime;
        }
        music.volume = 0;   
    }
}
