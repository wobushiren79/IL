using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameSetting : UIGameComponent, DropdownView.ICallBack, ProgressView.ICallBack
{
    public Button btBack;
    public DropdownView dvLanguage;
    public ProgressView pvMusic;
    public ProgressView pvSound;

    public void Start()
    {
        if (btBack != null)
        {
            btBack.onClick.AddListener(OnClickBack);
        }
        if (dvLanguage != null)
        {
            dvLanguage.SetCallBack(this);
            List<Dropdown.OptionData> listLanguage = new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData("简体中文")
               //new Dropdown.OptionData("English")
            };
            dvLanguage.SetData(listLanguage);
            switch (GameCommonInfo.GameConfig.language)
            {
                case "cn":
                    dvLanguage.SetPosition("简体中文");
                    break;    
            }
  
        }
        if (pvMusic != null)
        {
            pvMusic.SetData(GameCommonInfo.GameConfig.musicVolume);
            pvMusic.SetCallBack(this);
        }
        if (pvSound != null)
        {
            pvSound.SetData(GameCommonInfo.GameConfig.soundVolume);
            pvSound.SetCallBack(this);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();


    }

    /// <summary>
    /// 退出点击
    /// </summary>
    public void OnClickBack()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        if (SceneUtil.GetCurrentScene() == ScenesEnum.MainScene)
        {
            uiManager.OpenUIAndCloseOther(UIEnum.MainStart);
        }
        else
        {
            uiManager.OpenUIAndCloseOther(UIEnum.GameMain);
        }
        GameCommonInfo.SaveGameConfig();
    }

    #region 下拉回调
    public void OnDropDownValueChange(DropdownView view, int position, Dropdown.OptionData optionData)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        string languageStr = "cn";
        if (view == dvLanguage)
        {
            switch (optionData.text)
            {
                case "简体中文":
                    languageStr = "cn";
                    break;
            }
        }
        GameCommonInfo.GameConfig.language = languageStr;
    }
    #endregion


    #region 进度条回调
    public void OnProgressViewValueChange(ProgressView progressView, float value)
    {
        if (progressView == pvMusic)
        {
            GameCommonInfo.GameConfig.musicVolume = value;
        }
        else if(progressView == pvSound)
        {
            GameCommonInfo.GameConfig.soundVolume = value;
        }
        uiGameManager.audioHandler.InitAudio();
    }
    #endregion

}