using UnityEditor;
using UnityEngine;

public class ToastHandler : BaseUIHandler<ToastHandler,ToastManager>
{

    protected override void Awake()
    {
        sortingOrder = 5;
        base.Awake();
        manager.LoadToastListContainer();
    }

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

    public void ToastHint(Sprite toastIconSp, string hintContent, float destoryTime)
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
        GameObject objToastModel= manager.GetToastModel(toastName);
        if (objToastModel == null)
        {
            LogUtil.LogError("没有找到指定Toast："+ toastName);
            return;
        }
        GameObject objToast = Instantiate(manager.objToastContainer, objToastModel);
        if (objToast)
        {
            ToastView toastView = objToast.GetComponent<ToastView>();
            toastView.SetData(toastIconSp, toastContentStr, destoryTime);
        }
        else
        {
            LogUtil.LogError("实例化Toast失败" + toastName);
        }
    }

}