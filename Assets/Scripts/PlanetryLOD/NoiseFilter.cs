using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    Shape_Settings shapeSettings = new Shape_Settings();
    Noise noise = new Noise();

    public NoiseFilter(Shape_Settings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
    }

    public float noiseEvaluation(Vector3 point)
    {
        float noisevalue = (noise.Evaluate(point) + 1) * 0.5f;
        return noisevalue;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnSphere)
    {
        //float elaviation = (noiseEvaluation(pointOnSphere * noiseSettings.roughness + noiseSettings.center)) * noiseSettings.strength;
        //return pointOnSphere * shapeSettings.radius * (elaviation + 1f);
        float elaviation = 0;
        for (int a = 0; a < shapeSettings.noiseLayer.Length; a++)
        {
            if(shapeSettings.noiseLayer[a].enable)
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

                elaviation += noiseValue * shapeSettings.noiseLayer[a].noiseSettings.strength;
            }
        }

        
        return pointOnSphere * shapeSettings.radius * (elaviation + 1f);
    }
}
