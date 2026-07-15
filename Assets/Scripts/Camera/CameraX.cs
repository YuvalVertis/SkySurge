using UnityEngine;

public sealed class CameraX : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] Vector3 offset;
    [SerializeField] Transform target;
    [SerializeField] float smoothFactor;
    [SerializeField] float min, max;

    void LateUpdate()
    {
        Follow();
    }
 
    void Follow()
    {
        float targetX = Mathf.Clamp(target.position.x + offset.x, min, max);
        float smoothedX = Mathf.Lerp(transform.position.x, targetX, smoothFactor * Time.deltaTime); 

        //Only follow in the x direction.
        transform.position = new Vector3(smoothedX, transform.position.y, transform.position.z);
    }
}
