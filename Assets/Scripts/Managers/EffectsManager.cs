using PrimeTween;
using UnityEngine;

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

    public Tween Fade(SpriteRenderer sprite, float duration, float alpha, Ease ease = Ease.OutQuad)
    {
        if (sprite == null) return default;
        return Tween.Alpha(sprite, alpha, duration, ease);
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

    public void FadeIn(SpriteRenderer sprite, float duration, bool enableColliders = false,
        ChangeActiveState state = ChangeActiveState.NoChange, Ease ease = Ease.OutQuad)
    {
        if (sprite == null) return;

        Tween.Alpha(sprite, 1f, duration, ease).OnComplete(() =>
        {
            if (sprite == null) return;

            ActivateByState(true, sprite, state);

            if (enableColliders)
            {
                SetColliders(sprite, true);
            }
        }, warnIfTargetDestroyed: false);
    }

    public Tween FadeOut(SpriteRenderer sprite, float duration, bool disableColliders = false,
        ChangeActiveState state = ChangeActiveState.NoChange, Ease ease = Ease.InQuad)
    {
        if (sprite == null) return default;

        return Tween.Alpha(sprite, 0f, duration, ease).OnComplete(() =>
        {
            if (sprite == null) return;

            ActivateByState(false, sprite, state);

            if (disableColliders)
            {
                SetColliders(sprite, false);
            }
        }, warnIfTargetDestroyed: false);
    }

    public Tween ChangeColor(SpriteRenderer sprite, Color newColor, float duration, Ease ease = Ease.Linear)
    {
        if (sprite == null) return default;

        return Tween.Color(sprite, newColor, duration, ease);
    }

    public void ActivateByState(bool active, SpriteRenderer sprite, ChangeActiveState state)
    {
        switch (state)
        {
            case ChangeActiveState.NoChange:
                return;

            case ChangeActiveState.Change:
                sprite.gameObject.SetActive(active);
                break;

            case ChangeActiveState.ChangeInParent:
                Transform parent = sprite.transform.parent;
                if (parent != null)
                {
                    parent.gameObject.SetActive(active);
                }
                break;
        }
    }

    void SetColliders(SpriteRenderer sprite, bool enable)
    {
        Collider2D[] colliders = sprite.GetComponents<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = enable;
        }
    }

    public Tween Scale(Transform target, Vector3 newSize, float duration, bool repeat = false, Ease ease = Ease.OutBack)
    {
        if (target == null) return default;

        int cycles = repeat ? -1 : 1;
        return Tween.Scale(target, newSize, duration, ease, cycles, CycleMode.Rewind);
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

    public Tween Spin(Transform target, float duration, bool infinite = false,
        Ease ease = Ease.Linear, int cycles = 1)
    {
        if (target == null) return default;

        Vector3 start = target.eulerAngles;
        Vector3 end = start + new Vector3(0, 0, 360);
        return Tween.EulerAngles(target, start, end, duration, ease, infinite ? -1 : cycles);
    }
}