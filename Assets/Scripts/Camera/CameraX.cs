using UnityEngine;
public class CameraX : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothFactor;
    [SerializeField] float min, max;

    private void LateUpdate()
    {
        Follow();
    }
 
    void Follow()
    {
        float targetX = Mathf.Clamp(target.position.x + offset.x, min, max);
        float smoothedX = Mathf.Lerp(transform.position.x, targetX, smoothFactor * Time.deltaTime); // Smoothly move towards the clamped x-coordinate
        transform.position = new Vector3(smoothedX, transform.position.y, transform.position.z);
    }
}
