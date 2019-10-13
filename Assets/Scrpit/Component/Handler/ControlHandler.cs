using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Cinemachine;

public class ControlHandler : BaseMonoBehaviour
{
    public enum ControlEnum
    {
        Normal,//普通
        Build,//建筑模式
        Work,//上班模式
        Story,//故事模式
        MiniGameBarrage,//弹幕小游戏操作
        MiniGameCombat,//战斗小游戏操作
    }

    //镜头
    public CinemachineVirtualCamera camera2D;
    public List<BaseControl> listControl = new List<BaseControl>();

    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
        foreach (BaseControl itemControl in listControl)
        {
            itemControl.SetCamera2D(camera2D);
        }
    }

    /// <summary>
    /// 暂停控制
    /// </summary>
    public void StopControl()
    {
        BaseControl control = GetControl();
        if (control != null)
            control.StopControl();
    }

    /// <summary>
    /// 恢复控制
    /// </summary>
    public void RestoreControl()
    {
        BaseControl control = GetControl();
        if (control != null)
            control.RestoreControl();
    }

    /// <summary>
    /// 开始控制
    /// </summary>
    /// <param name="controlEnum"></param>
    public BaseControl StartControl(ControlEnum controlEnum)
    {
        string controlName = EnumUtil.GetEnumName(controlEnum);
        BaseControl baseControl = null;
        for (int i = 0; i < listControl.Count; i++)
        {
            BaseControl itemControl = listControl[i];
            if (itemControl.name.Equals(controlName))
            {
                itemControl.StartControl();
                baseControl = itemControl;
            }
            else
            {
                itemControl.EndControl();
            }
        }
        return baseControl;
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
        string controlName = EnumUtil.GetEnumName(controlEnum);
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
}