using PrimeTween;
using UnityEngine;

public sealed class Spikes : MonoBehaviour
{ 
    [SerializeField] float duration;
    [SerializeField] float range;
    Sequence attackSequence;

    void Start()
    {
        Attack();
    }

    void Attack()
    {
        float startY = transform.position.y;

        attackSequence = Sequence.Create()
            .Chain(Tween.PositionY(transform, startY + range, duration))
            .ChainDelay(0.8f)
            .Chain(Tween.PositionY(transform, startY, duration))
            .ChainDelay(0.8f);

        attackSequence.SetRemainingCycles(-1);
    }

    void OnDisable()
    {
        if (attackSequence.isAlive)
        {
            attackSequence.Stop();
        }
    }
}
