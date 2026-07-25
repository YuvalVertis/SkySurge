using UnityEngine;

public sealed class Wave : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float growthRate;
    [SerializeField] float spinDuration;
    [SerializeField] bool spin;

    void Start()
    {
        if(spin && EffectsManager.Instance != null)
        {
            EffectsManager.Instance.Spin(transform, spinDuration, true);            
        }
    }
    void Update()
    {
        transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
        transform.localScale += new Vector3(0f, growthRate * Time.deltaTime, 0f);
    }
}
