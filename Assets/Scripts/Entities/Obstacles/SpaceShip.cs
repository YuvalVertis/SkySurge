using UnityEngine;

public sealed class SpaceShip : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSpeed, flySpeed, acceleration, duration;
    float currentSpeed, accelerationTimer;
    [SerializeField] Transform[] points;
    int currentPoint = 0;
    float defaultPos1, defaultPos2;
    bool stagedUp = false;

    void Awake()
    {
        currentSpeed = flySpeed;
        defaultPos1 = points[0].position.x;
        defaultPos2 = points[1].position.x;
    }

    void Update()
    { 
        if(!stagedUp)
        {
            Stage1();
        }
        if(transform.position.y > 87.5f)
        {
            stagedUp = true;
        }
        if(stagedUp)
        {
            Stage2();
        }
    }
    void Stage1()
    {
        if (accelerationTimer < duration)
        {
            currentSpeed += acceleration * Time.deltaTime;
            accelerationTimer += Time.deltaTime;
        }
        Vector2 newPosition = transform.position + Vector3.up * currentSpeed * Time.deltaTime;
        transform.position = newPosition;
        Vector3 targetPoint = points[currentPoint].position;
        transform.position = Vector3.Lerp(transform.position, targetPoint, followSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.125f)
        {
            currentPoint = (currentPoint + 1) % points.Length;
        }
        points[0].position = new Vector2(defaultPos1, points[0].position.y);
        points[1].position = new Vector2(defaultPos2, points[1].position.y);
    }

    void Stage2()
    {
        float targetX = Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime);
        transform.position = new Vector2(targetX, transform.position.y);
    }
}
