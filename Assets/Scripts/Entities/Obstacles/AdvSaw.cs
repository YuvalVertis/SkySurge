using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AdvSaw : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform[] points;
    [SerializeField] bool rotate;
    float defaultSpeed;
    int currentPoint;
    bool movingForward = true;
    private void Awake()
    {
        defaultSpeed = moveSpeed;
    }
    private void Update()
    {
        Vector3 targetPoint = points[currentPoint].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            if (currentPoint == points.Length / 2 && rotate)
            {
                StartCoroutine(Rotate());
            }
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
    IEnumerator Rotate()
    {
        float duration = 0.5f;
        float time = 0f;
        moveSpeed = 0;
        while (time < duration)
        {
            transform.Rotate(0, 0, -360 * Time.deltaTime * 3);
            time += Time.deltaTime;
            yield return null;
        }
        moveSpeed = defaultSpeed;
    }
}
