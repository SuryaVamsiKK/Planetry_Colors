using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{
    public ColorSettings settings;
    public Material material;
    public Texture2D texture;
    const int resolution = 50;
    NoiseFilter biomeNoiseFilter;

    public void CreateMaterial(float min, float max)
    {
        if(texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(resolution, settings.biomeColorSettings.biomes.Length); 
        }

        if (material == null)
        {
            material = new Material(settings.shader);
        }
        else
        {
            material.shader = settings.shader;
        }

        material.SetVector("_elivationMinMax", new Vector4(min, max));
        UpdateColor();
        material.SetFloat("_specular", settings.specular);
        material.SetFloat("_smoothness", settings.smoothness);
        material.name = settings.matName;
    }

    void UpdateColor()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;
        foreach(var biomes in settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < resolution; i++)
            {
                Color grdiantColor = biomes.gradient.Evaluate(i / (resolution - 1f));
                Color tintCol = biomes.tint;

                colors[colorIndex] = grdiantColor * (1 - biomes.tintPercent) + tintCol * biomes.tintPercent;
                colorIndex++;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        material.SetTexture("_pColor", texture);
    }

    public float biomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        biomeNoiseFilter = new NoiseFilter(settings.biomeColorSettings.noise);
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;

        heightPercent += (biomeNoiseFilter.SimpleNoiseValueBiome(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrengeth;

        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendAmount / 2f + 0.00001f;
        for (int i = 0; i < numBiomes; i++)
        {
            float dst = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    private void OnValidate()
    {
        GetComponent<PlanetGenerator>().CreatePlanet();
    }

}
