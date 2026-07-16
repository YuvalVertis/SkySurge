using UnityEngine;

public sealed class AdvSaw : MonoBehaviour
{
    [SerializeField] float spinDuration;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] points;
    bool movingForward = true;
    int currentPoint;

    void Start()
    {
        EffectsManager.Instance.Spin(transform, spinDuration, -1);
    }

    void Update()
    {
        Vector3 targetPoint = points[currentPoint].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            if (movingForward)
            {
                currentPoint++;
                if (currentPoint == points.Length - 1)
                {
                    movingForward = false;
                }
            }
            else
            {
                currentPoint--;
                if (currentPoint == 0)
                {
                    movingForward = true;
                }
            }
        }
    }
}
