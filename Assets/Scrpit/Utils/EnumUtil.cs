using UnityEngine;
using UnityEditor;
using System;

public class EnumUtil
{
    public static string GetEnumName<T>(T data)
    {
         return data.ToString();
    }

    public static T GetEnum<T>(string data)
    {
        return (T)Enum.Parse(typeof(T), data);
    }
}