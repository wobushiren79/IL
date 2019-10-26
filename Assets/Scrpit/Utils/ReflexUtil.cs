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

    /// <summary>
    /// 通过反射调用类的方法（SayHello(string name)）
    /// </summary>
    public static string GetInvokeMethod(MonoBehaviour component,string methodName)
    {
        // 1.Load(命名空间名称)，GetType(命名空间.类名)
        Type type = component.GetType();
        // Type type = Assembly.Load(className).GetType(className+"."+className);
        //2.GetMethod(需要调用的方法名称)
        MethodInfo method = type.GetMethod(methodName);
        // 3.调用的实例化方法（非静态方法）需要创建类型的一个实例
        object obj = Activator.CreateInstance(type);
        //4.方法需要传入的参数
        object[] parameters = new object[] { 1 };
        // 5.调用方法，如果调用的是一个静态方法，就不需要第3步（创建类型的实例）
        // 相应地调用静态方法时，Invoke的第一个参数为null
        string result = (string)method.Invoke(obj, parameters);
        return result;
    }

}