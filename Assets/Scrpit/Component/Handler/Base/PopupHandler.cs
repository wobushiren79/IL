using UnityEditor;
using UnityEngine;

public class PopupHandler : BaseUIHandler<PopupHandler, PopupManger>
{
    protected override void Awake()
    {
        sortingOrder = 4;
        base.Awake();
        ChangeUIRenderMode(RenderMode.ScreenSpaceCamera);
    }

    public T CreatePopup<T>(PopupEnum popup) where T: PopupShowView
    {
        string popupName = EnumUtil.GetEnumName(popup);
        return  CreatePopup<T>(popupName);
    }

    public T CreatePopup<T>(string popupName) where T : PopupShowView
    {
        GameObject objModel = manager.GetPopupModel(popupName);
        if (objModel == null)
        {
            LogUtil.LogError("没有找到指定popup：" + popupName);
            return default(T);
        }
        GameObject objPopup = Instantiate(gameObject, objModel);
        return objPopup.GetComponent<T>();
    }
}