using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class ColorBean 
{
    public float r;
    public float g;
    public float b;
    public float a;

    public Color GetColor()
    {
        return new Color(r,g,b,a);
    }
}