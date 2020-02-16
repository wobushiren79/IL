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

    /// <summary>
    /// 获取枚举第几项
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="position"></param>
    /// <returns></returns>
    public static E GetEnumValueByPosition<E>(int position)
    {
        int i = 0;
        foreach (E item in Enum.GetValues(typeof(E)))
        {
            if (i == position)
            {
                return item;
            }
            i++;
        }
        return default;
    }
}