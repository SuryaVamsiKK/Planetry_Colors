using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour {
    public GameObject chunk;

    Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    string[] names = { "Up", "Down", "Left", "Right", "Front", "Back" };
    
    [Header("Shape")] public Shape_Settings shapeSettings;
    public Material mat;
    [Header("LOD")] public LOD_Settings lodSettings;

    //public ColorGenerator colorGenerator;

    private void OnValidate()
    {
        //mat = GetComponent<ColorSettings>().mat;
        CreatePlanet();
    }

    public void CreatePlanet()
    {
        //colorGenerator = new ColorGenerator(GetComponent<ColorSettings>());
        if (transform.childCount < 6)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                GameObject g = GameObject.Instantiate(chunk, this.transform) as GameObject;
                g.transform.position -= this.transform.position;
                g.name = "Chunk : " + names[i] + " : LOD : " + g.GetComponent<MeshGenerator>().lod;

                MeshGenerator g_MeshGenerator = g.GetComponent<MeshGenerator>();
                //g_MeshGenerator.mat = GetComponent<ColorSettings>().mat;
                g_MeshGenerator.chunk = chunk;
                g_MeshGenerator.localUp = directions[i];
                g_MeshGenerator.shapeSettings = shapeSettings;
                g_MeshGenerator.CreateShape();
                //colorGenerator.UpdateElaviation(g_MeshGenerator.noiseFilter.elivationMinMax);
                g_MeshGenerator.UpdateMesh();
            }
        }

        //this only for live editing or else entire ELSE can be removed
        else
        {
            for (int i = 0; i < directions.Length; i++)
            {
                GameObject g = transform.GetChild(i).gameObject;
                g.transform.position -= this.transform.position;
                g.name = "Chunk : " + names[i] + " : LOD : " + g.GetComponent<MeshGenerator>().lod;

                MeshGenerator g_MeshGenerator = g.GetComponent<MeshGenerator>();
                //g_MeshGenerator.mat = GetComponent<ColorSettings>().mat;
                g_MeshGenerator.chunk = chunk;
                g_MeshGenerator.localUp = directions[i];
                g_MeshGenerator.shapeSettings = shapeSettings;
                g_MeshGenerator.CreateShape();
                //colorGenerator.UpdateElaviation(g_MeshGenerator.noiseFilter.elivationMinMax);
                g_MeshGenerator.UpdateMesh();
            }
        }
    }
}