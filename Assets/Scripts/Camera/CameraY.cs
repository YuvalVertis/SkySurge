using UnityEngine;

public sealed class CameraY : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] Transform target;
    [SerializeField] float smoothFactor;
    [SerializeField] float minY, maxY;
    [SerializeField] bool followPlayer = true;
    public Vector3 offset;

    void LateUpdate()
    {
        if (!followPlayer) return;
        Follow();
    }

    void Follow()
    {
        float clampedY = Mathf.Clamp(target.position.y + offset.y, minY, maxY);
        float smoothedY = Mathf.Lerp(transform.position.y, clampedY, smoothFactor * Time.deltaTime);

        //Only follow in the y direction
        transform.position = new Vector3(transform.position.x, smoothedY, transform.position.z);
    }
}
