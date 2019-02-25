using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour {
    public GameObject chunk;

    Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    string[] names = { "Up", "Down", "Left", "Right", "Front", "Back" };
    
    [Header("Shape")] public Shape_Settings shapeSettings;
    [HideInInspector] public Material mat;
    [Header("LOD")] public LOD_Settings lodSettings;
    bool singleFace = false;
    [HideInInspector] public float min;
    [HideInInspector] public float max;

    public MinMax elevationMinMax;

    private void Awake()
    {
        shapeSettings.resolution = shapeSettings.realResolution;
    }

    void OnValidate()
    {
        CreatePlanet();
    }

    public void CreatePlanet () 
    {
        elevationMinMax = new MinMax();
        if (!singleFace)
        {       
            if (transform.childCount < 6) 
            {
                for (int i = 0; i < directions.Length; i++) 
                {
                    GameObject g = GameObject.Instantiate (chunk, this.transform) as GameObject;
                    g.transform.position -= this.transform.position;
                    g.name = "Chunk : " + names[i] + " : LOD : " + g.GetComponent<MeshGenerator>().lod;

                    MeshGenerator g_MeshGenerator =  g.GetComponent<MeshGenerator>();
                    g_MeshGenerator.mat = mat;
                    g_MeshGenerator.chunk = chunk;
                    g_MeshGenerator.localUp = directions[i];
                    g_MeshGenerator.shapeSettings = shapeSettings;
                    g_MeshGenerator.CreateShape();
                    g_MeshGenerator.UpdateMesh();
                }
            }
        
            //this only for live editing or else entire ELSE can be removed
            else
            {
                for (int i = 0; i < directions.Length; i++) 
                {
                    GameObject g = transform.GetChild (i).gameObject;
                    g.transform.position -= this.transform.position;
                    g.name = "Chunk : " + names[i] + " : LOD : " + g.GetComponent<MeshGenerator>().lod;

                    MeshGenerator g_MeshGenerator =  g.GetComponent<MeshGenerator>();
                    g_MeshGenerator.mat = mat;
                    g_MeshGenerator.chunk = chunk;
                    g_MeshGenerator.localUp = directions[i];
                    g_MeshGenerator.shapeSettings = shapeSettings;
                    g_MeshGenerator.CreateShape();
                    g_MeshGenerator.UpdateMesh();

                }
            }
        }
        else
        {       
            if (transform.childCount < 1) 
            {
                for (int i = 0; i < 1; i++) 
                {
                    GameObject g = GameObject.Instantiate (chunk, this.transform) as GameObject;
                    g.transform.position -= this.transform.position;
                    g.name = "Chunk : " + names[i] + " : LOD : " + g.GetComponent<MeshGenerator>().lod;

                    MeshGenerator g_MeshGenerator =  g.GetComponent<MeshGenerator>();
                    g_MeshGenerator.mat = mat;
                    g_MeshGenerator.chunk = chunk;
                    g_MeshGenerator.localUp = directions[i];
                    g_MeshGenerator.shapeSettings = shapeSettings;
                    g_MeshGenerator.CreateShape();
                    g_MeshGenerator.UpdateMesh();
                }
            }
        
            //this only for live editing or else entire ELSE can be removed
            else
            {
                for (int i = 0; i < 1; i++) 
                {
                    GameObject g = transform.GetChild (i).gameObject;
                    g.transform.position -= this.transform.position;
                    g.name = "Chunk : " + names[i] + " : LOD : " + g.GetComponent<MeshGenerator>().lod;

                    MeshGenerator g_MeshGenerator =  g.GetComponent<MeshGenerator>();
                    g_MeshGenerator.mat = mat;
                    g_MeshGenerator.chunk = chunk;
                    g_MeshGenerator.localUp = directions[i];
                    g_MeshGenerator.shapeSettings = shapeSettings;
                    g_MeshGenerator.CreateShape();
                    g_MeshGenerator.UpdateMesh();

                }
            }            
        }

        min = elevationMinMax.Min;
        max = elevationMinMax.Max;

        GetComponent<ColorGenerator>().CreateMaterial(min, max);
        mat = GetComponent<ColorGenerator>().material;
    }
}