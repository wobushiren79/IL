using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class ReflexUtil : ScriptableObject
{
    /// <summary>
    /// 根据反射获取所有属性名称
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<string> GetAllName<T>()
    {
        List<string> listName = new List<string>();
        Type type =typeof(T);
        FieldInfo[] fieldInfos = type.GetFields();
        if (fieldInfos == null)
            return listName;

        int propertyInfoSize = fieldInfos.Length;
        for (int i = 0; i < propertyInfoSize; i++)
        {
            FieldInfo fieldInfo = fieldInfos[i];
            listName.Add(fieldInfo.Name);
        };
        return listName;
    }

    /// <summary>
    /// 根据反射获取所有属性名称及值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="classType"></param>
    /// <returns></returns>
    public static Dictionary<String, object> GetAllNameAndValue<T>(T classType)
    {
        Dictionary<String, object> listData = new Dictionary<string, object>();
        Type type = typeof(T);
        FieldInfo[] fieldInfos = type.GetFields();

        if (fieldInfos == null)
            return listData;

        int propertyInfoSize = fieldInfos.Length;
        for (int i = 0; i < propertyInfoSize; i++)
        {
            FieldInfo fieldInfo = fieldInfos[i];
            listData.Add(fieldInfo.Name, fieldInfo.GetValue(classType));
        };
        return listData;
    }

    /// <summary>
    /// 根据反射 设置值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="classType"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void SetValueByName<T>(T classType, string name,object value)
    {
        Type type = classType.GetType();
        FieldInfo fieldInfo = type.GetField(name);
        if (fieldInfo == null)
            return;
        fieldInfo.SetValue(classType, value);  
    }
}