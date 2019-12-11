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
        CreateToast(0, null, hintContent, 3);
    }

    public void ToastHint(string hintContent, float destoryTime)
    {
        CreateToast(0, null, hintContent, destoryTime);
    }

    public void ToastHint(Sprite toastIconSp, string hintContent)
    {
        CreateToast(0, toastIconSp, hintContent, 3);
    }

    /// <summary>
    /// 创建toast
    /// </summary>
    /// <param name="toastIconSp"></param>
    /// <param name="toastTitleStr"></param>
    /// <param name="toastContentStr"></param>
    public void CreateToast(int position, Sprite toastIconSp, string toastContentStr, float destoryTime)
    {
        if (objToastContainer == null || listObjToastModel == null)
            return;
        GameObject objToast = Instantiate(objToastContainer, listObjToastModel[position]);
        objToast.transform.localScale = new Vector3(1, 1, 1);
        objToast.transform.DOScale(new Vector3(0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);

        ToastView toastView = objToast.GetComponent<ToastView>();
        toastView.SetData(toastIconSp, toastContentStr, destoryTime);
    }
}