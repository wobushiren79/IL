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

    protected AudioHandler audioHandler;
    public void Awake()
    {
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }

    /// <summary>
    /// Toast提示
    /// </summary>
    /// <param name="hintContent"></param>
    public void ToastHint(string hintContent)
    {
        CreateToast(ToastEnum.Normal, null, hintContent, 5);
    }

    public void ToastHint(string hintContent, float destoryTime)
    {
        CreateToast(ToastEnum.Normal, null, hintContent, destoryTime);
    }

    public void ToastHint(Sprite toastIconSp, string hintContent)
    {
        CreateToast(ToastEnum.Normal, toastIconSp, hintContent, 5);
    }

    public void ToastHint(Sprite toastIconSp, string hintContent,float destoryTime)
    {
        CreateToast(ToastEnum.Normal, toastIconSp, hintContent, destoryTime);
    }

    /// <summary>
    /// 创建toast
    /// </summary>
    /// <param name="toastType"></param>
    /// <param name="toastIconSp"></param>
    /// <param name="toastContentStr"></param>
    /// <param name="destoryTime"></param>
    public void CreateToast(ToastEnum toastType, Sprite toastIconSp, string toastContentStr, float destoryTime)
    {
        //audioHandler.PlaySound(AudioSoundEnum.ButtonForShow);
        if (objToastContainer == null || listObjToastModel == null)
            return;

        GameObject objCreateToastModel = null;
        foreach (GameObject itemDialog in listObjToastModel)
        {
            if (itemDialog.name.Equals(EnumUtil.GetEnumName(toastType)))
            {
                objCreateToastModel = itemDialog;
                break;
            }
        }
        if (objCreateToastModel == null)
            return;

        GameObject objToast = Instantiate(objToastContainer, objCreateToastModel);
        objToast.transform.localScale = new Vector3(1, 1, 1);
        objToast.transform.DOScale(new Vector3(0.2f, 0.2f), 0.3f).From().SetEase(Ease.OutBack);

        ToastView toastView = objToast.GetComponent<ToastView>();
        toastView.SetData(toastIconSp, toastContentStr, destoryTime);
    }
}