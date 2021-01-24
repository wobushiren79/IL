using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InteractiveLookCpt : BaseInteractiveCpt
{
    public long markId;//交互ID

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            GameEventHandler.Instance.EventTriggerForLook(markId);
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        TextInfoHandler.Instance.manager.GetTextById(TextEnum.Look, markId, SetTextInfoData);
    }

  
    /// <summary>
    /// 设置文本数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetTextInfoData(List<TextInfoBean> listData)
    {
        if (!CheckUtil.ListIsNull(listData))
            characterInt.ShowInteractive(listData[0].name);
    }

}