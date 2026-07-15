using UnityEngine;

public sealed class Rotate : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
    }
}
