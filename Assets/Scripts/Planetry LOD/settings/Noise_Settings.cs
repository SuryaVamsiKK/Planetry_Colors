using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Noise_Settings
{
    public NoiseType type;
    [Header("Noise Strength Values")]
    public float strength = 1;
    [Range(1,8)] public int numOfLayers = 1;
    public float baseRoughness = 1;
    public float roughness = 2;
    public float persistance = .5f;

    [Header("Noise Position Values")]
    public Vector3 center;
    public bool seaClamp = false;
    [ConditionalHide("seaClamp", 0)]
    public float minValue;

    [ConditionalHide("type", 1)]
    public float weightMultiplyer;
}

public enum NoiseType { Simple, Rigid }
