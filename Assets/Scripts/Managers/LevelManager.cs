using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public sealed class LevelManager : MonoBehaviour
{
    List<Sequence> fadeSequences = new List<Sequence>();
    [SerializeField] GameObject[] specialClouds;
    [SerializeField] float fadeSpeed = 1f;
    [SerializeField] float delay = 0.75f;
    [SerializeField] bool allowFade = true;

    void Start()
    {
        if (!allowFade) return;
        foreach (var item in specialClouds)
        {
            var sprite = item.GetComponent<SpriteRenderer>();
            if(sprite != null)
            {
                fadeSequences.Add(EffectsManager.Instance.FadeRepeat(sprite, fadeSpeed, delay, Ease.InOutSine));
            }
        }
    }

    //World border
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && enabled)
        {
            ScenesHandler.LoadSceneByIndex(Levels.Levels);
        }
    }

    void OnDisable()
    {
        foreach (var sequence in fadeSequences)
        {
            sequence.Stop();
        }

        fadeSequences.Clear();
    }
}
