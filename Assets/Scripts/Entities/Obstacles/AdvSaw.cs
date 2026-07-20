using PrimeTween;
using UnityEngine;

public sealed class AdvSaw : MonoBehaviour
{
    [SerializeField] float spinDuration;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] points;
    [SerializeField] Transform spinTransform;
    [SerializeField] int sawIndex;
    bool movingForward = true;
    int currentPoint;
    Tween moveTween;
    Tween spinTween;

    void OnEnable()
    {
        if (EffectsManager.Instance != null && spinTransform != null)
        {
            spinTween = EffectsManager.Instance.Spin(spinTransform, spinDuration, true);
        }
        MoveSaw();
    }

    void MoveSaw()
    {
        if (points.Length < 2 || spinTransform == null || moveSpeed <= 0) return;
        if (!gameObject.activeInHierarchy) return;

        Vector3 startPos = spinTransform.position;
        Vector3 targetPos = points[currentPoint].position;

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / moveSpeed;

        moveTween = Tween.Position(spinTransform, targetPos, duration, Ease.InOutSine)
            .OnComplete(() =>
            {
                if (!gameObject.activeInHierarchy) return;

                if (movingForward)
                {
                    currentPoint++;
                    if (currentPoint >= points.Length - 1)
                    {
                        movingForward = false;
                    }
                }
                else
                {
                    currentPoint--;
                    if (currentPoint <= 0)
                    {
                        movingForward = true;
                    }
                }
                MoveSaw();
            }, warnIfTargetDestroyed: false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var handler = gameObject.GetComponentInParent<SawHandler>();
            if (handler != null)
            {
                handler.HandleSaws(sawIndex);
            }
        }
    }

    void OnDisable()
    {
        moveTween.Stop();
        spinTween.Stop();
    }
}
