using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorSettings
{
    public Shader shader;
    public Gradient planetColor;
    [Range(0, 1)] public float specular;
    [Range(0, 1)] public float smoothness;
    public string matName;
}
