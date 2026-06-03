using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AmbientNoiseManager : MonoBehaviour
{
    private static AmbientNoiseManager instance;
    private static AmbientSoundManager asm;
    private AudioSource[] audioSources;
    private static float maxVol = .15f;
    BiomeType biome;
    BiomeType lastBiome;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        asm = GameObject.Find("AmbientSoundManager").GetComponent<AmbientSoundManager>();
        lastBiome = BiomeType.RED;  //fix later
        //blue green white red
    }

    //determine biome color
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //determine current biome
        biome = collision.gameObject.GetComponent<Biome>().getBiomeType();
        asm.biome = biome;

        //if different biome than before
        if (biome != lastBiome)
        {
            //fade out of current noise
            StartCoroutine(FadeAudio(audioSources[(int)lastBiome], maxVol, 0f, .25f));
            //fade into current noise
            StartCoroutine(FadeAudio(audioSources[(int)biome], 0f, maxVol, .25f));
        }
        //update last biome to current
        lastBiome = biome;
        asm.lastBiome = lastBiome;
    }

    //fade out/in over given period of time
    private IEnumerator FadeAudio(AudioSource aSrc, float startVol, float endVol, float dur)
    {
        //if fading in, turn on new noise
        if (!aSrc.isPlaying) { aSrc.volume = 0f; aSrc.Play(); }

        float currentTime = 0f;
        while (currentTime < dur)
        {
            currentTime += Time.deltaTime;
            aSrc.volume = Mathf.Lerp(startVol, endVol, (currentTime / dur));
            yield return 0;
        }
        aSrc.volume = endVol;

        //if faded out, turn off old noise
        if (aSrc.volume == 0f) { aSrc.Stop(); }

            yield return 1;
    }
}
