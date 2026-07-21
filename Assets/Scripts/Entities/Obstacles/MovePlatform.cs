using UnityEngine;

public sealed class MovePlatform : MonoBehaviour
{
    [SerializeField] Transform[] points;
    public float moveSpeed = 3f;
    int currentPoint;

    public Vector2 velocity { get; private set; }
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (points == null || points.Length < 2)
        {
            velocity = Vector2.zero;
            return;
        }

        Vector2 currentPos = rb.position;
        Vector2 targetPos = points[currentPoint].position;

        Vector2 direction = (targetPos - currentPos).normalized;
        velocity = direction * moveSpeed;

        Vector2 nextPos = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);

        if (Vector2.Distance(currentPos, targetPos) < 0.05f)
        {
            currentPoint = (currentPoint + 1) % points.Length;
        }
    }
}