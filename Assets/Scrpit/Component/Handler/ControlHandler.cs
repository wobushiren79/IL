using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ControlHandler : BaseManager
{
    public enum ControlEnum
    {
        Normal,//普通
        Build,//建筑模式
        Work,//上班模式
    }

    public List<BaseControl> listControl = new List<BaseControl>();

    /// <summary>
    /// 暂停控制
    /// </summary>
    public void StopControl()
    {
        BaseControl control= GetControl();
        control.enabled = false;
    }

    /// <summary>
    /// 恢复控制
    /// </summary>
    public void RestoreControl()
    {
        BaseControl control = GetControl();
        control.enabled = true;
    }

    /// <summary>
    /// 开始控制
    /// </summary>
    /// <param name="controlEnum"></param>
    public void StartControl(ControlEnum controlEnum)
    {
        string controlName = GetControlName(controlEnum);
        for (int i = 0; i < listControl.Count; i++)
        {
            BaseControl itemControl = listControl[i];
            if (itemControl.name.Equals(controlName))
            {
                itemControl.StartControl();
            }
            else
            {
                itemControl.EndControl();
            }
        }
    }

    /// <summary>
    /// 获取当前控制
    /// </summary>
    /// <returns></returns>
    public BaseControl GetControl()
    {
        for (int i = 0; i < listControl.Count; i++)
        {
            BaseControl itemControl = listControl[i];
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
    public BaseControl GetControl(ControlEnum controlEnum)
    {
        string controlName = GetControlName(controlEnum);
        for (int i = 0; i < listControl.Count; i++)
        {
            BaseControl itemControl = listControl[i];
            if (itemControl.name.Equals(controlName))
            {
                return itemControl;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取控制名称
    /// </summary>
    /// <param name="controlEnum"></param>
    /// <returns></returns>
    private string GetControlName(ControlEnum controlEnum)
    {
        string controlName = "";
        switch (controlEnum)
        {
            case ControlEnum.Normal:
                controlName = "NormalControl";
                break;
            case ControlEnum.Build:
                controlName = "BuildControl";
                break;
            case ControlEnum.Work:
                controlName = "WorkControl";
                break;
        }
        return controlName;
    }
}