using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Cinemachine;

public class GameControlHandler : BaseHandler<GameControlHandler, GameControlManager>
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

    /// <summary>
    /// 暂停控制
    /// </summary>
    public void StopControl()
    {
        BaseControl control = manager.GetControl();
        if (control != null)
            control.StopControl();
    }

    /// <summary>
    /// 关闭所有控制
    /// </summary>
    public void EndAllControl()
    {
        Dictionary<ControlEnum, BaseControl> dicControl = manager.dicControl;
        foreach (var itemDic in dicControl)
        {
            itemDic.Value.EndControl();
        }
    }

    /// <summary>
    /// 结束控制
    /// </summary>
    public void EndControl()
    {
        BaseControl control = manager.GetControl();
        if (control != null)
            control.EndControl();
    }

    /// <summary>
    /// 恢复控制
    /// </summary>
    public void RestoreControl()
    {
        BaseControl control = manager.GetControl();
        if (control != null)
            control.RestoreControl();
    }

    /// <summary>
    /// 开始控制
    /// </summary>
    /// <param name="controlEnum"></param>
    public T StartControl<T>(ControlEnum controlEnum) where T : BaseControl
    {
        Dictionary<ControlEnum, BaseControl> dicControl = manager.dicControl;
        BaseControl baseControl = null;
        foreach (var itemDic in dicControl)
        {
            if (itemDic.Key == controlEnum)
            {
                itemDic.Value.StartControl();
                baseControl = itemDic.Value;
            }
            else
            {
                itemDic.Value.EndControl();
            }
        }
        if (baseControl == null)
        {
            baseControl = manager.GetControl<T>(controlEnum);
            baseControl.StartControl();
        }
        return baseControl as T;
    }
}