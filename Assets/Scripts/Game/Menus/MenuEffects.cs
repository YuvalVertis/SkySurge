using PrimeTween;
using System.Collections;
using UnityEngine;
public sealed class MenuEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform background;
    [SerializeField] Transform target;
    [SerializeField] Animator anim;
    [SerializeField] float duration;

    [Header("Behaviours")]
    [SerializeField] bool doBlink;
    [SerializeField] bool doLook;
    [SerializeField] bool doParallax;

    void Start()
    {
        Parallax();
        StartCoroutine(Blink());
        Look();
    }

    IEnumerator Blink()
    {
        if (!doBlink) yield return null;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(6f, 11f));
            anim?.SetBool(CodesManager.Blink, true);
            yield return new WaitForSeconds(1f);
            anim?.SetBool(CodesManager.Blink, false);
        }
    }

    void Look()
    {
        if (!doLook) return;

        float startPosY = target.position.y;
        Sequence.Create()
            .ChainDelay(5f)
            .Chain(Tween.PositionY(target, startPosY + 2.2f, 0.6f))
            .ChainDelay(3.5f)
            .Chain(Tween.PositionY(target, startPosY, 0.65f))
            .ChainDelay(8f)
            .SetRemainingCycles(-1);
    }

    void Parallax()
    {
        if (!doParallax) return;

        float startPositionX = background.anchoredPosition.x;
        Sequence.Create()
            .ChainDelay(0.75f)
            .Chain(Tween.UIAnchoredPositionX(background, startPositionX + 2051f, duration, Ease.InOutSine))
            .ChainDelay(3f)
            .Chain(Tween.UIAnchoredPositionX(background, startPositionX, duration, Ease.InOutSine))
            .SetRemainingCycles(-1);
    }
}
