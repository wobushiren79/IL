using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameControlManager : BaseManager
{
    public Dictionary<GameControlHandler.ControlEnum, BaseControl> dicControl = new Dictionary<GameControlHandler.ControlEnum, BaseControl>();

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
        if (dicControl.TryGetValue(controlEnum, out BaseControl baseControl))
        {
            return baseControl as T;
        }
        else
        {
            GameObject objModel = GetModel<GameObject>("control/control", controlName);
            GameObject obj = Instantiate(gameObject, objModel);
            DestroyImmediate(objModel, true);
            BaseControl controlModel = obj.GetComponent<BaseControl>();
            controlModel.transform.SetParent(transform);
            dicControl.Add(controlEnum, controlModel);
            return controlModel as T;
        }
    }
}