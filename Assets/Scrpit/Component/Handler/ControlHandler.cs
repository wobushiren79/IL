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