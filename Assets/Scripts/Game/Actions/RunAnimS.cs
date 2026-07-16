using UnityEngine;

public sealed class RunAnimS : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float speed;

    void Awake()
    {
        anim.SetFloat(CodesManager.Run, speed);
    }
}
