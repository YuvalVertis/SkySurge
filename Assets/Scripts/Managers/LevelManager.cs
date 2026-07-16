using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public sealed class LevelManager : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject[] specialClouds;
    List<Sequence> fadeSequences = new List<Sequence>();

    void Start()
    {
        foreach (var item in specialClouds)
        {
            var sprite = item.GetComponent<SpriteRenderer>();
            if(sprite != null)
            {
                fadeSequences.Add(EffectsManager.Instance.FadeRepeat(sprite, 1f, 0.75f, Ease.InOutSine));
            }
        }
    }

    //World border
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
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
    }
}
