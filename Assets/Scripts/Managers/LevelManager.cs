using UnityEngine;
using PrimeTween;

public sealed class LevelManager : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject[] specialClouds;

    void Start()
    {
        foreach (var item in specialClouds)
        {
            var sprite = item.GetComponent<SpriteRenderer>();
            if(sprite != null)
            {
                EffectsManager.Instance.FadeRepeat(sprite, 1f, 0.75f, Ease.InOutSine);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        ScenesHandler.Instance.LoadSceneByIndex(Levels.Levels);
    }

}
