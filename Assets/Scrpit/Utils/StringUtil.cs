using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StringUtil
{
    /// <summary>
    /// 计算字符串中指定字符出现次数
    /// </summary>
    /// <param name="data">字符串</param>
    /// <param name="substring">指定</param>
    /// <returns></returns>
    public static int SubstringCount(string data, string substring)
    {
        if (data.Contains(substring))
        {
            string strReplaced = data.Replace(substring, "");
            return (data.Length - strReplaced.Length);
        }
        return 0;
    }

    /// <summary>
    /// string 拆分成指定枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static T[] SplitBySubstringForArrayEnum<T>(string data, char substring)
    {
        if (CheckUtil.StringIsNull(data))
            return new T[0];
        string[] splitData = data.Split(substring);
        if (CheckUtil.ArrayIsNull(splitData))
        {
            return new T[0];
        }
        T[] listData = new T[splitData.Length];
        for (int i = 0; i < splitData.Length; i++)
        {
            if (CheckUtil.StringIsNull(splitData[i]))
            {

            }
            else
            {
                listData[i] = EnumUtil.GetEnum<T>(splitData[i]);
            }
        }
        return listData;
    }

    /// <summary>
    /// string通过指定字符拆分成数组
    /// </summary>
    /// <param name="data"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static List<string> SplitBySubstringForListStr(string data, char substring)
    {
        if (data == null)
            return new List<string>();
        string[] splitData = data.Split(substring);
        List<string> listData = TypeConversionUtil.ArrayToList(splitData);
        return listData;
    }

    /// <summary>
    ///  string通过指定字符拆分成数组
    /// </summary>
    /// <param name="data"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static string[] SplitBySubstringForArrayStr(string data, char substring)
    {
        if (data == null)
            return new string[0];
        string[] splitData = data.Split(substring);
        return splitData;
    }

    /// <summary>
    /// string通过指定字符拆分成数组
    /// </summary>
    /// <param name="data"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static long[] SplitBySubstringForArrayLong(string data, char substring)
    {
        if (CheckUtil.StringIsNull(data))
            return new long[0];
        string[] splitData = data.Split(substring);
        long[] listData = TypeConversionUtil.ArrayStrToArrayLong(splitData);
        return listData;
    }

    /// <summary>
    /// string通过指定字符拆分成数组
    /// </summary>
    /// <param name="data"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static int[] SplitBySubstringForArrayInt(string data, char substring)
    {
        if (data == null)
            return new int[0];
        string[] splitData = data.Split(substring);
        int[] listData = TypeConversionUtil.ArrayStrToArrayInt(splitData);
        return listData;
    }

    /// <summary>
    /// string通过指定字符拆分成数组
    /// </summary>
    /// <param name="data"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static float[] SplitBySubstringForArrayFloat(string data, char substring)
    {
        if (data == null)
            return new float[0];
        string[] splitData = data.Split(substring);
        float[] listData = TypeConversionUtil.ArrayStrToArrayFloat(splitData);
        return listData;
    }
}