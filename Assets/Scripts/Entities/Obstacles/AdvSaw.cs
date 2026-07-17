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


    void Start()
    {
        EffectsManager.Instance.Spin(spinTransform, spinDuration, true);
    }

    void Update()
    {
        Vector3 targetPoint = points[currentPoint].position;
        spinTransform.position = Vector3.MoveTowards(spinTransform.position, targetPoint, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(spinTransform.position, targetPoint) < 0.1f)
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
        }
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
