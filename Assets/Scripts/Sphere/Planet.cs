using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Material mat;
   [Range(2, 256)] public int resolution = 10;
   public Vector2 planePos;
   [SerializeField, HideInInspector]
   MeshFilter[] meshFilters;
   TerrianFace[] terrrianFaces;
   [Range(0f, 1f)] public float distanceBwVerts;
    public bool excute;
    Vector3 averagePos;
    void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }

   void Initialize()
   {
       if(meshFilters == null || meshFilters.Length == 0)
       {
           meshFilters = new MeshFilter[6];
       }

       terrrianFaces = new TerrianFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

       for (int i = 0; i < 6; i++)
       {
           if(meshFilters[i] == null)
           {
               GameObject meshObj = new GameObject("mesh");
               meshObj.transform.parent = transform;
               meshObj.AddComponent<MeshRenderer>().sharedMaterial = mat;
               meshFilters[i] = meshObj.AddComponent<MeshFilter>();
               meshFilters[i].sharedMesh = new Mesh();     
           }
           else
           {
               meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = mat;
           }

           terrrianFaces[i] = new TerrianFace(meshFilters[i].sharedMesh, resolution, directions[i], distanceBwVerts, planePos, meshFilters[i].gameObject.transform);
           
            //Debug.Log(CalculateCenter(meshFilters[i].sharedMesh.vertices));
            //meshFilters[i].gameObject.transform.localPosition = CalculateCenter(meshFilters[i].sharedMesh.vertices);

       }
   }

   void GenerateMesh()
   {
       foreach (TerrianFace face in terrrianFaces)
       {           
           face.ConstructMesh();
       }
   }   
}
