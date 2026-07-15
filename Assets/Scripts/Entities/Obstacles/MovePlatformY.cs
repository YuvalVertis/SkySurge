using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformY : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] float moveSpeed;
    int currentPoint;
    private void Update()
    {
        Move();
    }
    void Move()
    {
        Vector2 moveDirection = new Vector2(0, points[currentPoint].position.y).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, points[currentPoint].position) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % points.Length;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(transform);
        collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Extrapolate;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(null);
        collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
    }
}
