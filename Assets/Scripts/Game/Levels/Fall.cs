using UnityEngine.SceneManagement;
using UnityEngine;

public sealed class Fall : MonoBehaviour
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
