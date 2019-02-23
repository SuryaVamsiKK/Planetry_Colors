using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Debug : MonoBehaviour
{
    public typeOfShape Shape;
    public float size;
    public Color color;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        switch (Shape)
        {
            case typeOfShape.Cube:
                Gizmos.DrawCube(transform.position,new Vector3(size, size, size));
                break;
            case typeOfShape.Sphere:
                Gizmos.DrawSphere(transform.position,size);
                break;
            case typeOfShape.Wire_Cube:
                Gizmos.DrawWireCube(transform.position,new Vector3(size, size, size));
                break;
            case typeOfShape.Wire_Sphere:
                Gizmos.DrawWireSphere(transform.position,size);
                break;
            default:
                break;
        }
    }
}

public enum typeOfShape
{
    Sphere, Cube, Wire_Cube, Wire_Sphere
}
