using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class Vector3Bean 
{
    public float x;
    public float y;
    public float z;

    public Vector3Bean(float x,float y)
    {
        this.x = x;
        this.y = y;
        this.z = 0;
    }
}