using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshGenerator : MonoBehaviour
{
    #region LOD

    [HideInInspector] public int lod = 0;
    [HideInInspector] public GameObject chunk;
    [HideInInspector] public int quad_Location;
    [HideInInspector] public Vector2 planerRefrence;

    #endregion

    #region Dimensions

    [HideInInspector] public ColorGenerator colorGenerator;
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
    Vector2[] uvs;
    public Material mat;

    #endregion

    public Transform planetCore;
    public Transform mainFace;

    private void UpdateColor()
    {
        if(lod <= 1)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void CreateShape()
    {

        if (lod > 0)
        {
            mainFace = transform.parent;            
        }

        #region Initalization

        noiseFilter = new NoiseFilter(shapeSettings);
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);

        mesh = new Mesh();
        verts = new Vector3[(shapeSettings.resolution + 1) * (shapeSettings.resolution + 1)];
        uvs = new Vector2[(shapeSettings.resolution + 1) * (shapeSettings.resolution + 1)];

        triangles = new int[(shapeSettings.resolution) * (shapeSettings.resolution) * 6]; 
        int vert = 0; 
        int triIndex = 0;

        if(planetCore == null)
        {
            if (transform.parent.GetComponent<MeshGenerator>() != null)
            {
                planetCore = transform.parent.GetComponent<MeshGenerator>().planetCore;
            }
            else
            {
                planetCore = transform.parent;
            }
        }

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
                uvs[i] = new Vector2(colorGenerator.biomePercentFromPoint(verts[i]), 0);

                verts[i] = noiseFilter.CalculatePointOnPlanet(verts[i], this.transform.position, out elevaltion);


                if (lod == 0)
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

    void Update()
    {
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("_planetCenter", planetCore.position);
    }

    public void UpdateMesh()
    {
        CalculateCenter(verts);
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
        GetComponent<MeshFilter>().sharedMesh = mesh;
        SetMaterial(mat);
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

        transform.position = verts[(verts.Length / 2)]; // To put the pivot at the exact middle vertice. For further control over LOD.
       
       for (int i = 0; i < verts.Length; i++)
       {
            verts[i] = verts[i] - this.transform.position;
       }
       
       Vector3 pose = planetCore.position + (planetCore.rotation * transform.position);
       transform.position = pose;
    }

    public void SetMaterial(Material mat)
    {
        if(mat != this.mat)
        {
            Material material = new Material(mat.shader);
            UpdateMaterial(material);
        }
        else
        {
            UpdateMaterial(mat);
        }
    }
    
    void UpdateMaterial(Material material)
    {
        material.SetVector("_elivationMinMax", mat.GetVector("_elivationMinMax"));
        material.SetTexture("_pColor", mat.GetTexture("_pColor"));
        material.SetFloat("_smoothness", mat.GetFloat("_smoothness"));
        material.SetFloat("_specular", mat.GetFloat("_specular"));
        material.SetVector("_planetCenter", planetCore.position);
        /* 
         * FUCKING DUCK THE BIGGEST BUG OF LIFE ..... 
         * The shader evaluation of hight was in object space meaing local space but since 
         * the planet is being rotated the face's position is alred but still doing a stupid thing of taking the world positions like is shit face idiot ...
         * original line ||  material.SetVector("_planetCenter", this.transform.position - planetCore.position);    ||
         * changed Line  ||  material.SetVector("_planetCenter", this.transform.localPosition);                     ||
         * 
         * 
         * well GG transform.localposition ate up the god damn LOD shaders so changed the approch of the shader it self !!
        */
        GetComponent<MeshRenderer>().material = material;
    }
}
