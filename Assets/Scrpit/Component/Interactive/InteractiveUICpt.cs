using UnityEngine;
using UnityEditor;

public class InteractiveUICpt : BaseInteractiveCpt
{
    public string interactiveContent;
    public UIEnum uiType;

    //备注信息
    public string remarkData;

    protected BaseUIManager uiManager;

    private void Start()
    {
        uiManager = Find<BaseUIManager>(ImportantTypeEnum.GameUI);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            if (!CheckUtil.StringIsNull(remarkData))
            {
                BaseUIComponent baseUIComponent = uiManager.GetUIByName(EnumUtil.GetEnumName(uiType));
                baseUIComponent.SetRemarkData(remarkData);
            }
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(uiType));
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        characterInt.ShowInteractive(interactiveContent);
    }
}