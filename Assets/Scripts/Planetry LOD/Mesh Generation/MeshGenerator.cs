using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    #region LOD

    [HideInInspector] public int lod = 0;
    [HideInInspector] public GameObject chunk;
    [HideInInspector] public int quad_Location;
    [HideInInspector] public Vector2 planerRefrence;

    #endregion

    #region Dimensions

    [HideInInspector] public float side;
    [HideInInspector] public Shape_Settings shapeSettings;
    [HideInInspector] public Vector3 localUp = Vector3.up;
    Vector3 axisA;
    Vector3 axisB;

    #endregion

    #region noise
    [HideInInspector] public NoiseFilter noiseFilter;

    #endregion

    #region Mesh

    Vector3[] verts;
    int[] triangles;   
    Mesh mesh;
    public Material mat;

    #endregion

    private void Update()
    {
        if(lod <= 1)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void CreateShape()
    {
        #region Initalization

        noiseFilter = new NoiseFilter(shapeSettings);
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);

        mesh = new Mesh();
        verts = new Vector3[(shapeSettings.resolution + 1) * (shapeSettings.resolution + 1)];

        triangles = new int[(shapeSettings.resolution) * (shapeSettings.resolution) * 6]; 
        int vert = 0; 
        int triIndex = 0;

        #endregion

        for (int x = 0,i = 0; x <= shapeSettings.resolution; x++) 
        {
            for (int z = 0; z <= shapeSettings.resolution; z++)
            {

                #region Adjustment of valuse according to the LOD to implement on vertices.

                float scale = 2f;
                float pos = -0.5f;
                scale = (2/(Mathf.Pow(2,lod)));
                pos = Mathf.Pow(2, lod - 1) - 1;

                #endregion

                Vector2 localVert = (new Vector2(x, z) / shapeSettings.resolution);
                if(lod > 0)
                {
                    #region Vertices based on Quad Tree position of the plane 
                    if (quad_Location == 0)
                    {                        
                        planerRefrence = new Vector2(0f,0f) + (transform.parent.GetComponent<MeshGenerator>().planerRefrence * 2); 
                    }
                    if(quad_Location == 1)
                    {                        
                        planerRefrence = new Vector2(1f,0f) + (transform.parent.GetComponent<MeshGenerator>().planerRefrence * 2);
                    }
                    if(quad_Location == 2)
                    {                        
                        planerRefrence = new Vector2(0f,1f) + (transform.parent.GetComponent<MeshGenerator>().planerRefrence * 2);
                    }
                    if(quad_Location == 3)
                    {                        
                        planerRefrence = new Vector2(1f,1f) + (transform.parent.GetComponent<MeshGenerator>().planerRefrence * 2);
                    }
                    
                    localVert = (new Vector2(x, z) / shapeSettings.resolution) - planerRefrence;
                    #endregion
                }

                #region Vertices

                float elevaltion = 0;
                verts[i] = (localUp + (localVert.x + pos) * scale * axisA - (localVert.y + pos) * scale * axisB);
                verts[i] = verts[i].normalized;               
                verts[i] = noiseFilter.CalculatePointOnPlanet(verts[i], out elevaltion);

                if(lod == 0)
                {
                   transform.parent.GetComponent<PlanetGenerator>().elevationMinMax.AddValue(elevaltion);
                }

                //Transform root = transform.parent;
                //for (int a = 0; a < lod; a++)
                //{
                //    root = transform.parent;
                //}
                //root.GetComponent<PlanetGenerator>().elevationMinMax.AddValue(elevaltion);


                i++;

                #endregion

                if (x!= shapeSettings.resolution && z!= shapeSettings.resolution)
                {
                    #region Triagnles

                    triangles[triIndex + 0] = vert + shapeSettings.resolution + 2;
                    triangles[triIndex + 1] = vert + shapeSettings.resolution + 1;
                    triangles[triIndex + 2] = vert + 1;
                    triangles[triIndex + 3] = vert + 1;
                    triangles[triIndex + 4] = vert + shapeSettings.resolution + 1;
                    triangles[triIndex + 5] = vert + 0;

                    #endregion

                    vert++;
                    triIndex += 6;
                }
            }

            if(x!= shapeSettings.resolution)
            {                
                vert++;
            }
        }
    }

    public void UpdateMesh()
    {
        CalculateCenter(verts);
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = mat;
    }

    void CalculateCenter(Vector3[] verts)
    {
        #region To center the pivot object
        //Vector3 sum = Vector3.zero;
        //for (int i = 0; i < verts.Length; i++)
        //{
        //    sum += verts[i];
        //}

        //this.transform.position = (sum/verts.Length) + transform.root.position;
        #endregion

        transform.position = verts[(verts.Length / 2)]; // To put the pivot at the middle vertice.
       
       for (int i = 0; i < verts.Length; i++)
       {
            verts[i] = verts[i] - this.transform.position + transform.root.position;        
       }

    }
}
