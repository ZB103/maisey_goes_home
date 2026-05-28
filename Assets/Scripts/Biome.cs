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
        /*if (biome == BiomeType.BLUE) { return "BLUE"; }
        else if (biome == BiomeType.GREEN) { return "GREEN"; }
        else if (biome == BiomeType.WHITE) { return "WHITE"; }
        else if (biome == BiomeType.RED) { return "RED"; }
        else { return "BIOME ERR!!"; }*/
        return biome.ToString();
    }
}
