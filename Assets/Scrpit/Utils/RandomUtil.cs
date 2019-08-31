using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

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
    /// 获取IconBeanDictionary 中 随机一个数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IconBean GetRandomDataByIconBeanDictionary(IconBeanDictionary list)
    {
        int position = Random.Range(0, list.Count);
        int i = 0;
        foreach (string key in list.Keys)
        {
            if (position == i)
            {
                IconBean data = new IconBean
                {
                    key = key,
                    value = list[key]
                };
                return data;
            }
            i++;
        }
        return null;
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

    /// <summary>
    /// 获取数组随机数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRandomDataByArray<T>(T[] list)
    {
        if (list == null)
            return default(T);
        int position = Random.Range(0, list.Length);
        return list[position];
    }

    /// <summary>
    /// 随机产生常用汉字
    /// </summary>
    /// <param name="count">要产生汉字的个数</param>
    /// <returns>常用汉字</returns>
    public static string GetRandomGenerateChineseWord(int count)
    {
        if (count < 1)
            return "";

        string chineseWords = "";
        Encoding gb = Encoding.GetEncoding("gb2312");
        //添加姓
        chineseWords += GetRandomDataByArray(GeneralDataUtil.ChinesNameWords);
        //添加名
        for (int i = 0; i < count - 1; i++)
        {
            // 获取区码(常用汉字的区码范围为16-55)
            int regionCode = Random.Range(16, 56);

            // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)
            int positionCode;
            if (regionCode == 55)
            {
                // 55区排除90,91,92,93,94
                positionCode = Random.Range(1, 90);
            }
            else
            {
                positionCode = Random.Range(1, 95);
            }

            // 转换区位码为机内码
            int regionCode_Machine = regionCode + 160;// 160即为十六进制的20H+80H=A0H
            int positionCode_Machine = positionCode + 160;// 160即为十六进制的20H+80H=A0H

            // 转换为汉字
            byte[] bytes = new byte[] { (byte)regionCode_Machine, (byte)positionCode_Machine };
            chineseWords += gb.GetString(bytes);
        }
        return chineseWords;
    }
}