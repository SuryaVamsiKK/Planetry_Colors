using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Noise_Settings
{
    public NoiseType type;

    [Header("Noise : Pattern")]
    [Range(1,8)] public int numOfLayers = 1;
    public float strength = 1;
    public float baseRoughness = 1;
    public float roughness = 2;
    public float persistance = .5f;
    [ConditionalHide("type" , 1)] public float weightMultiplyer;

    [Header("Nosie : Position")]
    public Vector3 center;
    public bool seaClamp = false;
    [ConditionalHide("seaClamp", 1)] public float minValue;

}

public enum NoiseType { Simple, Rigid }
