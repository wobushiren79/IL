using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class ToastManager : BaseManager
{
    //Toast容器
    public GameObject objToastContainer;
    //Toast模型
    public Dictionary<string, GameObject> listObjModel = new Dictionary<string, GameObject>();

    /// <summary>
    /// 加载toast容器
    /// </summary>
    public void LoadToastListContainer()
    {
        //加载Toast容器
        GameObject toastListModel = LoadResourcesUtil.SyncLoadData<GameObject>("UI/ToastList");
        GameObject objToastList = Instantiate(gameObject, toastListModel);
        objToastContainer = CptUtil.GetCptInChildrenByName<Transform>(objToastList, "Container").gameObject;
    }

    /// <summary>
    /// 获取toast模型
    /// </summary>
    /// <returns></returns>
    public GameObject GetToastModel(string toastName)
    {
        return  GetGameObjectModel(listObjModel,"ui/toast", toastName);
    }

    public RectTransform GetContainer()
    {
        return (RectTransform)objToastContainer.transform;
    }
}