using UnityEngine;
using UnityEditor;

public class InteractiveUICpt : BaseInteractiveCpt
{
    public string interactiveContent;
    public UIEnum uiType;

    //备注信息
    public string remarkData;


    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {

        //如果当前页面不是即将要打开的页面 并且当前页面是主界面
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            BaseUIComponent baseUIComponent = UIHandler.Instance.manager.GetUI<BaseUIComponent>(uiType);
            BaseUIComponent currentUIComponent = UIHandler.Instance.manager.GetOpenUI();
            if (baseUIComponent != null)
            {
                if (currentUIComponent == baseUIComponent)
                {

                }
                else
                {
                    baseUIComponent = UIHandler.Instance.manager.OpenUIAndCloseOther<BaseUIComponent>(uiType);
                    if (!CheckUtil.StringIsNull(remarkData))
                        baseUIComponent.SetRemarkData(remarkData);
                }
            }
            else
            {
                baseUIComponent = UIHandler.Instance.manager.OpenUIAndCloseOther<BaseUIComponent>(uiType);
                if (!CheckUtil.StringIsNull(remarkData))
                    baseUIComponent.SetRemarkData(remarkData);
            }    
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