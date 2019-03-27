using UnityEngine;

public class JsonUtil : ScriptableObject
{
    /// <summary>
    /// Json转换成类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strData"></param>
    /// <returns></returns>
    public static T FromJson<T>(string strData)
    {
        T dataBean = JsonUtility.FromJson<T>(strData);
        return dataBean;
    }

    /// <summary>
    /// 类转换成Json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataBean"></param>
    /// <returns></returns>
    public static string ToJson<T>(T dataBean)
    {
        string json = JsonUtility.ToJson(dataBean);
        return json;
    }

}