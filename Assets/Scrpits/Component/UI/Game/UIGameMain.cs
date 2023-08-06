using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
public partial class UIGameMain : BaseUIComponent, DialogView.IDialogCallBack, IRadioGroupCallBack
{
    [Header("控件")]
    public UIPopupPromptButton popupWorker;
    public UIPopupPromptButton popupBuild;
    public UIPopupPromptButton popupMenu;
    public UIPopupPromptButton popupBackpack;
    public UIPopupPromptButton popupFavorability;
    public UIPopupPromptButton popupDebug;
    public UIPopupPromptButton popupInnData;
    public UIPopupPromptButton popupHelp;
    public UIPopupPromptButton popupSetting;
    public UIPopupPromptButton popupJumpTime;
    public UIPopupPromptButton popupHotel;
    public UIPopupPromptButton popupFamily;
    public UIPopupPromptButton popupCourtyard;

    public Text tvMoneyS;
    public Text tvMoneyM;
    public Text tvMoneyL;

    public UIPopupPromptButton popupAesthetics;
    public UIPopupPromptButton popupPraise;
    public UIPopupPromptButton popupRichness;
    public ProgressView proAesthetics;
    public ProgressView proPraise;
    public ProgressView proRichness;

    public UIPopupPromptButton popupInnLevel;
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

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_ItemGameMainFeaturesItem_Worker)
        {
            OpenWorkerUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Build)
        {
            OpenBuildUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Menu)
        {
            OpenMenuUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Bag)
        {
            OpenBackpackUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Favorability)
        {
            OpenFavorabilityUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Statistics)
        {
            OpenStatisticsUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Setting)
        {
            OpenSettingUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Help)
        {
            OpenHelpUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Debug)
        {
            OpenTestUI();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Hotel)
        {
            OnClickForHotel();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Family)
        {
            OnClickForFamily();
        }
        else if (viewButton == ui_ItemGameMainFeaturesItem_Courtyard)
        {
            OnClickForCourtyard();
        }
        else if (viewButton == ui_Day)
        {
            OnClickForJumpTime();
        }
        else if (viewButton == ui_Sleep)
        {
            OnClickForEndDay();
        }
    }

    public void Start()
    {
        if (rgTimeScale != null)
            rgTimeScale.SetCallBack(this);

        if (btLayerFirstLayer != null)
            btLayerFirstLayer.onClick.AddListener(OnClickForFirstLayer);
        if (btLayerSecondLayer != null)
            btLayerSecondLayer.onClick.AddListener(OnClickForSecondLayer);

        InitInnData();

        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //判断是否展示教程
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        if (!userAchievement.isOpenedHelp)
        {
            UIHandler.Instance.OpenUIAndCloseOther<UIGameHelp>();
        }
        RefreshUI();
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

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        SetMoney(MoneyEnum.L, gameData.moneyL);
        SetMoney(MoneyEnum.M, gameData.moneyM);
        SetMoney(MoneyEnum.S, gameData.moneyS);
        GameUtil.RefreshRectTransform(ui_Features);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        //时间加速回归正常

        GameTimeHandler.Instance.SetTimeScale(1);
        rgTimeScale.SetPosition(-1, false);
        GameControlHandler.Instance.StopControl();
        uiHint.CloseUI();
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
            popupWorker.SetContent(TextHandler.Instance.manager.GetTextById(2031));
        if (popupBuild != null)
            popupBuild.SetContent(TextHandler.Instance.manager.GetTextById(2032));
        if (popupMenu != null)
            popupMenu.SetContent(TextHandler.Instance.manager.GetTextById(2033));
        if (popupBackpack != null)
            popupBackpack.SetContent(TextHandler.Instance.manager.GetTextById(2034));
        if (popupFavorability != null)
            popupFavorability.SetContent(TextHandler.Instance.manager.GetTextById(2035));
        if (popupDebug != null)
            popupDebug.SetContent(TextHandler.Instance.manager.GetTextById(2036));
        if (popupInnData != null)
            popupInnData.SetContent(TextHandler.Instance.manager.GetTextById(2037));
        if (popupSetting != null)
            popupSetting.SetContent(TextHandler.Instance.manager.GetTextById(2038));
        if (popupHelp != null)
            popupHelp.SetContent(TextHandler.Instance.manager.GetTextById(2039));
        if (popupJumpTime != null)
            popupJumpTime.SetContent(TextHandler.Instance.manager.GetTextById(2040));
        if (popupHotel != null)
            popupHotel.SetContent(TextHandler.Instance.manager.GetTextById(2041));
        if (popupFamily != null)
            popupFamily.SetContent(TextHandler.Instance.manager.GetTextById(2042));
        if (popupCourtyard != null)
            popupCourtyard.SetContent(TextHandler.Instance.manager.GetTextById(2043));
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
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (gameConfig.statusForWorkerNumber == 0)
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
            ui_ItemGameMainFeaturesItem_Hotel.ShowObj(true);
        }
        else
        {
            ui_ItemGameMainFeaturesItem_Hotel.ShowObj(false);
        }
        //是否展示建造按钮
        if (SceneUtil.GetCurrentScene() == ScenesEnum.GameInnScene)
        {
            ui_ItemGameMainFeaturesItem_Build.ShowObj(true);
        }
        else
        {
            ui_ItemGameMainFeaturesItem_Build.ShowObj(false);
        }
        //是否展示家族按钮
        if (gameData.GetFamilyData().CheckMarry(gameData.gameTime))
        {
            ui_ItemGameMainFeaturesItem_Family.ShowObj(true);
        }
        else
        {
            ui_ItemGameMainFeaturesItem_Family.ShowObj(false);
        }
        //是否展示测试按钮
        if (Application.platform ==  RuntimePlatform.WindowsEditor)
        {
            ui_ItemGameMainFeaturesItem_Debug.gameObject.ShowObj(true);
        }
        else
        {
            ui_ItemGameMainFeaturesItem_Debug.gameObject.ShowObj(false);
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
            popupAesthetics.SetContent(TextHandler.Instance.manager.GetTextById(2003) + ":" + aesthetics + "/" + maxAesthetics);
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
            popupRichness.SetContent(TextHandler.Instance.manager.GetTextById(2005) + ":" + richness + "/" + maxRichness);
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
            popupPraise.SetContent(TextHandler.Instance.manager.GetTextById(2004) + " " + (System.Math.Round((float)praise / maxPraise, 4) * 100) + "%");
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
            popupInnLevel.SetContent(TextHandler.Instance.manager.GetTextById(2006) + " " + innLevelStr);
        }

        if (ivInnLevel != null)
        {
            if (innLevelTitle == 0)
            {
                ivInnLevel.gameObject.SetActive(false);
                return;
            }
            Sprite spIcon = IconHandler.Instance.GetIconSpriteByName("inn_level_" + innLevelTitle + "_" + (innLevelStar - 1));
            if (spIcon)
            {
                ivInnLevel.gameObject.SetActive(true);
                ivInnLevel.sprite = spIcon;
            }
            else
                ivInnLevel.gameObject.SetActive(false);
        }
    }

    public void OpenBuildUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (InnHandler.Instance.rascalrQueue.IsNull())
        {
            DialogBean dialogData = new DialogBean();
            dialogData.content = TextHandler.Instance.manager.GetTextById(3007);
            dialogData.dialogPosition = 1;
            dialogData.dialogType = DialogEnum.Normal;
            dialogData.callBack = this;
            UIHandler.Instance.ShowDialog<DialogView>(dialogData);
        }
        else
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1016));
        }
    }

    public void OnClickForCourtyard()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        ScenesEnum scenes = SceneUtil.GetCurrentScene();
        if(scenes == ScenesEnum.GameCourtyardScene)
        {
            UIHandler.Instance.OpenUIAndCloseOther<UIGameBuildCourtyard>();
        }
    }

    public void OpenWorkerUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIGameWorker uiGameWorker = UIHandler.Instance.OpenUIAndCloseOther<UIGameWorker>();
        uiGameWorker.InitUI();
    }

    public void OpenMenuUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMenu>();
    }

    public void OpenBackpackUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameBackpack>();
    }

    public void OpenFavorabilityUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameFavorability>();
    }

    public void OpenStatisticsUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameStatistics>();
    }

    public void OpenSettingUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameSetting>();
    }

    public void OpenHelpUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameHelp>();
    }

    public void OpenTestUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameTest>();
    }

    public void OnClickForEndDay()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.content = TextHandler.Instance.manager.GetTextById(3004);
        dialogData.dialogPosition = 0;
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    public void OnClickForJumpTime()
    {
        if (GameTimeHandler.Instance.GetDayStatus() != GameTimeHandler.DayEnum.Rest)
        {
            return;
        }
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.dialogType = DialogEnum.JumpTime;
        dialogData.callBack = this;
        JumpTimeDialogView jumpTimeDialog = UIHandler.Instance.ShowDialog<JumpTimeDialogView>(dialogData);
        jumpTimeDialog.SetData();
    }

    public void OnClickForHotel()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameHotel>();
    }

    public void OnClickForFamily()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameFamily>();
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
    private void AnimForAddMoney(long priceL, long priceM, long priceS,Vector3 startPosition)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (priceL != 0)
        {
            if (tweenForMoneyL != null)
                tweenForMoneyL.Kill();
            long startMoney = gameData.moneyL - priceL;
            tweenForMoneyL = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.L, x); }, gameData.moneyL, 3);
            //tvMoneyL.transform.localScale = new Vector3(1, 1, 1);
            //tvMoneyL.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1f, 10, 1);
            AnimForMoneyItem(startPosition,MoneyEnum.L, priceL);
        }
        if (priceM != 0)
        {
            if (tweenForMoneyM != null)
                tweenForMoneyM.Kill();
            long startMoney = gameData.moneyM - priceM;
            tweenForMoneyM = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.M, x); }, gameData.moneyM, 3);
            //tvMoneyM.transform.localScale = new Vector3(1, 1, 1);
            //tvMoneyM.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1f, 10, 1);
            AnimForMoneyItem(startPosition,MoneyEnum.M, priceM);
        }
        if (priceS != 0)
        {
            if (tweenForMoneyS != null)
                tweenForMoneyS.Kill();
            long startMoney = gameData.moneyS - priceS;
            tweenForMoneyS = DOTween.To(() => startMoney, x => { SetMoney(MoneyEnum.S, x); }, gameData.moneyS, 3);
            //tvMoneyS.transform.localScale = new Vector3(1, 1, 1);
            //tvMoneyS.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1f, 10, 1);
            AnimForMoneyItem(startPosition,MoneyEnum.S, priceS);
        }
    }

    private void AnimForMoneyItem(Vector3 addPosition, MoneyEnum moneyType, long money)
    {
        Vector2 startPosition = GameUtil.WorldPointToUILocalPoint((RectTransform)transform, addPosition);
        Vector3 endPosition = Vector3.zero;
        Color tvColor = Color.black;

        GameObject itemMoney = Instantiate(gameObject, tvMoneyForAnimModel.gameObject);
        ((RectTransform)itemMoney.transform).anchoredPosition = startPosition;
        switch (moneyType)
        {
            case MoneyEnum.L:
                endPosition = UGUIUtil.GetUIRootPosForIcon((RectTransform)transform, (RectTransform)tvMoneyL.transform);
                tvColor = tvMoneyL.color;
                break;
            case MoneyEnum.M:
                endPosition = UGUIUtil.GetUIRootPosForIcon((RectTransform)transform, (RectTransform)tvMoneyM.transform);
                tvColor = tvMoneyM.color;
                break;
            case MoneyEnum.S:
                endPosition = UGUIUtil.GetUIRootPosForIcon((RectTransform)transform, (RectTransform)tvMoneyS.transform);
                tvColor = tvMoneyS.color;
                break;
        }
        Text tvItem = itemMoney.GetComponent<Text>();
        //tvItem.DOFade(0, 1).SetDelay(1);
        tvItem.color = tvColor;
        tvItem.text = money + "";
        RectTransform rtItem = ((RectTransform)itemMoney.transform);

        rtItem.DOAnchorPos(endPosition,3).OnComplete(delegate () { Destroy(itemMoney); });
    }


    protected void EndDay()
    {
        //结束一天
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals(ScenesEnum.GameInnScene.GetEnumName()))
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
                UIHandler.Instance.OpenUIAndCloseOther<UIGameBuild>();
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
        if (rbview.isSelect == true)
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

    public void NotifyForData(GameDataHandler.NotifyTypeEnum notifyType, params object[] obj)
    {
        if (notifyType == GameDataHandler.NotifyTypeEnum.AddMoney)
        {
            long priceL = System.Convert.ToInt64(obj[0]);
            long priceM = System.Convert.ToInt64(obj[1]);
            long priceS = System.Convert.ToInt64(obj[2]);
            Vector3 position = (Vector3)(obj[3]);
            AnimForAddMoney(priceL, priceM, priceS, position);
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