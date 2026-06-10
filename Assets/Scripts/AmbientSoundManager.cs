using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
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
    WIND,
    CARDINAL,
}

public enum WhiteAmbientSounds
{
    WIND,
}

public enum RedAmbientSounds
{
    WIND,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AmbientSoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] blueSoundList; //array of arrays of sounds
    [SerializeField] private SoundList[] greenSoundList; //array of arrays of sounds
    [SerializeField] private SoundList[] whiteSoundList; //array of arrays of sounds
    [SerializeField] private SoundList[] redSoundList; //array of arrays of sounds
    public List<object[]> soundIntervalTracker; //tracker of time until next sound played - {sound, timeLeft}
    private static AmbientSoundManager instance;
    private static AmbientNoiseManager anm;
    private AudioSource[] audioSources;

    private float[][] BlueIntervals =
{
    new float[] {0,0},     //CICADAS,
    new float[] {60,70},    //THUNDER,
    new float[] {1,5},      //DRIP,
    new float[] {25,35},     //SWISH,
    new float[] {30,60},    //STARLING,
    new float[] {30,60},    //MAGPIE,
    new float[] {30,60},    //DOVE,
    new float[] {30,60},    //DUCK,
};

    private float[][] GreenIntervals =
{
    new float[] {15,45},     //WIND,
    new float[] {10,20},    //CARDINAL,
};

    private float[][] WhiteIntervals =
{
    new float[] {15,45},     //WIND,
};

    private float[][] RedIntervals =
{
    new float[] {15,45},     //WIND,
};

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        anm = GameObject.Find("AmbientNoiseManager").GetComponent<AmbientNoiseManager>();
        soundIntervalTracker = new List<object[]>();
        //blue green white red
    }

    private void FixedUpdate()
    {
        //if the queue is empty, fill it with each sound
        if (soundIntervalTracker.Count == 0)
        {
            switch (anm.biome)
            {
                case BiomeType.BLUE:
                    for (int i = 0; i < BlueIntervals.Length; i++)
                    {
                        BlueAmbientSounds name = (BlueAmbientSounds)i;
                        float interval = UnityEngine.Random.Range(BlueIntervals[i][0], BlueIntervals[i][1]);
                        object[] pair = { name, interval };
                        soundIntervalTracker.Add(pair);
                    }
                    break;

            case BiomeType.GREEN:
                for (int i = 0; i < GreenIntervals.Length; i++)
                {
                    GreenAmbientSounds name = (GreenAmbientSounds)i;
                    float interval = UnityEngine.Random.Range(GreenIntervals[i][0], GreenIntervals[i][1]);
                    object[] pair = { name, interval };
                        soundIntervalTracker.Add(pair);
                    }
                break;

            case BiomeType.WHITE:
                for (int i = 0; i < WhiteIntervals.Length; i++)
                {
                    WhiteAmbientSounds name = (WhiteAmbientSounds)i;
                    float interval = UnityEngine.Random.Range(WhiteIntervals[i][0], WhiteIntervals[i][1]);
                    object[] pair = { name, interval };
                    soundIntervalTracker.Add(pair);
                    }
                break;

            case BiomeType.RED:
                for (int i = 0; i < RedIntervals.Length; i++)
                {
                    RedAmbientSounds name = (RedAmbientSounds)i;
                    float interval = UnityEngine.Random.Range(RedIntervals[i][0], RedIntervals[i][1]);
                    object[] pair = { name, interval };
                    soundIntervalTracker.Add(pair);
                    }
                break;
            }
        }

        for(int i = 0; i < soundIntervalTracker.Count; i++)
        {
            //tick down all counters
            soundIntervalTracker[i][1] = (float)soundIntervalTracker[i][1] - Time.deltaTime;

            //if a timer has reached 0, play the sound and re-roll interval
            if((float)soundIntervalTracker[i][1] <= 0f)
            {
                PlaySound((int)soundIntervalTracker[i][0]);
                float interval = 10;
                switch (anm.biome)
                {
                    case BiomeType.BLUE:
                        interval = UnityEngine.Random.Range(BlueIntervals[i][0], BlueIntervals[i][1]);
                        break;
                    case BiomeType.GREEN:
                        interval = UnityEngine.Random.Range(GreenIntervals[i][0], GreenIntervals[i][1]);
                        break;
                    case BiomeType.WHITE:
                        interval = UnityEngine.Random.Range(WhiteIntervals[i][0], WhiteIntervals[i][1]);
                        break;
                    case BiomeType.RED:
                        interval = UnityEngine.Random.Range(RedIntervals[i][0], RedIntervals[i][1]);
                        break;
                }
                soundIntervalTracker[i][1] = interval;
            }
        }

        //print(soundIntervalTracker[0][0] + " : " + soundIntervalTracker[0][1]);
    }

    //at random interval, play random sound
    private static void PlaySound(int sound, float vol = .4f)
    {
        //get randomly chosen sound clip
        AudioClip[] clips;
        switch (anm.biome)
        {
            case BiomeType.BLUE:
                clips = instance.blueSoundList[sound].Sounds;
                break;
            case BiomeType.GREEN:
                clips = instance.greenSoundList[sound].Sounds;
                break;
            case BiomeType.WHITE:
                clips = instance.whiteSoundList[sound].Sounds;
                break;
            case BiomeType.RED:
                clips = instance.redSoundList[sound].Sounds;
                break;
            default:
                clips = Array.Empty<AudioClip>();
                break;
        }
        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        //play sound clip
        //attempts to play in src 1. If busy tries src 2, if also busy tries src 3. If all are busy, doesn't play
        if (!instance.audioSources[0].isPlaying) { instance.audioSources[0].PlayOneShot(randClip, vol); }
        else if (!instance.audioSources[1].isPlaying) { instance.audioSources[1].PlayOneShot(randClip, vol); }
        else if (!instance.audioSources[2].isPlaying) { instance.audioSources[2].PlayOneShot(randClip, vol); }
    }

//give elements in inspector
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

        //white sounds list
        names = Enum.GetNames(typeof(WhiteAmbientSounds));
        Array.Resize(ref whiteSoundList, names.Length);
        for (int i = 0; i < whiteSoundList.Length; i++)
        {
            whiteSoundList[i].name = names[i];
        }

        //red sounds list
        names = Enum.GetNames(typeof(RedAmbientSounds));
        Array.Resize(ref redSoundList, names.Length);
        for (int i = 0; i < redSoundList.Length; i++)
        {
            greenSoundList[i].name = names[i];
        }
    }
#endif
    
}
