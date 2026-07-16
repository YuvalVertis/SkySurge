using System.Collections;
using UnityEngine;
using PrimeTween;

public sealed class Macer : MonoBehaviour
{
    [SerializeField] float duration;

    void Start()
    {
        Attack();
    }

    public void Attack()
    {
        float startY = transform.position.y;

        Sequence.Create()
            .Chain(Tween.PositionY(transform, startY - 4.61f, duration * 1.5f, Ease.Linear))
            .ChainDelay(1f)
            .Chain(Tween.PositionY(transform, startY, duration, Ease.OutQuad))
            .ChainDelay(0.9f)
            .SetRemainingCycles(-1);
    }
}
