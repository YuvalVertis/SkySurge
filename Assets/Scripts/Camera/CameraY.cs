using UnityEngine;

public class CameraY : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothFactor;
    [SerializeField] float minY, maxY;
    private void LateUpdate()
    {
        Follow();
    }
    void Follow()
    {
        float clampedY = Mathf.Clamp(target.position.y + offset.y, minY, maxY); // Clamp target position y-coordinate
        float smoothedY = Mathf.Lerp(transform.position.y, clampedY, smoothFactor * Time.deltaTime); // Smoothly move towards the clamped y-coordinate
        transform.position = new Vector3(transform.position.x, smoothedY, transform.position.z);
    }
}
