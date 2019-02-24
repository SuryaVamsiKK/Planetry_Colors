using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shape_Settings
{
    [Range(1, 256)] public int resolution = 10;
    public float radius;
    [Header("Noise")] public NoiseLayer[] noiseLayer;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enable = true;
        public Noise_Settings noiseSettings;
    }
}
