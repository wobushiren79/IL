using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public partial class UIMainStart : BaseUIComponent
{

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if(viewButton == ui_BTStart)
        {
            OpenCreateUI();
        }
        if (viewButton == ui_BTContinue)
        {
            OpenContinueUI();
        }
        if (viewButton == ui_BTSetting)
        {
            OpenSettingUI();
        }
        if (viewButton == ui_BTExit)
        {
            ExitGame();
        }
        if (viewButton == ui_TitleBT)
        {
            OpenMakerUI();
        }
    }

    private void Start()
    {
        SetTexts();
        SetVersion(ProjectConfigInfo.GAME_VERSION);
    }

    /// <summary>
    /// 刷新UI：每次打开主界面（含语言切换后刷新）都会调用，
    /// 保证按钮文本跟随当前语言（Start 只在首次实例化时执行一次）
    /// </summary>
    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        SetTexts();
    }

    /// <summary>
    /// 设置主界面按钮文本（语言相关）
    /// </summary>
    private void SetTexts()
    {
        if (ui_BTStartText != null)
            ui_BTStartText.text = TextHandler.Instance.GetTextById(4011);
        if (ui_BTContinueText != null)
            ui_BTContinueText.text = TextHandler.Instance.GetTextById(4012);
        if (ui_BTSettingText != null)
            ui_BTSettingText.text = TextHandler.Instance.GetTextById(4013);
        if (ui_BTExitText != null)
            ui_BTExitText.text = TextHandler.Instance.GetTextById(4014);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        AnimForInit();
    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit()
    {
        if (ui_BTStart != null)
        {
            ui_BTStart.transform.localScale = Vector3.one;
            ui_BTStart.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack);
        }
        if (ui_BTContinue != null)
        {
            ui_BTContinue.transform.localScale = Vector3.one;
            ui_BTContinue.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.1f);
        }
        if (ui_BTSetting != null)
        {
            ui_BTSetting.transform.localScale = Vector3.one;
            ui_BTSetting.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.2f);
        }
        if (ui_BTExit != null)
        {
            ui_BTExit.transform.localScale = Vector3.one;
            ui_BTExit.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.3f);
        }
    }

    /// <summary>
    /// 设置版本号
    /// </summary>
    /// <param name="version"></param>
    public void SetVersion(string version)
    {
        if (ui_Version != null)
        {
            ui_Version.text = "ver " + version;
        }
    }

    /// <summary>
    /// 打开继续页面
    /// </summary>
    public void OpenContinueUI()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIMainContinue>();
    }


    /// <summary>
    /// 打开创建页面
    /// </summary>
    public void OpenCreateUI()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIMainCreate>();
    }

    /// <summary>
    /// 打开创建页面
    /// </summary>
    public void OpenSettingUI()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameSetting>();
    }

    /// <summary>
    /// 打开制作人UI
    /// </summary>
    public void OpenMakerUI()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIMainMaker>();
    }

    /// <summary>
    /// 离开游戏
    /// </summary>
    public void ExitGame()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameUtil.ExitGame();
    }

}
