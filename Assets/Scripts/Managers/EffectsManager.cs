using System.Collections;
using UnityEngine;
using PrimeTween;

public sealed class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void FadeEffect(SpriteRenderer sprite, float duration, float alpha, Ease ease = Ease.OutQuad)
    {
        if (sprite == null) return;
        Tween.Alpha(sprite, alpha, duration, ease);
    }

    public Sequence FadeRepeat(SpriteRenderer sprite, float duration, float delay, Ease ease = Ease.InOutSine)
    {
        if (sprite == null) return default;

        Sequence sequence = Sequence.Create()
            .Chain(Tween.Alpha(sprite, 0f, duration, ease))
            .ChainCallback(() =>
            {
                if (sprite != null) SetColliders(sprite, false);
            })
            .ChainDelay(delay)
            .ChainCallback(() =>
            {
                if (sprite != null) SetColliders(sprite, true);
            })
            .Chain(Tween.Alpha(sprite, 1f, duration, ease))
            .ChainDelay(delay);

        sequence.SetRemainingCycles(-1);

        return sequence;
    }

    public void FadeIn(SpriteRenderer sprite, float duration, bool enableColliders = false, Ease ease = Ease.OutQuad)
    {
        if (sprite == null) return;

        Tween.Alpha(sprite, 1f, duration, ease).OnComplete(() =>
        {
            if (sprite == null) return;

            if (enableColliders)
            {
                SetColliders(sprite, true);
            }
        }, warnIfTargetDestroyed: false);
    }

    public void FadeOut(SpriteRenderer sprite, float duration, bool disableColliders = false, Ease ease = Ease.InQuad)
    {
        if (sprite == null) return;

        Tween.Alpha(sprite, 0f, duration, ease).OnComplete(() =>
        {
            if (sprite == null) return;

            if (disableColliders)
            {
                SetColliders(sprite, false);
            }
        }, warnIfTargetDestroyed: false); 
    }

    void SetColliders(SpriteRenderer sprite, bool enable)
    {
        Collider2D[] colliders = sprite.GetComponents<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = enable;
        }
    }

    public void Scale(Transform target, Vector2 newSize, float duration, Ease ease = Ease.OutBack)
    {
        Tween.Scale(target, newSize, duration, ease);
    }

    public void FadeVolume(AudioSource src, float finalValue, float duration, Ease ease = Ease.OutQuad)
    {
        if (src == null) return;

        finalValue = Mathf.Clamp01(finalValue);
        Tween.AudioVolume(src, finalValue, duration, ease);
    }

    public void FadePitch(AudioSource src, float finalValue, float duration, Ease ease = Ease.OutQuad)
    {
        if (src == null) return;

        finalValue = Mathf.Clamp(finalValue, 0.01f, 3f);
        Tween.AudioPitch(src, finalValue, duration, ease);
    }

    public void Spin(Transform target, float duration, bool infinite = false, Ease ease = Ease.Linear, int cycles = 1)
    {
        Vector3 start = target.eulerAngles;
        Vector3 end = start + new Vector3(0, 0, 360);

        Tween.EulerAngles(target, start, end, duration, ease, infinite ? -1 : cycles);
    }
}