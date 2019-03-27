using UnityEngine;
using UnityEditor;

public class CptUtil
{

    /// <summary>
    /// 删除所有子物体
    /// </summary>
    /// <param name="tf"></param>
    public static void RemoveChild(Transform tf)
    {
        for (int i = 0; i < tf.childCount; i++)
        {
          GameObject.Destroy(tf.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 删除所有显示的子物体
    /// </summary>
    /// <param name="tf"></param>
    public static void RemoveChildsByActive(Transform tf)
    {
        for (int i = 0; i < tf.childCount; i++)
        {
            if (tf.GetChild(i).gameObject.activeSelf)
            {
                GameObject.Destroy(tf.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 通过名字获取子列表的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetCptInChildrenByName<T>(GameObject obj,string name) where T : Component
    {
       T[] cptList= obj.GetComponentsInChildren<T>();
        foreach (T item in cptList)
        {
            if (item.name.Equals(name))
            {
                return item;
            }
        }
        return null;
    }
}