using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
    }
}
