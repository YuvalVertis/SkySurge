using UnityEngine;
using System;

public sealed class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio source")]
    [SerializeField] AudioSource sfxSource;

    [Header("Sounds")]
    [SerializeField] AudioInfo[] sfxData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlaySfx(SfxType type)
    {
        if (sfxSource == null) return;

        int index = (int)type;
        if (index < 0 || index >= sfxData.Length) return;

        AudioInfo audio = sfxData[index];
        if (audio == null || audio.clip == null) return;

        sfxSource.pitch = audio.pitch;
        sfxSource.PlayOneShot(audio.clip, audio.volume);
    }
}

[Serializable]
public class AudioInfo
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;
}
