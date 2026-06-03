using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum BlueAmbientSounds
{
    CICADAS,
    THUNDER,
    DRIP,
    SWISH,
    STARLING,
    MAGPIE,
    DOVE,
    DUCK,
}

public enum GreenAmbientSounds
{
    CARDINAL,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AmbientSoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] blueSoundList; //array of arrays of sounds
    [SerializeField] private SoundList[] greenSoundList; //array of arrays of sounds
    private static AmbientSoundManager instance;
    private AudioSource audioSource;
    public BiomeType biome;
    public BiomeType lastBiome;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //at random interval, play random sound
    public static void PlaySound(SoundType sound, float vol = 1f)
    {
        //get randomly chosen sound clip
        //AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        //AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        //play sound clip
        //instance.audioSource.PlayOneShot(randClip, vol);
    }

    //give elements in inspector names
    
#if UNITY_EDITOR
    private void OnEnable()
    {
        //blue sounds list
        string[] names = Enum.GetNames(typeof(BlueAmbientSounds));
        Array.Resize(ref blueSoundList, names.Length);
        for (int i = 0; i < blueSoundList.Length; i++)
        {
            blueSoundList[i].name = names[i];
        }

        //green sounds list
        names = Enum.GetNames(typeof(GreenAmbientSounds));
        Array.Resize(ref greenSoundList, names.Length);
        for (int i = 0; i < greenSoundList.Length; i++)
        {
            greenSoundList[i].name = names[i];
        }
    }
#endif
    
}
