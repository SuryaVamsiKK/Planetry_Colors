using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreePlanetry : MonoBehaviour
{
    [HideInInspector] public float threshold = 2f;
    public bool divide = false;
    [HideInInspector] public bool recreate = false;
    [HideInInspector] public bool revertToHigherLOD = false;
    [HideInInspector] public int maxDepth = 0;

    bool divided = false;
    

    void Start()
    {
        if(transform.parent.GetComponent<QuadTreePlanetry>() != null)
        {
            maxDepth = transform.parent.GetComponent<QuadTreePlanetry>().maxDepth - 1;
            threshold = transform.parent.GetComponent<QuadTreePlanetry>().threshold / 2;
        }
        else
        {
            maxDepth = transform.parent.GetComponent<PlanetGenerator>().lodSettings.maxDepth;
            threshold = transform.parent.GetComponent<PlanetGenerator>().shapeSettings.radius + transform.parent.GetComponent<PlanetGenerator>().lodSettings.playerdetectionRadius;
        }
    }

    void Update()
    {
        

        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < threshold && maxDepth > 0)
        {
            divide = true;
            if(!divided)
            {
                quadTree();
                divided = true;
            }
        }
        else
        {
            divided = false;
            if(transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                    GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    void quadTree()
    {
        if(GetComponent<MeshGenerator>().lod <= 0)
        {
            GetComponent<MeshGenerator>().mat = transform.parent.GetComponent<PlanetGenerator>().mat;
        }

        if(divide)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            for (int i = 0; i < 4; i++)
            {             
                GameObject g = GameObject.Instantiate(GetComponent<MeshGenerator>().chunk, transform);
                g.GetComponent<MeshGenerator>().lod = GetComponent<MeshGenerator>().lod + 1;
                g.name = "LOD : " + g.GetComponent<MeshGenerator>().lod;
                g.GetComponent<MeshGenerator>().mat = GetComponent<MeshGenerator>().mat;
                
                MeshGenerator g_MeshGenerator =  g.GetComponent<MeshGenerator>();
                g_MeshGenerator.quad_Location = i;
                g_MeshGenerator.chunk = GetComponent<MeshGenerator>().chunk;
                g_MeshGenerator.localUp = GetComponent<MeshGenerator>().localUp;
                g_MeshGenerator.shapeSettings = GetComponent<MeshGenerator>().shapeSettings;
                g_MeshGenerator.CreateShape();
                g_MeshGenerator.UpdateMesh(); 
            }
            divide = false;
        }
        //this only for live editing or else entire ELSE can be removed
        else
        {
            if(transform.childCount > 0)
            {
                for (int i = 0; i < 4; i++)
                {             
                    GameObject g = this.transform.GetChild(i).gameObject;
                    g.GetComponent<MeshGenerator>().lod = GetComponent<MeshGenerator>().lod + 1;
                    g.name = "LOD : " + g.GetComponent<MeshGenerator>().lod;
                    g.GetComponent<MeshRenderer> ().sharedMaterial = GetComponent<MeshGenerator>().mat;

                    MeshGenerator g_MeshGenerator =  g.GetComponent<MeshGenerator>();
                    g_MeshGenerator.quad_Location = i;
                    g_MeshGenerator.chunk = GetComponent<MeshGenerator>().chunk;
                    g_MeshGenerator.localUp = GetComponent<MeshGenerator>().localUp;
                    g_MeshGenerator.shapeSettings = GetComponent<MeshGenerator>().shapeSettings;
                    g_MeshGenerator.CreateShape();
                    g_MeshGenerator.UpdateMesh();
                }
            }
        }
    
        if(revertToHigherLOD)
        {
            #region Revert

            GetComponent<MeshRenderer>().enabled = true;
            revertToHigherLOD = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                Debug.Log(i);
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            #endregion
        }
    }
}
