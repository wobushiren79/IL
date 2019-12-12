using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class ToastManager : BaseMonoBehaviour
{
    //数据列表
    public GameObject objToastContainer;
    //Item模板
    public List<GameObject> listObjToastModel = new List<GameObject>();

    /// <summary>
    /// Toast提示
    /// </summary>
    /// <param name="hintContent"></param>
    public void ToastHint(string hintContent)
    {
        CreateToast(ToastEnum.Normal, null, hintContent, 3);
    }

    public void ToastHint(string hintContent, float destoryTime)
    {
        CreateToast(ToastEnum.Normal, null, hintContent, destoryTime);
    }

    public void ToastHint(Sprite toastIconSp, string hintContent)
    {
        CreateToast(ToastEnum.Normal, toastIconSp, hintContent, 3);
    }

    /// <summary>
    /// 创建toast
    /// </summary>
    /// <param name="toastIconSp"></param>
    /// <param name="toastTitleStr"></param>
    /// <param name="toastContentStr"></param>
    public void CreateToast(ToastEnum dialogType, Sprite toastIconSp, string toastContentStr, float destoryTime)
    {
        if (objToastContainer == null || listObjToastModel == null)
            return;
        GameObject objToast = Instantiate(objToastContainer, listObjToastModel[(int)dialogType]);
        objToast.transform.localScale = new Vector3(1, 1, 1);
        objToast.transform.DOScale(new Vector3(0.2f, 0.2f), 0.3f).From().SetEase(Ease.OutBack);

        ToastView toastView = objToast.GetComponent<ToastView>();
        toastView.SetData(toastIconSp, toastContentStr, destoryTime);
    }
}