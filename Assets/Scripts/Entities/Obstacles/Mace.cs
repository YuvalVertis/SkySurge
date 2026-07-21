using UnityEngine;

public sealed class Mace : MonoBehaviour
{
    [Header("Scaling")]
    [SerializeField] Vector3 newSize = new Vector3(1, 1, 1);
    [SerializeField] float duration;

    void Start()
    {
        var manager = EffectsManager.Instance;
        if (manager != null)
        {
            manager.Scale(transform, newSize, duration, true);
        }
    }
}