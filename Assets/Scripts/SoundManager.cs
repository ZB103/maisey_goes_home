using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundType
{
    RUN,
    HOBBLE,
    JUMP,
    LAND,
    FALL,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList; //array of arrays of sounds
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
        //get randomly chosen sound clip
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        //play sound clip
        instance.audioSource.PlayOneShot(randClip, vol);
    }

//give elements in inspector names
#if UNITY_EDITOR
    private void OnEnable()
    {
        //assign name of element array to name of soundtype enum
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    //getter
    public AudioClip[] Sounds { get => sounds; }
    //name for inspector
    [HideInInspector] public string name;
    //list of sound clips
    [SerializeField] private AudioClip[] sounds;
}
