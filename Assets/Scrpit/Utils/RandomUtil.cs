using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RandomUtil
{

    /// <summary>
    /// 获取List 中 随机一个数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRandomDataByList<T>(List<T> list)
    {
        if (CheckUtil.ListIsNull(list))
            return default(T);
       int position = Random.Range(0, list.Count);
       return list[position];
    }

    /// <summary>
    /// 获取List随机数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<T> GetRandomListByList<T>(List<T> list)
    {
        if (list == null)
            return list;
        int counter = list.Count;
        T temp;
        int index;
        while (counter>0)
        {
            counter--;
            index = Random.Range(0, counter);
            temp = list[counter];
            list[counter] = list[index];
            list[index] = temp;
        }
        return list;
    }

    /// <summary>
    /// 获取数组随机数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T[] GetRandomArrayByArray<T>(T[] list)
    {
        if (list == null)
            return list;
        int counter = list.Length;
        T temp;
        int index;
        while (counter > 0)
        {
            counter--;
            index = Random.Range(0, counter);
            temp = list[counter];
            list[counter] = list[index];
            list[index] = temp;
        }
        return list;
    }
}