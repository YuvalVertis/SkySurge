using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Saw : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform[] points;
    int currentPoint;
    private void Update()
    {
        Move();
    }
    void Move()
    {
        Vector3 targetPoint = points[currentPoint].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % points.Length;
        }
    }
}
