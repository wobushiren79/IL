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
            BaseUIComponent baseUIComponent = UIHandler.Instance.GetUI<BaseUIComponent>();
            BaseUIComponent currentUIComponent = UIHandler.Instance.GetOpenUI();
            if (baseUIComponent != null)
            {
                if (currentUIComponent == baseUIComponent)
                {

                }
                else
                {
                    baseUIComponent = UIHandler.Instance.OpenUIAndCloseOther<BaseUIComponent>();
                    if (!remarkData.IsNull())
                        baseUIComponent.SetRemarkData(remarkData);
                }
            }
            else
            {
                baseUIComponent = UIHandler.Instance.OpenUIAndCloseOther<BaseUIComponent>();
                if (!remarkData.IsNull())
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