
using System.Collections.Generic;

public class CheckUtil {


    /// <summary>
    /// 检测 string是否为null
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool StringIsNull(string str)
    {
        if (str == null || str.Length == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测 list是否为null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool ListIsNull<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否是数字
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static bool CheckIsNumber(string number)
    {
        int temp;
        return int.TryParse(number, out temp);
    }


}