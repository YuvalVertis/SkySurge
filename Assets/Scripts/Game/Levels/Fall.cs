using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 fallCords;
    void Update()
    {
        if (target.position.y <= fallCords.y)
        {
            SceneManager.LoadScene("Levels");
        }
    }
}
