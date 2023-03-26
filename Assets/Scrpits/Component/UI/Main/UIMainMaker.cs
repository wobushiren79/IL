using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public partial class UIMainMaker : BaseUIComponent
{
    public override void OnClickForButton(Button viewButton)
    {
        if (viewButton == ui_Back)
        {
            OnClickBack();
        }
    }


    public void OnClickBack()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);

        UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>();
    }
}