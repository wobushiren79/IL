using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class ToastManager : BaseManager
{
    public GameObject objContainer;
    public Dictionary<string, GameObject> listObjModel = new Dictionary<string, GameObject>();
    protected string resUrl = "UI/Toast/";

    /// <summary>
    /// Toast提示
    /// </summary>
    /// <param name="hintContent"></param>
    public void ToastHint(string hintContent)
    {
        CreateToast<ToastView>(ToastEnum.Normal, null, hintContent, 5);
    }

    public void ToastHint(string hintContent, float destoryTime)
    {
        CreateToast<ToastView>(ToastEnum.Normal, null, hintContent, destoryTime);
    }

    public void ToastHint(Sprite toastIconSp, string hintContent)
    {
        CreateToast<ToastView>(ToastEnum.Normal, toastIconSp, hintContent, 5);
    }

    public void ToastHint(Sprite toastIconSp, string hintContent,float destoryTime)
    {
        CreateToast<ToastView>(ToastEnum.Normal, toastIconSp, hintContent, destoryTime);
    }

    /// <summary>
    /// 创建toast
    /// </summary>
    /// <param name="toastType"></param>
    /// <param name="toastIconSp"></param>
    /// <param name="toastContentStr"></param>
    /// <param name="destoryTime"></param>
    public void CreateToast<T>(ToastEnum toastType, Sprite toastIconSp, string toastContentStr, float destoryTime) where T : ToastView
    {
        string toastName = EnumUtil.GetEnumName(toastType);
        GameObject objToast = CreateToast(toastName);
        if (objToast)
        {
            ToastView toastView = objToast.GetComponent<ToastView>();
            toastView.SetData(toastIconSp, toastContentStr, destoryTime);
        }
        else
        {
            LogUtil.LogError("没有找到指定Msg：" + "Resources/" + resUrl + toastName);
        }
    }


    public GameObject CreateToast(string name)
    {
        GameObject objModel = null;
        if (listObjModel.TryGetValue(name, out objModel))
        {

        }
        else
        {
            objModel = CreatToastModel(name);
        }
        if (objModel == null)
            return null;
        GameObject obj = Instantiate(objContainer, objModel);
        return obj;
    }

    private GameObject CreatToastModel(string name)
    {
        GameObject objModel = Resources.Load<GameObject>(resUrl + name);
        objModel.name = name;
        listObjModel.Add(name, objModel);
        return objModel;
    }

    public RectTransform GetContainer()
    {
        return (RectTransform)objContainer.transform;
    }
}