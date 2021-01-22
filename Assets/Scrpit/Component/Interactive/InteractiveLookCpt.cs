using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InteractiveLookCpt : BaseInteractiveCpt
{
    public long markId;//交互ID

    protected EventHandler eventHandler;

    private void Awake()
    {
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E) && eventHandler != null)
        {
            eventHandler.EventTriggerForLook(markId);
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