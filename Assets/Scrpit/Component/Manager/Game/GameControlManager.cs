using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameControlManager : BaseManager
{
    public Dictionary<string, BaseControl> dicControl = new Dictionary<string, BaseControl>();

    /// <summary>
    /// 获取当前控制
    /// </summary>
    /// <returns></returns>
    public BaseControl GetControl()
    {
        foreach (var itemDic in dicControl)
        {
            BaseControl itemControl = itemDic.Value;
            if (itemControl.gameObject.activeSelf)
            {
                return itemControl;
            }
        }
        return null;
    }

    /// <summary>
    /// 通过类型获取控制
    /// </summary>
    /// <param name="controlEnum"></param>
    /// <returns></returns>
    public T GetControl<T>(GameControlHandler.ControlEnum controlEnum) where T : BaseControl
    {
        string controlName = EnumUtil.GetEnumName(controlEnum);
        if (dicControl.TryGetValue(controlName, out BaseControl baseControl))
        {
            return baseControl as T;
        }
        else
        {
            BaseControl baseControl1 = GetModel(dicControl, "", controlName);
            baseControl1.transform.SetParent(transform);
            return baseControl as T;
        }
    }
}