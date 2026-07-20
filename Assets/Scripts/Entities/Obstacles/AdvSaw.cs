using UnityEngine;
using PrimeTween;
public sealed class AdvSaw : MonoBehaviour
{
    [SerializeField] float spinDuration;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] points;
    [SerializeField] Transform spinTransform;
    [SerializeField] int sawIndex;
    bool movingForward = true;
    int currentPoint;


    void Start()
    {
        if (EffectsManager.Instance != null)
        {
            EffectsManager.Instance.Spin(spinTransform, spinDuration, true);

        }
        MoveSaw();
    }

    void MoveSaw()
    {
        Vector3 startPos = spinTransform.position;
        Vector3 targetPos = points[currentPoint].position;

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / moveSpeed;

        Tween.Position(spinTransform, targetPos, duration, Ease.InOutSine)
            .OnComplete(() =>
            {
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
            handler.HandleSaws(sawIndex);
        }
    }
}
