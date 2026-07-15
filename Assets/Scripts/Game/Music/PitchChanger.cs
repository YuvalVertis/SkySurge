using UnityEngine;

public sealed class PitchChanger : MonoBehaviour
{
    [SerializeField] float add;
    AudioSource audioS;
    bool changed;

    void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Application.isFocused && !audioS.isPlaying && !changed)
        {
            audioS.pitch += add;
            audioS.Play();
            changed = true;
        }
    }
}
