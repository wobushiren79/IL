using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Cinemachine;

public class GameControlHandler : BaseHandler<GameControlHandler,GameControlManager>
{
    public enum ControlEnum
    {
        Normal,//普通
        Build,//建筑模式
        Work,//上班模式
        Story,//故事模式
        MiniGameBarrage,//弹幕小游戏操作
        MiniGameCombat,//战斗小游戏操作
        MiniGameCooking,//烹饪小游戏操作
        MiniGameAccount,//算账小游戏
        MiniGameDebate,//辩论
    }
    public List<BaseControl> listControl = new List<BaseControl>();

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CinemachineVirtualCamera camera2D = Find<CinemachineVirtualCamera>(ImportantTypeEnum.Camera2D);
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
    /// 结束控制
    /// </summary>
    public void EndControl()
    {
        BaseControl control = GetControl();
        if (control != null)
            control.EndControl();
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
    public T StartControl<T>(ControlEnum controlEnum) where T: BaseControl
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
        return baseControl as T;
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
    public T GetControl<T>(ControlEnum controlEnum) where T: BaseControl
    {
        string controlName = EnumUtil.GetEnumName(controlEnum);
        for (int i = 0; i < listControl.Count; i++)
        {
            BaseControl itemControl = listControl[i];
            if (itemControl.name.Equals(controlName))
            {
                return itemControl as T;
            }
        }
        return null;
    }
}