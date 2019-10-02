using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonoBehaviour : MonoBehaviour {

    /// <summary>
    /// 实例化一个物体
    /// </summary>
    /// <param name="objContent"></param>
    /// <param name="objModel"></param>
    /// <returns></returns>
    public GameObject Instantiate(GameObject objContent,GameObject objModel)
    {
        GameObject objItem = Instantiate(objModel, objContent.transform);
        objItem.SetActive(true);
        return objItem;
    }

    /// <summary>
    /// 实例化一个物体
    /// </summary>
    /// <param name="objContent"></param>
    /// <param name="objModel"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject Instantiate(GameObject objContent, GameObject objModel,Vector3 position)
    {
        GameObject objItem = Instantiate(objModel, objContent.transform);
        objItem.SetActive(true);
        objItem.transform.position = position;
        return objItem;
    }
}
