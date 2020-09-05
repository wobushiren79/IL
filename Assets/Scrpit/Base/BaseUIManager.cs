using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseUIManager : BaseMonoBehaviour
{
    //所有的UI控件
    public List<BaseUIComponent> uiList;

    /// <summary>
    /// 获取打开的UI
    /// </summary>
    /// <returns></returns>
    public BaseUIComponent GetOpenUI()
    {
        foreach (BaseUIComponent itemUI in uiList)
        {
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
        foreach (BaseUIComponent itemUI in uiList)
        {
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
    public BaseUIComponent GetUIByName(string uiName)
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return null;
        foreach (BaseUIComponent itemUI in uiList)
        {
            if (itemUI.name.Equals(uiName))
            {
                return itemUI;
            }
        }
        return null;
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
        foreach (BaseUIComponent itemUI in uiList)
        {
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
    public void OpenUIByName(string uiName)
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return;
        foreach (BaseUIComponent itemUI in uiList)
        {
            if (itemUI.name.Equals(uiName))
            {
                itemUI.OpenUI();
            }
        }
    }

    /// <summary>
    /// 通过UI的名字关闭UI
    /// </summary>
    /// <param name="uiName"></param>
    public void CloseUIByName(string uiName)
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return;
        foreach (BaseUIComponent itemUI in uiList)
        {
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
        foreach (BaseUIComponent itemUI in uiList)
        {
            if (itemUI.gameObject.activeSelf)
                itemUI.CloseUI();
        }
    }

    /// <summary>
    /// 通过UI的名字开启UI并关闭其他UI
    /// </summary>
    /// <param name="uiName"></param>
    public BaseUIComponent OpenUIAndCloseOtherByName(string uiName)
    {
        if (uiList == null || CheckUtil.StringIsNull(uiName))
            return null;
        BaseUIComponent uiComponent = null;
        foreach (BaseUIComponent itemUI in uiList)
        {
            if (!itemUI.name.Equals(uiName))
            {
                if (itemUI.gameObject.activeSelf)
                    itemUI.CloseUI();
            }
        }
        foreach (BaseUIComponent itemUI in uiList)
        {
            if (itemUI.name.Equals(uiName))
            {
                itemUI.OpenUI();
                uiComponent = itemUI;
            }
        }
        return uiComponent;
    }

    public BaseUIComponent OpenUIAndCloseOther(UIEnum ui)
    {
       return OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(ui));
    }

    /// <summary>
    /// 通过UI开启UI并关闭其他UI
    /// </summary>
    /// <param name="uiName"></param>
    public void OpenUIAndCloseOtherByName(BaseUIComponent uiComponent)
    {
        if (uiList == null || uiComponent == null)
            return;
        foreach (BaseUIComponent itemUI in uiList)
        {
            if (!itemUI == uiComponent)
            {
                itemUI.CloseUI();
            }
        }
        foreach (BaseUIComponent itemUI in uiList)
        {
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
        foreach (BaseUIComponent itemUI in uiList)
        {
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
        foreach (BaseUIComponent itemUI in uiList)
        {
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

}
