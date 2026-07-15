using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] float moveSpeed;
    int currentPoint;
    private void Update()
    {
        Vector3 targetPoint = points[currentPoint].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % points.Length;
        }
    }
}
