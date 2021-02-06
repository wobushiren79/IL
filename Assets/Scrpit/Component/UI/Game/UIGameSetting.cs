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
    public InputField etFrames;

    public void Start()
    {
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
            switch (GameCommonInfo.GameConfig.window)
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
            switch (GameCommonInfo.GameConfig.statusForCheckOut)
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
            switch (GameCommonInfo.GameConfig.language)
            {
                case "cn":
                    dvLanguage.SetPosition("简体中文");
                    break;
            }

        }




        //音乐选择初始化
        if (pvMusic != null)
        {
            pvMusic.SetData(GameCommonInfo.GameConfig.musicVolume);
            pvMusic.SetCallBack(this);
        }
        //音效选择初始化
        if (pvSound != null)
        {
            pvSound.SetData(GameCommonInfo.GameConfig.soundVolume);
            pvSound.SetCallBack(this);
        }
        //环境音乐初始化
        if (pvEnvironment != null)
        {
            pvEnvironment.SetData(GameCommonInfo.GameConfig.environmentVolume);
            pvEnvironment.SetCallBack(this);
        }

        //帧数设定初始化
        if (rbFrames != null)
        {
            rbFrames.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForFrames == 0)
            {
                rbFrames.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForFrames == 1)
            {
                rbFrames.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }
        if (etFrames != null)
        {
            etFrames.text = GameCommonInfo.GameConfig.frames + "";
            etFrames.onEndEdit.AddListener(OnValueChangeForFrame);
        }

        //鼠标移动初始化
        if (rbMouseMove != null)
        {
            rbMouseMove.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForMouseMove == 0)
            {
                rbMouseMove.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForMouseMove == 1)
            {
                rbMouseMove.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //按键提示初始化
        if (rbKeyTip != null)
        {
            rbKeyTip.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForKeyTip == 0)
            {
                rbKeyTip.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForKeyTip == 1)
            {
                rbKeyTip.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }
        //事件镜头跟随初始化
        if (rbEventCameraMove != null)
        {
            rbEventCameraMove.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForEventCameraMove == 0)
            {
                rbEventCameraMove.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForEventCameraMove == 1)
            {
                rbEventCameraMove.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }
        //事件倍速停止初始化
        if (rbEventStopTimeScale != null)
        {
            rbEventStopTimeScale.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForEventStopTimeScale == 0)
            {
                rbEventStopTimeScale.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForEventStopTimeScale == 1)
            {
                rbEventStopTimeScale.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //事件镜头跟随初始化
        if (rbEvent != null)
        {
            rbEvent.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForEvent == 0)
            {
                rbEvent.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForEvent == 1)
            {
                rbEvent.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //员工工作状态
        if (rbWorkerNumber != null)
        {
            rbWorkerNumber.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForWorkerNumber == 0)
            {
                rbWorkerNumber.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForWorkerNumber == 1)
            {
                rbWorkerNumber.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

        //战斗的力道设定
        if (rbPowerTest != null)
        {
            rbPowerTest.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForCombatForPowerTest == 0)
            {
                rbPowerTest.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForCombatForPowerTest == 1)
            {
                rbPowerTest.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
        }

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
        btExitGame.onClick.AddListener(OnClickExitGame);
        btGoMain.onClick.AddListener(OnClickGoMain);
        btRestartDay.onClick.AddListener(OnClickRestartDay);
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        if (SceneUtil.GetCurrentScene() == ScenesEnum.MainScene)
        {
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainStart>(UIEnum.MainStart);
        }
        else
        {
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        }
        GameCommonInfo.SaveGameConfig();
    }



    /// <summary>
    /// 点击离开游戏
    /// </summary>
    public void OnClickExitGame()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogBean = new DialogBean();
        dialogBean.dialogPosition = 1;
        dialogBean.content = GameCommonInfo.GetUITextById(3081);
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
        dialogBean.content = GameCommonInfo.GetUITextById(3082);
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
        dialogBean.content = GameCommonInfo.GetUITextById(3083);
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
            GameCommonInfo.GameConfig.frames = result;      
            FPSHandler.Instance.SetData(GameCommonInfo.GameConfig.statusForFrames, GameCommonInfo.GameConfig.frames);
        }
    }   

    #region 下拉回调
    public void OnDropDownValueChange(DropdownView view, int position, Dropdown.OptionData optionData)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (view== dvLanguage)
        {
            string languageStr = "cn";
            switch (optionData.text)
            {
                case "简体中文":
                    languageStr = "cn";
                    break;
            }
            GameCommonInfo.GameConfig.language = languageStr;
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
                    Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
                    Screen.fullScreen = true;
                    break;
            }
            GameCommonInfo.GameConfig.window = windowType;
        }
        else if (view == dvCheckOut)
        {
            GameCommonInfo.GameConfig.statusForCheckOut = position;
        }
    }
    #endregion

    #region 进度条回调
    public void OnProgressViewValueChange(ProgressView progressView, float value)
    {
        if (progressView == pvMusic)
        {
            GameCommonInfo.GameConfig.musicVolume = value;
        }
        else if (progressView == pvSound)
        {
            GameCommonInfo.GameConfig.soundVolume = value;
        }
        else if (progressView == pvEnvironment)
        {
            GameCommonInfo.GameConfig.environmentVolume = value;
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
        if (view == rbKeyTip)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForKeyTip = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForKeyTip = 0;
            }
        }
        else if (view == rbFrames)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForFrames = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForFrames = 0;
            }
            FPSHandler.Instance.SetData(GameCommonInfo.GameConfig.statusForFrames, GameCommonInfo.GameConfig.frames);
        }
        else if (view == rbMouseMove)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForMouseMove = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForMouseMove = 0;
            }
        }
        else if (view == rbEventCameraMove)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForEventCameraMove = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForEventCameraMove = 0;
            }
        }
        else if (view == rbEventStopTimeScale)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForEventStopTimeScale = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForEventStopTimeScale = 0;
            }
        }
        else if (view == rbEvent)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForEvent = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForEvent = 0;
            }
        }
        else if (view == rbWorkerNumber)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForWorkerNumber = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForWorkerNumber = 0;
            }
        }
        else if (view == rbPowerTest)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForCombatForPowerTest = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForCombatForPowerTest = 0;
            }
        }
    }
    #endregion
}