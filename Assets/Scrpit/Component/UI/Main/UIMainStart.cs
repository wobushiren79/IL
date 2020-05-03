using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIMainStart : UIGameComponent
{
    public Button btMaker;
    public Text tvTitle;
    public Text tvVersion;

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
        if (btContinue != null)
        {
            btContinue.onClick.AddListener(OpenContinueUI);
        }
        if (btSetting != null)
        {
            btSetting.onClick.AddListener(OpenSettingUI);
        }
        if (btExit != null)
        {
            btExit.onClick.AddListener(ExitGame);
        }
        if (btMaker != null)
        {
            btMaker.onClick.AddListener(OpenMakerUI);
        }
        if (tvStart != null)
            tvStart.text = GameCommonInfo.GetUITextById(4011);
        if (tvContinue != null)
            tvContinue.text = GameCommonInfo.GetUITextById(4012);
        if (tvSetting != null)
            tvSetting.text = GameCommonInfo.GetUITextById(4013);
        if (tvExit != null)
            tvExit.text = GameCommonInfo.GetUITextById(4014);

        SetVersion(ProjectConfigInfo.GAME_VERSION);
        AnimForInit();
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
        if (btStart != null)
            btStart.transform.DOScaleX(0,0.5f).From().SetEase(Ease.OutBack);
        if (btContinue != null)
            btContinue.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.1f);
        if (btSetting != null)
            btSetting.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.2f);
        if (btExit != null)
            btExit.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.3f);
    }

    /// <summary>
    /// 设置版本号
    /// </summary>
    /// <param name="version"></param>
    public void SetVersion(string version)
    {
        if (tvVersion != null)
        {
            tvVersion.text = "ver " + version;
        }
    }

    /// <summary>
    /// 打开继续页面
    /// </summary>
    public void OpenContinueUI()
    {
        //按键音效
        uiGameManager.audioHandler.PlaySound( AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOther(UIEnum.MainContinue);
    }


    /// <summary>
    /// 打开创建页面
    /// </summary>
    public void OpenCreateUI()
    {
        //按键音效
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOther(UIEnum.MainCreate);
    }

    /// <summary>
    /// 打开创建页面
    /// </summary>
    public void OpenSettingUI()
    {
        //按键音效
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOther(UIEnum.GameSetting);
    }

    /// <summary>
    /// 打开制作人UI
    /// </summary>
    public void OpenMakerUI()
    {
        //按键音效
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOther(UIEnum.MainMaker);
    }

    /// <summary>
    /// 离开游戏
    /// </summary>
    public void ExitGame()
    {
        //按键音效
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameUtil.ExitGame();
    }

}
