using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIMainMaker : BaseUIComponent
{
    public Button btBack;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OnClickBack);
    }


    public void OnClickBack()
    {        
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);

        UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>();
    }
}