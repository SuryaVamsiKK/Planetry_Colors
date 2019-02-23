using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianFace : MonoBehaviour
{
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    float distanceBwVerts;
    Vector2 planePos;
    Transform correction;

    public TerrianFace(Mesh mesh, int resolution, Vector3 localUp, float distanceBwVerts, Vector2 planePos, Transform correction)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.distanceBwVerts = distanceBwVerts;
        this.planePos = planePos;
        this.correction = correction;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int [] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x+y * resolution;
                Vector2 percent = (new Vector2(x + planePos.x, y + planePos.y) / (resolution - 1) * distanceBwVerts); // ------- Any adjustments on the verts its here ot be done !!
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;

                if(x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex+1] = i + resolution + 1;
                    triangles[triIndex+2] = i + resolution;

                    triangles[triIndex+3] = i;
                    triangles[triIndex+4] = i + 1;
                    triangles[triIndex+5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }

        correction.gameObject.transform.localPosition = CalculateCenter(vertices);

        for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v] = vertices[v] - correction.gameObject.transform.localPosition;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    Vector3 CalculateCenter(Vector3[] verts)
   {
       Vector3 sum = Vector3.zero;
       Vector3 avg = Vector3.zero;
       for (int i = 0; i < verts.Length; i++)
       {
           sum += new Vector3(verts[i].x, verts[i].y, verts[i].z);           
       }
       avg = sum/verts.Length;
       return avg;
   }
}
