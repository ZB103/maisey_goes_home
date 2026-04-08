using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    RUN,
    HOBBLE,
    JUMP,
    LAND,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //play a sound of given enum type at given volume
    public static void PlaySound(SoundType sound, float vol = 1f)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], vol);
    }
}
