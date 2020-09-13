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
        BaseUIComponent baseUIComponent = uiManager.GetUIByName(EnumUtil.GetEnumName(uiType));
        BaseUIComponent currentUIComponent = uiManager.GetOpenUI();
        //如果当前页面不是即将要打开的页面 并且当前页面是主界面
        if (Input.GetButtonDown(InputInfo.Interactive_E)&& baseUIComponent != currentUIComponent && currentUIComponent as UIGameMain)
        {
            if (!CheckUtil.StringIsNull(remarkData))
            {
             
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