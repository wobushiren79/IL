using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToastView : MonoBehaviour
{
    //数据列表
    public GameObject listContent;
    //Item模板
    public GameObject itemToastModel;

    /// <summary>
    /// Toast提示
    /// </summary>
    /// <param name="hintContent"></param>
    public void ToastHint(string hintContent)
    {
        CreateToast(null, hintContent, 3);
    }

    public void ToastHint(string hintContent, float destoryTime)
    {
        CreateToast(null, hintContent, destoryTime);
    }

    /// <summary>
    /// 创建toast
    /// </summary>
    /// <param name="toastIconSp"></param>
    /// <param name="toastTitleStr"></param>
    /// <param name="toastContentStr"></param>
    public void CreateToast(Sprite toastIconSp, string toastContentStr, float destoryTime)
    {
        if (listContent == null || itemToastModel == null)
            return;
        GameObject toastHintObj = Instantiate(itemToastModel, itemToastModel.transform);
        toastHintObj.SetActive(true);
        toastHintObj.transform.localScale = new Vector3(0, 0, 1);
        toastHintObj.transform.DOScale(new Vector3(1, 1), 0.5f).SetEase(Ease.OutBack);
        toastHintObj.transform.parent = listContent.transform;

        //设置Icon
        Image toastIcon = CptUtil.GetCptInChildrenByName<Image>(toastHintObj, "ToastIcon");
        if (toastIcon != null && toastIconSp != null)
            toastIcon.sprite = toastIconSp;
        //设置内容
        Text toastContent = CptUtil.GetCptInChildrenByName<Text>(toastHintObj, "ToastContent");
        if (toastContent != null && toastContentStr != null)
            toastContent.text = toastContentStr;
        //定时销毁
        toastHintObj.transform.DOScaleZ(1, destoryTime).OnComplete(delegate () {
            DestroyToast(toastHintObj);
        });
    }

    private void DestroyToast(GameObject toastObj)
    {
        toastObj.transform.DOScale(new Vector3(0, 0), 0.4f).OnComplete(delegate () {
            Destroy(toastObj);
        });
    }
}
