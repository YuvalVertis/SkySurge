using UnityEngine;

public sealed class RunAnimS : MonoBehaviour
{
    bool tracker;
    [SerializeField] Animator anim;
    [SerializeField] float speed;

    void Awake()
    {
        if(!tracker)
        {
            anim.SetFloat("Speed", speed);
        }
    }
}
