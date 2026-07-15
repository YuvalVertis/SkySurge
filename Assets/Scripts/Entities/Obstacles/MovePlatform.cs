using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] Transform[] points;
    public float moveSpeed;
    public float speed;
    public bool clouded = false;
    int currentPoint;
    private void Awake()
    {
        speed = -moveSpeed;
    }
    private void Update()
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.CompareTag("Player");
        clouded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.CompareTag("Player");
        clouded = false;
    }
}