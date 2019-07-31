using UnityEngine;
using UnityEditor;

public class EnumUtil
{
    public static string GetEnumName<T>(T data)
    {
         return data.ToString();
    }

}