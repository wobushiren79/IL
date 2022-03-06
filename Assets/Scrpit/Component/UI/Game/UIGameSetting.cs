using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameSetting : BaseUIComponent, DropdownView.ICallBack, ProgressView.ICallBack, DialogView.IDialogCallBack, IRadioButtonCallBack
{
    public Button btExitGame;
    public Button btGoMain;
    public Button btRestartDay;

    public Button btBack;
    public DropdownView dvLanguage;
    public DropdownView dvWindow;
    public DropdownView dvCheckOut;

    public ProgressView pvMusic;
    public ProgressView pvSound;
    public ProgressView pvEnvironment;

    public RadioButtonView rbFrames;
    public RadioButtonView rbMouseMove;
    public RadioButtonView rbKeyTip;
    public RadioButtonView rbEventCameraMove;
    public RadioButtonView rbEventStopTimeScale;
    public RadioButtonView rbEvent;
    public RadioButtonView rbWorkerNumber;
    public RadioButtonView rbPowerTest;
    public RadioButtonView rbTownerInfo;
    public InputField etFrames;

    public void Start()
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (btBack != null)
        {
            btBack.onClick.AddListener(OnClickBack);
        }
        if (dvWindow != null)
        {
            dvWindow.SetCallBack(this);
            List<Dropdown.OptionData> listWindow = new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData("窗口"),
                new Dropdown.OptionData("全屏")
            };
            dvWindow.SetData(listWindow);

            switch (gameConfig.window)
            {
                case 0:
                    dvWindow.SetPosition("窗口");
                    break;
                case 1:
                    dvWindow.SetPosition("全屏");
                    break;
            }

        }

        //结账选择出世法U
        if (dvCheckOut != null)
        {
            dvCheckOut.SetCallBack(this);
            List<Dropdown.OptionData> listCheckOut = new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData("选择最近的柜台结账"),
                new Dropdown.OptionData("选择随机的柜台结账"),
                new Dropdown.OptionData("选择人少的柜台结账")
            };
            dvCheckOut.SetData(listCheckOut);
            switch (gameConfig.statusForCheckOut)
            {
                case 0:
                    dvCheckOut.SetPosition(0);
                    break;
                case 1:
                    dvCheckOut.SetPosition(1);
                    break;
                case 2:
                    dvCheckOut.SetPosition(2);
                    break;
            }
        }

        //语言选择初始化
        if (dvLanguage != null)
        {
            dvLanguage.SetCallBack(this);
            List<Dropdown.OptionData> listLanguage = new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData("简体中文")
               //new Dropdown.OptionData("English")
            };
            dvLanguage.SetData(listLanguage);
            switch (GameDataHandler.Instance.manager.GetGameConfig().language)
            {
                case "cn":
                    dvLanguage.SetPosition("简体中文");
                    break;
            }

        }




        //音乐选择初始化
        if (pvMusic != null)
        {
            pvMusic.SetData(gameConfig.musicVolume);
            pvMusic.SetCallBack(this);
        }
        //音效选择初始化
        if (pvSound != null)
        {
            pvSound.SetData(gameConfig.soundVolume);
            pvSound.SetCallBack(this);
        }
        //环境音乐初始化
        if (pvEnvironment != null)
        {
            pvEnvironment.SetData(gameConfig.environmentVolume);
            pvEnvironment.SetCallBack(this);
        }

        //帧数设定初始化
        if (rbFrames != null)
        {
            rbFrames.SetCallBack(this);
            if (gameConfig.statusForFrames == 0)
            {
                rbFrames.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (gameConfig.statusForFrames == 1)
            {
                rbFrames.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }
        if (etFrames != null)
        {
            etFrames.text = gameConfig.frames + "";
            etFrames.onEndEdit.AddListener(OnValueChangeForFrame);
        }

        //鼠标移动初始化
        if (rbMouseMove != null)
        {
            rbMouseMove.SetCallBack(this);
            if (gameConfig.statusForMouseMove == 0)
            {
                rbMouseMove.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (gameConfig.statusForMouseMove == 1)
            {
                rbMouseMove.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //按键提示初始化
        if (rbKeyTip != null)
        {
            rbKeyTip.SetCallBack(this);
            if (gameConfig.statusForKeyTip == 0)
            {
                rbKeyTip.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (gameConfig.statusForKeyTip == 1)
            {
                rbKeyTip.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }
        //事件镜头跟随初始化
        if (rbEventCameraMove != null)
        {
            rbEventCameraMove.SetCallBack(this);
            if (gameConfig.statusForEventCameraMove == 0)
            {
                rbEventCameraMove.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (gameConfig.statusForEventCameraMove == 1)
            {
                rbEventCameraMove.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }
        //事件倍速停止初始化
        if (rbEventStopTimeScale != null)
        {
            rbEventStopTimeScale.SetCallBack(this);
            if (gameConfig.statusForEventStopTimeScale == 0)
            {
                rbEventStopTimeScale.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (gameConfig.statusForEventStopTimeScale == 1)
            {
                rbEventStopTimeScale.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //事件镜头跟随初始化
        if (rbEvent != null)
        {
            rbEvent.SetCallBack(this);
            if (gameConfig.statusForEvent == 0)
            {
                rbEvent.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (gameConfig.statusForEvent == 1)
            {
                rbEvent.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //员工工作状态
        if (rbWorkerNumber != null)
        {
            rbWorkerNumber.SetCallBack(this);
            if (gameConfig.statusForWorkerNumber == 0)
            {
                rbWorkerNumber.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (gameConfig.statusForWorkerNumber == 1)
            {
                rbWorkerNumber.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //战斗的力道设定
        if (rbPowerTest != null)
        {
            rbPowerTest.SetCallBack(this);
            if (gameConfig.statusForCombatForPowerTest == 0)
            {
                rbPowerTest.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (gameConfig.statusForCombatForPowerTest == 1)
            {
                rbPowerTest.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //盘龙塔详细信息
        if (rbTownerInfo != null)
        {
            rbTownerInfo.SetCallBack(this);
            if (!gameConfig.isShowDetailsForTower)
            {
                rbTownerInfo.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else
            {
                rbTownerInfo.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        btExitGame.onClick.AddListener(OnClickExitGame);
        btGoMain.onClick.AddListener(OnClickGoMain);
        btRestartDay.onClick.AddListener(OnClickRestartDay);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        //离开游戏回到主菜单初始化
        if (SceneUtil.GetCurrentScene() == ScenesEnum.MainScene)
        {
            btExitGame.gameObject.SetActive(false);
            btGoMain.gameObject.SetActive(false);
            btRestartDay.gameObject.SetActive(false);
        }
        else
        {
            btExitGame.gameObject.SetActive(true);
            btGoMain.gameObject.SetActive(true);
            btRestartDay.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 退出点击
    /// </summary>
    public void OnClickBack()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        if (SceneUtil.GetCurrentScene() == ScenesEnum.MainScene)
        {
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainStart>(UIEnum.MainStart);
        }
        else
        {
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        }
        GameDataHandler.Instance.manager.SaveGameConfig();
    }



    /// <summary>
    /// 点击离开游戏
    /// </summary>
    public void OnClickExitGame()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogBean = new DialogBean();
        dialogBean.dialogPosition = 1;
        dialogBean.content = TextHandler.Instance.manager.GetTextById(3081);
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogBean);
    }

    /// <summary>
    /// 点击前往主界面
    /// </summary>
    public void OnClickGoMain()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogBean = new DialogBean();
        dialogBean.dialogPosition = 2;
        dialogBean.content = TextHandler.Instance.manager.GetTextById(3082);
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogBean);
    }

    /// <summary>
    /// 读档重来
    /// </summary>
    public void OnClickRestartDay()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogBean = new DialogBean();
        dialogBean.dialogPosition = 3;
        dialogBean.content = TextHandler.Instance.manager.GetTextById(3083);
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogBean);
    }

    /// <summary>
    /// 帧数修改
    /// </summary>
    /// <param name="value"></param>
    public void OnValueChangeForFrame(string value)
    {
        if (int.TryParse(value,out int result)) {
            if (result < 30)
            {
                etFrames.text = "30";
                return;
            }
            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            gameConfig.frames = result;      
            FPSHandler.Instance.SetData(gameConfig.statusForFrames, gameConfig.frames);
        }
    }   

    #region 下拉回调
    public void OnDropDownValueChange(DropdownView view, int position, Dropdown.OptionData optionData)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (view== dvLanguage)
        {
            string languageStr = "cn";
            switch (optionData.text)
            {
                case "简体中文":
                    languageStr = "cn";
                    break;
            }
            GameDataHandler.Instance.manager.GetGameConfig().language = languageStr;
        }
        else if (view == dvWindow)
        {
            int windowType = 0;
            switch (optionData.text)
            {
                case "窗口":
                    windowType = 0;
                    Screen.fullScreen = false;
                    break;
                case "全屏":
                    windowType = 1;
                    //获取设置当前屏幕分辩率 
                    Resolution[] resolutions = Screen.resolutions;
                    //设置当前分辨率 
                    //Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height , true);
                    Screen.SetResolution(1920, 1080, true);
                    Screen.fullScreen = true;
                    break;
            }
            gameConfig.window = windowType;
        }
        else if (view == dvCheckOut)
        {
            gameConfig.statusForCheckOut = position;
        }
    }
    #endregion

    #region 进度条回调
    public void OnProgressViewValueChange(ProgressView progressView, float value)
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (progressView == pvMusic)
        {
            gameConfig.musicVolume = value;
        }
        else if (progressView == pvSound)
        {
            gameConfig.soundVolume = value;
        }
        else if (progressView == pvEnvironment)
        {
            gameConfig.environmentVolume = value;
        }
        AudioHandler.Instance.InitAudio();
    }


    #endregion

    #region 弹窗确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogBean.dialogPosition == 1)
        {
            //离开游戏
            GameUtil.ExitGame();
        }
        else if (dialogBean.dialogPosition == 2)
        {
            //回调主菜单       
            GameScenesHandler.Instance.ChangeScene(ScenesEnum.MainScene);
            GameCommonInfo.ClearData();
        }
        else if (dialogBean.dialogPosition == 3)
        {
            GameDataBean gameData =  GameDataHandler.Instance.manager.GetGameData();
            GameDataHandler.Instance.manager.GetGameDataByUserId(gameData.userId);
            GameCommonInfo.ClearData();
            GameScenesHandler.Instance.ChangeScene(ScenesEnum.GameInnScene);
        }

    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }

    #endregion


    #region checkBox回调
    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStates)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (view == rbKeyTip)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.statusForKeyTip = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.statusForKeyTip = 0;
            }
        }
        else if (view == rbFrames)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.statusForFrames = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.statusForFrames = 0;
            }
            FPSHandler.Instance.SetData(gameConfig.statusForFrames, gameConfig.frames);
        }
        else if (view == rbMouseMove)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.statusForMouseMove = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.statusForMouseMove = 0;
            }
        }
        else if (view == rbEventCameraMove)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.statusForEventCameraMove = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.statusForEventCameraMove = 0;
            }
        }
        else if (view == rbEventStopTimeScale)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.statusForEventStopTimeScale = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.statusForEventStopTimeScale = 0;
            }
        }
        else if (view == rbEvent)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.statusForEvent = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.statusForEvent = 0;
            }
        }
        else if (view == rbWorkerNumber)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.statusForWorkerNumber = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.statusForWorkerNumber = 0;
            }
        }
        else if (view == rbPowerTest)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.statusForCombatForPowerTest = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.statusForCombatForPowerTest = 0;
            }
        }
        else if (view== rbTownerInfo)
        {
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                gameConfig.isShowDetailsForTower = true;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                gameConfig.isShowDetailsForTower = false;
            }
        }
    }
    #endregion
}