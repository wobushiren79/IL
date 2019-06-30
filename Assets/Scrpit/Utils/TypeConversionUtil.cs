using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
public class TypeConversionUtil
{
    /// <summary>
    /// 自定义时间格式转换系统时间格式
    /// </summary>
    /// <param name="timeBean"></param>
    /// <returns></returns>
    public static DateTime TimeBeanToDateTime(TimeBean timeBean)
    {
        DateTime dateTime = new DateTime(timeBean.year, timeBean.month, timeBean.day, timeBean.hour, timeBean.minute, timeBean.second);
        return dateTime;
    }

    /// <summary>
    /// 自定义位置转为系统位置
    /// </summary>
    /// <param name="vector3Bean"></param>
    /// <returns></returns>
    public static Vector3 Vector3BeanToVector3(Vector3Bean vector3Bean)
    {
        Vector3 vector3 = new Vector3(vector3Bean.x, vector3Bean.y, vector3Bean.z);
        return vector3;
    }

    /// <summary>
    /// Vector3 转化为 Vector2
    /// </summary>
    /// <param name="listVector3"></param>
    /// <returns></returns>
    public static List<Vector2> ListV3ToListV2(List<Vector3> listVector3)
    {
        List<Vector2> listVector2 = new List<Vector2>();
        foreach (Vector3 item in listVector3)
        {
            listVector2.Add(new Vector2(item.x, item.y));
        }
        return listVector2;
    }

    /// <summary>
    /// Vector3 转化为 Vector2
    /// </summary>
    /// <param name="listVector3"></param>
    /// <returns></returns>
    public static List<Vector3Bean> ListV3ToListV3Bean(List<Vector3> listVector3)
    {
        List<Vector3Bean> listVector3Bean = new List<Vector3Bean>();
        foreach (Vector3 item in listVector3)
        {
            listVector3Bean.Add(new Vector3Bean(item));
        }
        return listVector3Bean;
    }

    /// <summary>
    /// list转数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T[] ListToArray<T>(List<T> list)
    {
        if (list == null)
            return null;
        return list.ToArray();
    }

    /// <summary>
    /// 数组转List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static List<T> ArrayToList<T>(T[] array)
    {
        if (array == null)
            return null;
        return array.ToList<T>();
    }

    /// <summary>
    /// list转数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T[] ListToArrayFromPosition<T>(List<T> list, int position)
    {
        if (list == null)
            return null;
        int listCount = list.Count;
        T[] tempArray = new T[listCount];
        int f = 0;
        for (int i = 0; i < listCount; i++)
        {
            int startPosition = i + position;
            if (startPosition < listCount)
            {
                tempArray[i] = list[startPosition];
            }
            else
            {
                tempArray[i] = list[f];
                f++;
            }

        }
        return tempArray;
    }

    /// <summary>
    /// list转string 通过split分割
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="split"></param>
    /// <returns></returns>
    public static string ListToStringBySplit<T>(List<T> list, string split)
    {
        string data = "";
        if (data == null)
            return data;
        for (int i = 0; i < list.Count; i++)
        {
            if (i != 0)
            {
                data += split;
            }
            data += list[i].ToString();
        }
        return data;
    }

    /// <summary>
    /// 数组转string 通过split分割
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="split"></param>
    /// <returns></returns>
    public static string ArrayToStringBySplit<T>(T[] list, string split)
    {
        string data = "";
        if (data == null)
            return data;
        for (int i = 0; i < list.Length; i++)
        {
            if (i != 0)
            {
                data += split;
            }
            data += list[i].ToString();
        }
        return data;
    }

    /// <summary>
    /// Color转换ColorBean
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static ColorBean ColorToColorBean(Color color)
    {
        ColorBean colorBean = new ColorBean(color.a, color.g, color.b, color.a);
        return colorBean;
    }


    /// <summary>
    ///  图标字典转List
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static List<IconBean> IconBeanDictionaryToList(IconBeanDictionary map)
    {
        List<IconBean> listData = new List<IconBean>();
        foreach(string key in map.Keys)
        {
            IconBean iconBean = new IconBean
            {
                key = key,
                value = map[key]
            };
            listData.Add(iconBean);
        }
        return listData;
    }
    
}