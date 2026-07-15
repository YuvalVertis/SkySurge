using UnityEngine;

public sealed class MovePlatform : MonoBehaviour
{
    [SerializeField] Transform[] points;
    public float moveSpeed;
    public float speed;
    public bool clouded = false;
    int currentPoint;

    void Awake()
    {
        speed = -moveSpeed;
    }
    void Update()
    {
        Move();
    }
    void Move()
    {
        if (points.Length == 0)
        {
            return;
        }
        Vector2 moveDirection = new Vector2(points[currentPoint].position.x, 0).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, points[currentPoint].position) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % points.Length;
            speed *= -1;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        clouded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        clouded = false;
    }
}