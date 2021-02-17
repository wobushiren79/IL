using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
public class UIGameMain : BaseUIComponent, DialogView.IDialogCallBack, IRadioGroupCallBack
{
    [Header("控件")]
    public PopupPromptButton popupWorker;
    public Button btWorker;
    public PopupPromptButton popupBuild;
    public Button btBuild;
    public PopupPromptButton popupMenu;
    public Button btMenu;
    public PopupPromptButton popupBackpack;
    public Button btBackpack;
    public PopupPromptButton popupFavorability;
    public Button btFavorability;
    public PopupPromptButton popupSave;
    public Button btSave;
    public PopupPromptButton popupInnData;
    public Button btInnData;
    public PopupPromptButton popupHelp;
    public Button btHelp;
    public PopupPromptButton popupSetting;
    public Button btSetting;
    public PopupPromptButton popupJumpTime;
    public Button btJumpTime;
    public PopupPromptButton popupHotel;
    public Button btHotel;
    public PopupPromptButton popupFamily;
    public Button btFamily;


    public Button btSleep;

    public Text tvMoneyS;
    public Text tvMoneyM;
    public Text tvMoneyL;

    public PopupPromptButton popupAesthetics;
    public PopupPromptButton popupPraise;
    public PopupPromptButton popupRichness;
    public ProgressView proAesthetics;
    public ProgressView proPraise;
    public ProgressView proRichness;

    public PopupPromptButton popupInnLevel;
    public Image ivInnLevel;

    public ClockView clockView;//时钟
    public RadioGroupView rgTimeScale;
    public RadioButtonView rbTimeScaleStop;
    public RadioButtonView rbTimeScale2;
    public RadioButtonView rbTimeScale5;
    public RadioButtonView rbTimeScale8;
    public WorkerNumberView workerNumber;

    public Button btLayerFirstLayer;
    public Button btLayerSecondLayer;
    public GameObject objLayerSelect;

    protected Tween tweenForMoneyL;
    protected Tween tweenForMoneyM;
    protected Tween tweenForMoneyS;

    public Text tvMoneyForAnimModel;

    public UIGameMainForHint uiHint;

    public override void Awake()
    {
        base.Awake();
        GameDataHandler.Instance.RegisterNotifyForData(NotifyForData);
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

        if (btSetting != null)
            btSetting.onClick.AddListener(OpenSettingUI);

        if (btHelp != null)
            btHelp.onClick.AddListener(OpenHelpUI);

        if (btSave != null)
            btSave.onClick.AddListener(SaveData);

        if (btSleep != null)
            btSleep.onClick.AddListener(OnClickForEndDay);

        if (btJumpTime != null)
            btJumpTime.onClick.AddListener(OnClickForJumpTime);

        if (btHotel != null)
            btHotel.onClick.AddListener(OnClickForHotel);

        if (btFamily != null)
            btFamily.onClick.AddListener(OnClickForFamily);

        if (rgTimeScale != null)
            rgTimeScale.SetCallBack(this);

        if (btLayerFirstLayer != null)
            btLayerFirstLayer.onClick.AddListener(OnClickForFirstLayer);
        if (btLayerSecondLayer != null)
            btLayerSecondLayer.onClick.AddListener(OnClickForSecondLayer);

        InitInnData();
        RefreshUI();

        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //判断是否展示教程
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        if (!userAchievement.isOpenedHelp)
        {
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameHelp>(UIEnum.GameHelp);
        }
    }

    private void Update()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnAttributesBean innAttributes = gameData.GetInnAttributesData();
        if (clockView != null)
        {
            GameTimeHandler.Instance.GetTime(out float hour, out float min);
            GameTimeHandler.Instance.GetTime(out int year, out int month, out int day);
            clockView.SetTime(month, day, (int)hour, (int)min);
        }
        SetInnPraise(innAttributes);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        SetMoney(MoneyEnum.L, gameData.moneyL);
        SetMoney(MoneyEnum.M, gameData.moneyM);
        SetMoney(MoneyEnum.S, gameData.moneyS);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        //时间加速回归正常

        GameTimeHandler.Instance.SetTimeScale(1);
        rgTimeScale.SetPosition(-1, false);
        GameControlHandler.Instance.StopControl();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitInnData();
        GameControlHandler.Instance.RestoreControl();
        RefreshUI();
    }

    /// <summary>
    /// 初始化客栈数据
    /// </summary>
    public void InitInnData()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnAttributesBean innAttributes = gameData.GetInnAttributesData();

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
        if (popupSetting != null)
            popupSetting.SetContent(GameCommonInfo.GetUITextById(2038));
        if (popupHelp != null)
            popupHelp.SetContent(GameCommonInfo.GetUITextById(2039));
        if (popupJumpTime != null)
            popupJumpTime.SetContent(GameCommonInfo.GetUITextById(2040));
        if (popupHotel != null)
            popupHotel.SetContent(GameCommonInfo.GetUITextById(2041));
        if (popupFamily != null)
            popupFamily.SetContent(GameCommonInfo.GetUITextById(2042));
        SetInnPraise(innAttributes);
        SetInnAesthetics(innAttributes);
        SetInnRichNess(innAttributes);
        SetInnLevel(innAttributes);

        //设置是否显示时间缩放
        if (SceneUtil.GetCurrentScene() != ScenesEnum.GameInnScene)
        {
            rgTimeScale.gameObject.SetActive(false);
            objLayerSelect.SetActive(false);
        }
        else
        {
            if (InnHandler.Instance.GetInnStatus() == InnHandler.InnStatusEnum.Close)
            {
                rgTimeScale.gameObject.SetActive(false);
                objLayerSelect.SetActive(false);
            }
            else
            {
                rgTimeScale.gameObject.SetActive(true);
                InnBuildBean innBuild = gameData.GetInnBuildData();
                if (innBuild.innSecondWidth != 0 && innBuild.innSecondHeight != 0)
                {
                    objLayerSelect.SetActive(true);
                }
                else
                {
                    objLayerSelect.SetActive(false);
                }
            }
        }

        //设置是否显示时间跳跃
        //if (uiGameManager.gameTimeHandler == null)
        //{
        //    btJumpTime.gameObject.SetActive(false);
        //}
        //else
        //{
        //    if(GameTimeHandler.Instance.GetDayStatus()== GameTimeHandler.DayEnum.Rest)
        //    {
        //        btJumpTime.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        btJumpTime.gameObject.SetActive(false);
        //    }
        //}

        if (GameCommonInfo.GameConfig.statusForWorkerNumber == 0)
        {
            workerNumber.Close();
        }
        else
        {
            if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work)
            {
                workerNumber.Open();
            }
            else
            {
                workerNumber.Close();
            }
        }

        //是否展示住店相关
        if (gameData.listBed.Count != 0)
        {
            btHotel.gameObject.SetActive(true);
        }
        else
        {
            btHotel.gameObject.SetActive(false);
        }
        //是否展示建造按钮
        if(SceneUtil.GetCurrentScene() == ScenesEnum.GameInnScene)
        {
            btBuild.gameObject.SetActive(true);
        }
        else
        {
            btBuild.gameObject.SetActive(false);
        }
        //是否展示家族按钮
        if (gameData.GetFamilyData().CheckMarry(gameData.gameTime))
        {
            btFamily.gameObject.SetActive(true);
        }
        else
        {
            btFamily.gameObject.SetActive(false);
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
                {
                    tvMoneyL.text = price + "";
                    tvMoneyL.transform.localScale = new Vector3(1, 1, 1);
                }
                break;
            case MoneyEnum.M:
                if (tvMoneyM != null)
                {
                    tvMoneyM.text = price + "";
                    tvMoneyM.transform.localScale = new Vector3(1, 1, 1);
                }

                break;
            case MoneyEnum.S:
                if (tvMoneyS != null)
                {
                    tvMoneyS.text = price + "";
                    tvMoneyS.transform.localScale = new Vector3(1, 1, 1);
                }
                break;
        }
    }

    /// <summary>
    /// 设置美观值
    /// </summary>
    public void SetInnAesthetics(InnAttributesBean innAttributes)
    {
        innAttributes.GetAesthetics(out float maxAesthetics, out float aesthetics);
        if (popupAesthetics != null)
        {
            popupAesthetics.SetContent(GameCommonInfo.GetUITextById(2003) + ":" + aesthetics + "/" + maxAesthetics);
        }
        if (proAesthetics != null)
        {
            proAesthetics.SetData(maxAesthetics, aesthetics);
        }
    }

    /// <summary>
    /// 设置菜品丰富度
    /// </summary>
    /// <param name="innAttributes"></param>
    public void SetInnRichNess(InnAttributesBean innAttributes)
    {
        innAttributes.GetRichness(out int maxRichness, out int richness);
        if (popupRichness != null)
        {
            popupRichness.SetContent(GameCommonInfo.GetUITextById(2005) + ":" + richness + "/" + maxRichness);
        }
        if (proRichness != null)
        {
            proRichness.SetData(maxRichness, richness);
        }
    }

    /// <summary>
    /// 设置客栈点赞
    /// </summary>
    /// <param name="innAttributes"></param>
    public void SetInnPraise(InnAttributesBean innAttributes)
    {
        innAttributes.GetPraise(out int maxPraise, out int praise);
        if (popupPraise != null)
        {
            popupPraise.SetContent(GameCommonInfo.GetUITextById(2004) + " " + (System.Math.Round((float)praise / maxPraise, 4) * 100) + "%");
        }
        if (proPraise != null)
        {
            proPraise.SetData(maxPraise, praise);
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
            Sprite spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("inn_level_" + innLevelTitle + "_" + (innLevelStar - 1));
            if (spIcon)
            {
                ivInnLevel.gameObject.SetActive(true);
                ivInnLevel.sprite = spIcon;
            }
            else
                ivInnLevel.gameObject.SetActive(false);
        }
    }

    public void SaveData()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameDataHandler.Instance.manager.SaveGameData(InnHandler.Instance.GetInnRecord());
    }

    public void OpenBuildUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (CheckUtil.ListIsNull(InnHandler.Instance.rascalrQueue))
        {
            DialogBean dialogBean = new DialogBean();
            dialogBean.content = GameCommonInfo.GetUITextById(3007);
            dialogBean.dialogPosition = 1;
            DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogBean);
        }
        else
        {
            ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1016));
        }
    }

    public void OpenWorkerUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIGameWorker uiGameWorker = UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameWorker>(UIEnum.GameWorker);
        uiGameWorker.InitUI();
    }

    public void OpenMenuUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMenu>(UIEnum.GameMenu);
    }

    public void OpenBackpackUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameBackpack>(UIEnum.GameBackpack);
    }

    public void OpenFavorabilityUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameFavorability>(UIEnum.GameFavorability);
    }

    public void OpenStatisticsUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameStatistics>(UIEnum.GameStatistics);
    }

    public void OpenSettingUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameSetting>(UIEnum.GameSetting);
    }

    public void OpenHelpUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameHelp>(UIEnum.GameHelp);
    }

    public void OnClickForEndDay()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogBean = new DialogBean();
        dialogBean.content = GameCommonInfo.GetUITextById(3004);
        dialogBean.dialogPosition = 0;
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogBean);
    }

    public void OnClickForJumpTime()
    {
        if (GameTimeHandler.Instance.GetDayStatus() != GameTimeHandler.DayEnum.Rest)
        {
            return;
        }
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogBean = new DialogBean();
        JumpTimeDialogView jumpTimeDialog = DialogHandler.Instance.CreateDialog<JumpTimeDialogView>(DialogEnum.JumpTime, this, dialogBean);
        jumpTimeDialog.SetData();
    }

    public void OnClickForHotel()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameHotel>(UIEnum.GameHotel);
    }

    public void OnClickForFamily()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameFamily>(UIEnum.GameFamily);
    }

    /// <summary>
    /// 点击第一层
    /// </summary>
    public void OnClickForFirstLayer()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        SetInnLayer(1);
    }

    /// <summary>
    /// 点击第二层
    /// </summary>
    public void OnClickForSecondLayer()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        SetInnLayer(2);
    }

    /// <summary>
    /// 设置层数
    /// </summary>
    /// <param name="layer"></param>
    public void SetInnLayer(int layer)
    {
        ControlForWorkCpt controlForWork = (ControlForWorkCpt)GameControlHandler.Instance.manager.GetControl();
        controlForWork.SetLayer(layer);
    }

    /// <summary>
    /// 增加金钱动画
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    private void AnimForAddMoney(long priceL, long priceM, long priceS)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (priceL != 0)
        {
            if (tweenForMoneyL != null)
                tweenForMoneyL.Kill();
            long startMoney = gameData.moneyL - priceL;
            tweenForMoneyL = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.L, x); }, gameData.moneyL, 1);
            tvMoneyL.transform.localScale = new Vector3(1, 1, 1);
            tvMoneyL.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1f, 10, 1);
            AnimForMoneyItem(MoneyEnum.L, priceL);
        }
        if (priceM != 0)
        {
            if (tweenForMoneyM != null)
                tweenForMoneyM.Kill();
            long startMoney = gameData.moneyM - priceM;
            tweenForMoneyM = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.M, x); }, gameData.moneyM, 1);
            tvMoneyM.transform.localScale = new Vector3(1, 1, 1);
            tvMoneyM.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1f, 10, 1);
            AnimForMoneyItem(MoneyEnum.M, priceM);
        }
        if (priceS != 0)
        {
            if (tweenForMoneyS != null)
                tweenForMoneyS.Kill();
            long startMoney = gameData.moneyS - priceS;
            tweenForMoneyS = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.S, x); }, gameData.moneyS, 1);
            tvMoneyS.transform.localScale = new Vector3(1, 1, 1);
            tvMoneyS.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1f, 10, 1);
            AnimForMoneyItem(MoneyEnum.S, priceS);
        }
    }

    private void AnimForMoneyItem(MoneyEnum moneyType, long money)
    {
        Vector3 startPosition = Vector3.zero;
        Color tvColor = Color.black;
        switch (moneyType)
        {
            case MoneyEnum.L:
                startPosition = tvMoneyL.transform.position;
                tvColor = tvMoneyL.color;
                break;
            case MoneyEnum.M:
                startPosition = tvMoneyM.transform.position;
                tvColor = tvMoneyM.color;
                break;
            case MoneyEnum.S:
                startPosition = tvMoneyS.transform.position;
                tvColor = tvMoneyS.color;
                break;
        }
        GameObject itemMoney = Instantiate(gameObject, tvMoneyForAnimModel.gameObject, startPosition);
        Text tvItem = itemMoney.GetComponent<Text>();
        tvItem.DOFade(0, 1).SetDelay(1);
        tvItem.color = tvColor;
        tvItem.text = money + "";
        RectTransform rtItem = ((RectTransform)itemMoney.transform);
        rtItem.DOAnchorPosY(rtItem.anchoredPosition.y + 30, 2).OnComplete(delegate () { Destroy(itemMoney); });
    }

    protected void EndDay()
    {
        //结束一天
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals(EnumUtil.GetEnumName(ScenesEnum.GameInnScene)))
        {
            GameScenesHandler.Instance.manager.GetSceneInit<SceneGameInnInit>().CleanInnData();
        }
        else
        {
            GameScenesHandler.Instance.manager.GetSceneInit<BaseNormalSceneInit>().EndDay();
        }
    }

    #region dialog 回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        if (dialogView as JumpTimeDialogView)
        {

        }
        else
        {
            if (dialogData.dialogPosition == 0)
            {
                EndDay();
            }
            else if (dialogData.dialogPosition == 1)
            {
                //打开建筑
                UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameBuild>(UIEnum.GameBuild);
            }
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion

    #region 单选回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        int timeScale = 1;
        if (rbview == rbTimeScale2)
        {
            timeScale = 2;
        }
        else if (rbview == rbTimeScale5)
        {
            timeScale = 5;
        }
        else if (rbview == rbTimeScale8)
        {
            timeScale = 8;
        }
        else if (rbview == rbTimeScaleStop)
        {
            timeScale = 0;
        }
        if (rbview.status == RadioButtonView.RadioButtonStatus.Selected)
        {
            GameTimeHandler.Instance.SetTimeScale(timeScale);
        }
        else
        {
            GameTimeHandler.Instance.SetTimeScale(1);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion

    #region 通知回调

    public void NotifyForData(GameDataHandler.NotifyTypeEnum notifyType,params object[] obj)
    {
        if (notifyType == GameDataHandler.NotifyTypeEnum.AddMoney)
        {
            long priceL = System.Convert.ToInt64(obj[0]);
            long priceM = System.Convert.ToInt64(obj[1]);
            long priceS = System.Convert.ToInt64(obj[2]);
            AnimForAddMoney(priceL, priceM, priceS);
        }
        else if (notifyType == GameDataHandler.NotifyTypeEnum.MenuResearchChange)
        {
            List<MenuOwnBean> listMenu = (List<MenuOwnBean>)obj[0];
            uiHint.SetData(listMenu);
        }
        else if (notifyType == GameDataHandler.NotifyTypeEnum.BedResearchChange)
        {
            List<BuildBedBean> listBed = (List<BuildBedBean>)obj[0];
            uiHint.SetData(listBed);
        }
        else if (notifyType == GameDataHandler.NotifyTypeEnum.InfiniteTowerProChange)
        {
            List<UserInfiniteTowersBean> listData = (List<UserInfiniteTowersBean>)obj[0];
            uiHint.SetData(listData);
        }
    }
    #endregion
}