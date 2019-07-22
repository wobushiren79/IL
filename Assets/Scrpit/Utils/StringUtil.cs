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
    /// string通过指定字符拆分成数组
    /// </summary>
    /// <param name="data"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static List<string> SplitBySubstring(string data, char substring)
    {
        string[] splitData = data.Split(substring);
        List<string> listData = TypeConversionUtil.ArrayToList(splitData);
        return listData;
    }
}