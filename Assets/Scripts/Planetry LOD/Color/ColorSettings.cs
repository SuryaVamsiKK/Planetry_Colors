using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorSettings
{
    public Shader shader;
    public BiomeSettings biomeColorSettings;
    [Range(0, 1)] public float specular;
    [Range(0, 1)] public float smoothness;
    public string matName;

    [System.Serializable]
    public class BiomeSettings
    {
        public Biome[] biomes;
        public Noise_Settings noise;
        public float noiseOffset;
        public float noiseStrengeth;
        [Range(0,1)]public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range (0 ,1)] public float startHeight;
            [Range(0, 1)] public float tintPercent;
        }
    }

}
