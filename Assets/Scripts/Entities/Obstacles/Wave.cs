using UnityEngine;
using UnityEngine.SceneManagement;
public class Wave : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float growthRate;
    private void Update()
    {
        transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
        transform.localScale += new Vector3(0f, growthRate * Time.deltaTime, 0f);
    }
}
