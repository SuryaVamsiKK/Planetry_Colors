using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    Shape_Settings shapeSettings = new Shape_Settings();
    Noise noise = new Noise();
    Noise_Settings noiseSettings;

    public NoiseFilter(Shape_Settings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
    }

    public NoiseFilter(Noise_Settings noiseSettings)
    {
        this.noiseSettings = noiseSettings;
    }

    public float noiseEvaluation(Vector3 point)
    {
        float noisevalue = (noise.Evaluate(point) + 1) * 0.5f;
        return noisevalue;
    }

    public float noiseEvaluationBiome(Vector3 point)
    {
        float noisevalue = (noise.Evaluate(point) + 1) * 0.5f;
        return noisevalue;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnSpher, Vector3 Pos, out float elevation)
    {
        //float elaviation = (noiseEvaluation(pointOnSphere * noiseSettings.roughness + noiseSettings.center)) * noiseSettings.strength;
        //return pointOnSphere * shapeSettings.radius * (elaviation + 1f);
        float elaviation = 0;
        Vector3 pointOnSphere = pointOnSpher;
        for (int a = 0; a < shapeSettings.noiseLayer.Length; a++)
        {
            if(shapeSettings.noiseLayer[a].enable)
            {
                switch (shapeSettings.noiseLayer[a].noiseSettings.type)
                {
                    case NoiseType.Simple:
                        elaviation += SimpleNoiseValue(a, pointOnSphere) * shapeSettings.noiseLayer[a].noiseSettings.strength;
                        break;
                    case NoiseType.Rigid:
                        elaviation += RighidNoiseValue(a, pointOnSphere) * shapeSettings.noiseLayer[a].noiseSettings.strength;
                        break;
                    default:
                        break;
                }
            }
        }

        elaviation = shapeSettings.radius * (elaviation + 1f);
        elevation = elaviation - ((shapeSettings.radius + 1f)/10f);
        return pointOnSphere * elaviation;
    }

    public float RighidNoiseValue(int a, Vector3 pointOnSphere)
    {
        float noiseValue = 0;
        float frequency = shapeSettings.noiseLayer[a].noiseSettings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < shapeSettings.noiseLayer[a].noiseSettings.numOfLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(pointOnSphere * frequency + shapeSettings.noiseLayer[a].noiseSettings.center));
            v *= v;
            v *= weight;
            weight = v * shapeSettings.noiseLayer[a].noiseSettings.weightMultiplyer;

            noiseValue += v * amplitude;
            frequency *= shapeSettings.noiseLayer[a].noiseSettings.roughness;
            amplitude *= shapeSettings.noiseLayer[a].noiseSettings.persistance;
        }

        if (shapeSettings.noiseLayer[a].noiseSettings.seaClamp)
        {
            noiseValue = Mathf.Max(0, noiseValue - shapeSettings.noiseLayer[a].noiseSettings.minValue);
        }

        return noiseValue;
    }

    public float SimpleNoiseValue(int a, Vector3 pointOnSphere)
    {
        float noiseValue = 0;
        float frequency = shapeSettings.noiseLayer[a].noiseSettings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < shapeSettings.noiseLayer[a].noiseSettings.numOfLayers; i++)
        {
            float v = noise.Evaluate(pointOnSphere * frequency + shapeSettings.noiseLayer[a].noiseSettings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= shapeSettings.noiseLayer[a].noiseSettings.roughness;
            amplitude *= shapeSettings.noiseLayer[a].noiseSettings.persistance;
        }

        if (shapeSettings.noiseLayer[a].noiseSettings.seaClamp)
        {
            noiseValue = Mathf.Max(0, noiseValue - shapeSettings.noiseLayer[a].noiseSettings.minValue);
        }

        return noiseValue;
    }

    public float SimpleNoiseValueBiome(Vector3 pointOnSphere)
    {
        float noiseValue = 0;
        float frequency = noiseSettings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < noiseSettings.numOfLayers; i++)
        {
            float v = noise.Evaluate(pointOnSphere * frequency + noiseSettings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= noiseSettings.roughness;
            amplitude *= noiseSettings.persistance;
        }

        if (noiseSettings.seaClamp)
        {
            noiseValue = Mathf.Max(0, noiseValue - noiseSettings.minValue);
        }

        return noiseValue;
    }
}
