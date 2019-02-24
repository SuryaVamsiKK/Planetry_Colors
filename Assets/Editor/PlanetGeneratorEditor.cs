using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanetGenerator))]
public class PlanetGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlanetGenerator planetGenerator = (PlanetGenerator)target;
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if(GUILayout.Button("Recreate"))
        {
            planetGenerator.CreatePlanet();
        }
    }
}
