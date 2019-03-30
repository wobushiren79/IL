using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIMainCreate : BaseUIComponent
{
    //返回按钮
    public Button btBack;
    public Text tvBack;

    private void Start()
    {
        if (btBack != null)
        {
            btBack.onClick.AddListener(OpenStartUI);
        }; 
    }

    /// <summary>
    /// 返回开始菜单
    /// </summary>
    public void OpenStartUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Start");
    }
}