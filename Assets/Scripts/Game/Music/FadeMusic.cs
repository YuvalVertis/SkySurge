using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMusic : MonoBehaviour
{
    [SerializeField] float duration;
    AudioSource music;
    bool use;
    private void Awake()
    {
        music = GetComponent<AudioSource>();
    }
    private void Update()
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
