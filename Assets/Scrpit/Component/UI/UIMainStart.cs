using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainStart : BaseUIComponent
{
    //开始按钮
    public Button btStart;
    public Text tvStart;
    //继续按钮
    public Button btContinue;
    public Text tvContinue;
    //设置按钮
    public Button btSetting;
    public Text tvSetting;
    //离开按钮
    public Button btExit;
    public Text tvExit;

    private void Start()
    {
        if (btStart != null)
        {
            btStart.onClick.AddListener(OpenCreateUI);
        }
        if (btExit != null)
        {
            btExit.onClick.AddListener(ExitGame);
        }
     
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
         
    }

    /// <summary>
    /// 打开创建页面
    /// </summary>
    public void OpenCreateUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Create");
    }

    /// <summary>
    /// 离开游戏
    /// </summary>
    public void ExitGame()
    {
        GameUtil.ExitGame();
    }
}
