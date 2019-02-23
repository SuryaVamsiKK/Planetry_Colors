    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    public Transform T1;
    public Transform T2;
    public float distance;

    public bool calculate;

    void OnValidate()
    {
        if(T1 != null && T2 != null)
        {
            distance = Vector3.Distance(T1.position, T2.position);
        }
    }
}
