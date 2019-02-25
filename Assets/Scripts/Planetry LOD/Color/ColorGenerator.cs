using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{
    public ColorSettings settings;
    public Material material;
    public Texture2D texture;
    const int resolution = 50;

    public void CreateMaterial(float min, float max)
    {

        if(texture == null)
        {
            texture = new Texture2D(resolution, 1); 
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
        material.name = settings.matName;
    }

    void UpdateColor()
    {
        Color[] colors = new Color[resolution];
        for (int i = 0; i < resolution; i++)
        {
            colors[i] = settings.planetColor.Evaluate(i / (resolution - 1f));
        }
        texture.SetPixels(colors);
        texture.Apply();
        material.SetTexture("_pColor", texture);
    }
}
