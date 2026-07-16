using PrimeTween;
using UnityEngine;

public sealed class SpikesAdv : MonoBehaviour
{
    [Header("Spikes")]
    [SerializeField] Transform[] spikes;
    [SerializeField] float duration;
    [SerializeField] bool shouldSwitch;

    Sequence switchSequence;

    void Start()
    {
        if(shouldSwitch)
        {
            Switch();
        }
    }

    public void Switch()
    {
        float firstX = spikes[0].position.x;
        float secondX = spikes[1].position.x;

        switchSequence = Sequence.Create()
            .Group(Tween.PositionX(spikes[0], secondX, duration))
            .Group(Tween.PositionX(spikes[1], firstX, duration));

        switchSequence.SetRemainingCycles(-1);
    }

    void OnDisable()
    {
        switchSequence.Stop();
    }
}
