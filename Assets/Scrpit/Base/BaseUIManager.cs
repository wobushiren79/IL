using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseUIManager : BaseManager
{

    //所有的UI控件
    public List<BaseUIComponent> uiList = new List<BaseUIComponent>();

    /// <summary>
    /// 获取打开的UI
    /// </summary>
    /// <returns></returns>
    public BaseUIComponent GetOpenUI()
    {
        for (int i=0;i< uiList.Count;i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI.gameObject.activeSelf)
            {
                return itemUI;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取打开UI的名字
    /// </summary>
    /// <returns></returns>
    public string GetOpenUIName()
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI.gameObject.activeSelf)
            {
                return itemUI.name;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据UI的名字获取UI
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public T GetUIByName<T>(string uiName) where T: BaseUIComponent 
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return null;
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI.name.Equals(uiName))
            {
                return itemUI as T;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取UI
    /// </summary>
    /// <param name="uiEnum"></param>
    /// <returns></returns>
    public T GetUI<T>(UIEnum uiEnum) where T : BaseUIComponent
    {
        return GetUIByName<T>(EnumUtil.GetEnumName(uiEnum));
    }

    /// <summary>
    /// 根据UI的名字获取UI列表
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public List<BaseUIComponent> GetUIListByName(string uiName)
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return null;
        List<BaseUIComponent> tempUIList = new List<BaseUIComponent>();
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI.name.Equals(uiName))
            {
                tempUIList.Add(itemUI);
            }
        }
        return tempUIList;
    }

    /// <summary>
    /// 通过UI的名字开启UI
    /// </summary>
    /// <param name="uiName"></param>
    public T OpenUIByName<T>(string uiName) where T: BaseUIComponent 
    {
        BaseUIComponent uiComponent = null;
        if (CheckUtil.StringIsNull(uiName))
            return uiComponent as T;
        bool hasData = false;
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI.name.Equals(uiName))
            {
                uiComponent = itemUI;
                itemUI.OpenUI();
                hasData = true;
            }
        }
        if (!hasData)
        {
            GameObject uiModel = LoadAssetUtil.SyncLoadAsset<GameObject>("ui/ui", uiName);
            //BaseUIComponent uiModel = LoadResourcesUtil.SyncLoadData<BaseUIComponent>("UI/"+ uiName);
            if (uiModel)
            {
                GameObject objUIComponent = Instantiate(gameObject, uiModel.gameObject);
                objUIComponent.name =objUIComponent.name.Replace("(Clone)", "");
                uiComponent = objUIComponent.GetComponent<BaseUIComponent>();
                uiComponent.OpenUI();
                uiList.Add(uiComponent);
            }
            else
            {
                LogUtil.LogError("没有找到指定UI："+ "ui/ui " + uiName);
            }
        }
        return uiComponent as T;
    }

    /// <summary>
    /// 开启UI
    /// </summary>
    /// <param name="uiEnum"></param>
    public T OpenUI<T>(UIEnum uiEnum) where T: BaseUIComponent
    {
        string uiName = EnumUtil.GetEnumName(uiEnum);
        return  OpenUIByName<T>(uiName);
    }


    /// <summary>
    /// 通过UI的名字关闭UI
    /// </summary>
    /// <param name="uiName"></param>
    public void CloseUIByName(string uiName)
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return;
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI.name.Equals(uiName))
            {
                itemUI.CloseUI();
            }
        }
    }

    /// <summary>
    /// 关闭所有UI
    /// </summary>
    public void CloseAllUI()
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI.gameObject.activeSelf)
                itemUI.CloseUI();
        }
    }

    /// <summary>
    /// 通过UI的名字开启UI并关闭其他UI
    /// </summary>
    /// <param name="uiName"></param>
    public T OpenUIAndCloseOtherByName<T>(string uiName) where T : BaseUIComponent
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return null;
        //首先关闭其他UI
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (!itemUI.name.Equals(uiName))
            {
                if (itemUI.gameObject.activeSelf)
                    itemUI.CloseUI();
            }
        }
        return OpenUIByName<T>(uiName);
    }

    public T OpenUIAndCloseOther<T>(UIEnum ui) where T: BaseUIComponent
    {
       return OpenUIAndCloseOtherByName<T>(EnumUtil.GetEnumName(ui));
    }

    /// <summary>
    /// 通过UI开启UI并关闭其他UI
    /// </summary>
    /// <param name="uiName"></param>
    public void OpenUIAndCloseOtherByName(BaseUIComponent uiComponent)
    {
        if (uiList == null || uiComponent == null)
            return;
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (!itemUI == uiComponent)
            {
                itemUI.CloseUI();
            }
        }
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI == uiComponent)
            {
                itemUI.OpenUI();
            }
        }
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public void RefreshAllUI()
    {
        if (uiList == null)
            return;
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            itemUI.RefreshUI();
        }
    }

    /// <summary>
    /// 根据名字刷新UI
    /// </summary>
    /// <param name="uiName"></param>
    public void RefreshUIByName(string uiName)
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return;
        for (int i = 0; i < uiList.Count; i++)
        {
            BaseUIComponent itemUI = uiList[i];
            if (itemUI.name.Equals(uiName))
            {
                itemUI.RefreshUI();
            }
        }
    }

    /// <summary>
    /// 初始化所有UI
    /// </summary>
    public void InitListUI()
    {
        uiList = new List<BaseUIComponent>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tfChild = transform.GetChild(i);
            BaseUIComponent childUI = tfChild.GetComponent<BaseUIComponent>();
            if (childUI)
            {
                childUI.uiManager = this;
                uiList.Add(childUI);
            }
        }
    }

    public RectTransform GetContainer()
    {
        return (RectTransform)gameObject.transform;
    }
}
