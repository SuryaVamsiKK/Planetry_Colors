using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Noise_Settings
{
    public float strength = 1;
    [Range(1,8)] public int numOfLayers = 1;
    public float baseRoughness = 1;
    public float roughness = 2;
    public float persistance = .5f;
    public Vector3 center;
    public bool seaClamp = false;
    public float minValue;
    public NoiseType type;

    public float weightMultiplyer;
}

public enum NoiseType { Simple, Rigid }
