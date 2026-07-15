using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchChanger : MonoBehaviour
{
    [SerializeField] float add;
    AudioSource audioS;
    bool changed;
    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Application.isFocused && !audioS.isPlaying && !changed)
        {
            audioS.pitch += add;
            audioS.Play();
            changed = true;
        }
    }
}
