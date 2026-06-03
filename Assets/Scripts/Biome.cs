using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomeType
{
    BLUE,
    GREEN,
    WHITE,
    RED
}

public class Biome : MonoBehaviour
{
    public BiomeType biome;

    public BiomeType getBiomeType() { return biome; }

    public string getBiomeString()
    {
        return biome.ToString();
    }
}
