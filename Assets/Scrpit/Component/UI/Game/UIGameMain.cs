using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class UIGameMain : UIGameComponent, DialogView.IDialogCallBack, IRadioGroupCallBack, IBaseObserver
{
    [Header("控件")]
    public InfoPromptPopupButton popupWorker;
    public Button btWorker;
    public InfoPromptPopupButton popupBuild;
    public Button btBuild;
    public InfoPromptPopupButton popupMenu;
    public Button btMenu;
    public InfoPromptPopupButton popupBackpack;
    public Button btBackpack;
    public InfoPromptPopupButton popupFavorability;
    public Button btFavorability;
    public InfoPromptPopupButton popupSave;
    public Button btSave;
    public InfoPromptPopupButton popupInnData;
    public Button btInnData;

    public Button btSleep;

    public Text tvInnStatus;
    public Text tvMoneyS;
    public Text tvMoneyM;
    public Text tvMoneyL;

    public InfoPromptPopupButton popupAesthetics;
    public Text tvAesthetics;
    public InfoPromptPopupButton popupPraise;
    public Slider sliderPraise;
    public Text tvPraise;
    public InfoPromptPopupButton popupRichness;
    public Text tvRichness;
    public InfoPromptPopupButton popupInnLevel;
    public Image ivInnLevel;

    public ClockView clockView;//时钟
    public RadioGroupView rgTimeScale;
    public RadioButtonView rbTimeScale2;
    public RadioButtonView rbTimeScale3;
    public RadioButtonView rbTimeScale5;


    protected Tween tweenForMoneyL;
    protected Tween tweenForMoneyM;
    protected Tween tweenForMoneyS;

    public override void Awake()
    {
        base.Awake();
        uiGameManager.gameDataHandler.AddObserver(this);
    }

    public void Start()
    {
        if (btWorker != null)
            btWorker.onClick.AddListener(OpenWorkerUI);

        if (btBuild != null)
            btBuild.onClick.AddListener(OpenBuildUI);

        if (btMenu != null)
            btMenu.onClick.AddListener(OpenMenuUI);

        if (btBackpack != null)
            btBackpack.onClick.AddListener(OpenBackpackUI);

        if (btFavorability != null)
            btFavorability.onClick.AddListener(OpenFavorabilityUI);

        if (btInnData != null)
            btInnData.onClick.AddListener(OpenStatisticsUI);

        if (btSave != null)
            btSave.onClick.AddListener(SaveData);

        if (btSleep != null)
            btSleep.onClick.AddListener(EndDay);

        if (popupWorker != null)
            popupWorker.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupBuild != null)
            popupBuild.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupMenu != null)
            popupMenu.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupBackpack != null)
            popupBackpack.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupFavorability != null)
            popupFavorability.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupSave != null)
            popupSave.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupInnData != null)
            popupInnData.SetPopupShowView(uiGameManager.infoPromptPopup);

        if (popupAesthetics != null)
            popupAesthetics.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupPraise != null)
            popupPraise.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupRichness != null)
            popupRichness.SetPopupShowView(uiGameManager.infoPromptPopup);
        if (popupInnLevel != null)
            popupInnLevel.SetPopupShowView(uiGameManager.infoPromptPopup);

        if (rgTimeScale != null)
            rgTimeScale.SetCallBack(this);
        InitInnData();
    }

    private void Update()
    {
        InnAttributesBean innAttributes = uiGameManager.gameDataManager.gameData.GetInnAttributesData();
        if (tvInnStatus != null && uiGameManager.innHandler != null)
            if (uiGameManager.innHandler.GetInnStatus() == InnHandler.InnStatusEnum.Close)
            {
                tvInnStatus.text = GameCommonInfo.GetUITextById(2002);
            }
            else
            {
                tvInnStatus.text = GameCommonInfo.GetUITextById(2001);
            }
        if (clockView != null && uiGameManager.gameTimeHandler != null)
        {
            uiGameManager.gameTimeHandler.GetTime(out float hour, out float min);
            uiGameManager.gameTimeHandler.GetTime(out int year, out int month, out int day);
            clockView.SetTime(month, day, (int)hour, (int)min);
        }
        SetInnPraise(innAttributes);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        SetMoney(MoneyEnum.L, uiGameManager.gameDataManager.gameData.moneyL);
        SetMoney(MoneyEnum.M, uiGameManager.gameDataManager.gameData.moneyM);
        SetMoney(MoneyEnum.S, uiGameManager.gameDataManager.gameData.moneyS);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        //时间加速回归正常
        if (uiGameManager.gameTimeHandler != null)
        {
            uiGameManager.gameTimeHandler.SetTimeScale(1);
            rgTimeScale.SetPosition(-1, false);
        }
        uiGameManager.controlHandler.StopControl();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitInnData();
        uiGameManager.controlHandler.RestoreControl();
        RefreshUI();
    }

    /// <summary>
    /// 初始化客栈数据
    /// </summary>
    public void InitInnData()
    {
        InnAttributesBean innAttributes = uiGameManager.gameDataManager.gameData.GetInnAttributesData();
        if (innAttributes == null)
            return;

        if (popupWorker != null)
            popupWorker.SetContent(GameCommonInfo.GetUITextById(2031));
        if (popupBuild != null)
            popupBuild.SetContent(GameCommonInfo.GetUITextById(2032));
        if (popupMenu != null)
            popupMenu.SetContent(GameCommonInfo.GetUITextById(2033));
        if (popupBackpack != null)
            popupBackpack.SetContent(GameCommonInfo.GetUITextById(2034));
        if (popupFavorability != null)
            popupFavorability.SetContent(GameCommonInfo.GetUITextById(2035));
        if (popupSave != null)
            popupSave.SetContent(GameCommonInfo.GetUITextById(2036));
        if (popupInnData != null)
            popupInnData.SetContent(GameCommonInfo.GetUITextById(2037));
        if (popupPraise != null)
            popupPraise.SetContent(GameCommonInfo.GetUITextById(2004) + " " + innAttributes.praise + "%");

        SetInnAesthetics(innAttributes);

        if (popupRichness != null)
            popupRichness.SetContent(GameCommonInfo.GetUITextById(2005) + " " + innAttributes.richness);
        if (tvRichness != null)
            tvRichness.text = innAttributes.richness + "";

        SetInnLevel(innAttributes);

        //设置是否显示时间缩放
        if (uiGameManager.innHandler == null)
        {
            rgTimeScale.gameObject.SetActive(false);
        }
        else
        {
            if (uiGameManager.innHandler.GetInnStatus() == InnHandler.InnStatusEnum.Close)
            {
                rgTimeScale.gameObject.SetActive(false);
            }
            else
            {
                rgTimeScale.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    /// <param name="moneyType"></param>
    /// <param name="price"></param>
    public void SetMoney(MoneyEnum moneyType, long price)
    {
        switch (moneyType)
        {
            case MoneyEnum.L:
                if (tvMoneyL != null)
                    tvMoneyL.text = price + "";
                break;
            case MoneyEnum.M:
                if (tvMoneyM != null)
                    tvMoneyM.text = price + "";
                break;
            case MoneyEnum.S:
                if (tvMoneyS != null)
                    tvMoneyS.text = price + "";
                break;
        }
    }

    /// <summary>
    /// 设置美观值
    /// </summary>
    public void SetInnAesthetics(InnAttributesBean innAttributes)
    {
        long aesthetics = innAttributes.GetAesthetics(out string aestheticsLevel);
        if (popupAesthetics != null)
        {
            popupAesthetics.SetContent(GameCommonInfo.GetUITextById(2003) + " " + aesthetics + " " + aestheticsLevel);
        }
        if (tvAesthetics != null)
        {
            tvAesthetics.text = aesthetics + " " + aestheticsLevel;
        }
    }

    /// <summary>
    /// 设置客栈等级
    /// </summary>
    /// <param name="innAttributes"></param>
    public void SetInnLevel(InnAttributesBean innAttributes)
    {
        //设置客栈等级
        string innLevelStr = innAttributes.GetInnLevel(out int innLevelTitle, out int innLevelStar);
        if (popupInnLevel != null)
        {
            popupInnLevel.SetContent(GameCommonInfo.GetUITextById(2006) + " " + innLevelStr);
        }

        if (ivInnLevel != null)
        {
            Sprite spIcon = uiGameManager.iconDataManager.GetIconSpriteByName("inn_level_" + innLevelTitle + "_" + (innLevelStar - 1));
            if (spIcon)
            {
                ivInnLevel.gameObject.SetActive(true);
                ivInnLevel.sprite = spIcon;
            }
            else
                ivInnLevel.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置客栈点赞
    /// </summary>
    /// <param name="innAttributes"></param>
    public void SetInnPraise(InnAttributesBean innAttributes)
    {
        if (sliderPraise != null)
        {
            sliderPraise.value = innAttributes.GetPraise();
        }
        if (tvPraise != null)
        {
            tvPraise.text = innAttributes.GetPraise() + "%";
        }
    }

    public void SaveData()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiGameManager.gameDataManager.SaveGameData(uiGameManager.innHandler.GetInnRecord());
    }

    public void OpenBuildUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (CheckUtil.ListIsNull(uiGameManager.innHandler.rascalrQueue))
        {
            DialogBean dialogBean = new DialogBean();
            dialogBean.content = GameCommonInfo.GetUITextById(3007);
            dialogBean.dialogPosition = 1;
            uiGameManager.dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
        }
        else
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1016));
        }
    }

    public void OpenWorkerUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
    }

    public void OpenMenuUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMenu));
    }

    public void OpenBackpackUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameBackpack));
    }

    public void OpenFavorabilityUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameFavorability));
    }

    public void OpenStatisticsUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameStatistics));
    }

    public void EndDay()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogBean = new DialogBean();
        dialogBean.content = GameCommonInfo.GetUITextById(3004);
        dialogBean.dialogPosition = 0;
        uiGameManager.dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
    }

    /// <summary>
    /// 增加金钱动画
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    private void AddMoneyAnim(long priceL, long priceM, long priceS)
    {
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        if (priceL != 0)
        {
            if (tweenForMoneyL != null)
                tweenForMoneyL.Kill();
            long startMoney = gameData.moneyL - priceL;
            tweenForMoneyL = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.L, x); }, gameData.moneyL, 1);
        }
        if (priceM != 0)
        {
            if (tweenForMoneyM != null)
                tweenForMoneyM.Kill();
            long startMoney = gameData.moneyM - priceM;
            tweenForMoneyM = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.M, x); }, gameData.moneyM, 1);
        }
        if (priceS != 0)
        {
            if (tweenForMoneyS != null)
                tweenForMoneyS.Kill();
            long startMoney = gameData.moneyS - priceS;
            tweenForMoneyS = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.S, x); }, gameData.moneyS, 1);
        }
    }

    #region dialog 回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        if (dialogData.dialogPosition == 0)
        {
            //结束一天
            Scene scene = SceneManager.GetActiveScene();
            //如果是客栈场景
            if (EnumUtil.GetEnumName(ScenesEnum.GameInnScene).Equals(scene.name))
            {
                ((SceneGameInnInit)uiGameManager.sceneInit).EndDay();
            }
            //如果是城镇 则先回到客栈
            else if (EnumUtil.GetEnumName(ScenesEnum.GameTownScene).Equals(scene.name))
            {
                ((SceneGameTownInit)uiGameManager.sceneInit).EndDay();
            }
        }
        else if (dialogData.dialogPosition == 1)
        {
            //打开建筑
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameBuild));
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion

    #region 单选回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        int timeScale = 1;
        if (rbview == rbTimeScale2)
        {
            timeScale = 2;
        }
        else if (rbview == rbTimeScale3)
        {
            timeScale = 3;
        }
        else if (rbview == rbTimeScale5)
        {
            timeScale = 5;
        }
        if (rbview.status == RadioButtonView.RadioButtonStatus.Selected)
        {
            uiGameManager.gameTimeHandler.SetTimeScale(timeScale);
        }
        else
        {
            uiGameManager.gameTimeHandler.SetTimeScale(1);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        if (observable as GameDataHandler)
        {
            if (type == (int)GameDataHandler.NotifyTypeEnum.AddMoney)
            {
                long priceL = System.Convert.ToInt64(obj[0]);
                long priceM = System.Convert.ToInt64(obj[1]);
                long priceS = System.Convert.ToInt64(obj[2]);
                AddMoneyAnim(priceL, priceM, priceS);
            }
        }
    }
    #endregion
}