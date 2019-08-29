using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameMain : BaseUIComponent, DialogView.IDialogCallBack
{
    [Header("控件")]
    public Button btWorker;
    public Button btBuild;
    public Button btMenu;
    public Button btBackpack;
    public Button btSave;
    public Button btSleep;

    public Text tvInnStatus;
    public Text tvMoneyS;
    public Text tvMoneyM;
    public Text tvMoneyL;

    public InfoPromptPopupButton popupAesthetics;
    public Text tvAesthetics;
    public InfoPromptPopupButton popupPraise;
    public Text tvPraise;
    public InfoPromptPopupButton popupRichness;
    public Text tvRichness;
    public InfoPromptPopupButton popupInnLevel;
    public Image ivInnLevel;

    public ClockView clockView;//时钟

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

        if (btSave != null)
            btSave.onClick.AddListener(SaveData);

        if (btSleep != null)
            btSleep.onClick.AddListener(EndDay);

        InfoPromptPopupShow infoPromptPopup = GetUIMananger<UIGameManager>().infoPromptPopup;
        if (popupAesthetics != null)
            popupAesthetics.SetPopupShowView(infoPromptPopup);
        if (popupPraise != null)
            popupPraise.SetPopupShowView(infoPromptPopup);
        if (popupRichness != null)
            popupRichness.SetPopupShowView(infoPromptPopup);
        if (popupInnLevel != null)
            popupInnLevel.SetPopupShowView(infoPromptPopup);
        InitInnData();
    }

    private void Update()
    {
        InnHandler innHandler = GetUIMananger<UIGameManager>().innHandler;
        GameTimeHandler gameTimeHandler = GetUIMananger<UIGameManager>().gameTimeHandler;
        if (tvInnStatus != null && innHandler != null)
            if (innHandler.innStatus == InnHandler.InnStatusEnum.Close)
            {
                tvInnStatus.text = GameCommonInfo.GetUITextById(2002);
            }
            else
            {
                tvInnStatus.text = GameCommonInfo.GetUITextById(2001);
            }
        if (tvMoneyS != null)
            tvMoneyS.text = GetUIMananger<UIGameManager>().gameDataManager.gameData.moneyS + "";
        if (tvMoneyM != null)
            tvMoneyM.text = GetUIMananger<UIGameManager>().gameDataManager.gameData.moneyM + "";
        if (tvMoneyL != null)
            tvMoneyL.text = GetUIMananger<UIGameManager>().gameDataManager.gameData.moneyL + "";
        if (clockView!=null&& gameTimeHandler!=null)
        {
            gameTimeHandler.GetTime(out float hour,out float min);
            clockView.SetTime((int)hour, (int)min);
        }  
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitInnData();
    }

    /// <summary>
    /// 初始化客栈数据
    /// </summary>
    public void InitInnData()
    {
        InnAttributesBean innAttributes = GetUIMananger<UIGameManager>().gameDataManager.gameData.GetInnAttributesData();
        if (innAttributes == null)
            return;
        if (popupAesthetics != null)
            popupAesthetics.SetContent(GameCommonInfo.GetUITextById(2003) + " " + innAttributes.aesthetics);
        if (tvAesthetics != null)
            tvAesthetics.text = innAttributes.aesthetics + "";
        if (popupPraise != null)
            popupPraise.SetContent(GameCommonInfo.GetUITextById(2004) + " " + innAttributes.praise);
        if (tvPraise != null)
            tvPraise.text = innAttributes.praise + "";
        if (popupRichness != null)
            popupRichness.SetContent(GameCommonInfo.GetUITextById(2005) + " " + innAttributes.richness);
        if (tvRichness != null)
            tvRichness.text = innAttributes.richness + "";

        string innLevelStr = GetUIMananger<UIGameManager>().gameDataManager.gameData.GetInnLevel(out int innLevelTitle, out int innLevelStar);
        if (popupInnLevel != null)
        {
            popupInnLevel.SetContent(GameCommonInfo.GetUITextById(2006) + " " + innLevelStr);
        }

        if (ivInnLevel != null)
        {
            Sprite spIcon = GetUIMananger<UIGameManager>().gameItemsManager.GetItemsSpriteByName("inn_level_" + innLevelTitle + "_" + (innLevelStar - 1));
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
        GetUIMananger<UIGameManager>().gameDataManager.SaveGameData();
    }

    public void OpenBuildUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameBuild));
    }

    public void OpenWorkerUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
    }

    public void OpenMenuUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMenu));
    }

    public void OpenBackpackUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameBackpack));
    }

    public void EndDay()
    {
        DialogBean dialogBean = new DialogBean();
        dialogBean.content = GameCommonInfo.GetUITextById(3004);
        GetUIMananger<UIGameManager>().dialogManager.CreateDialog(0, this, dialogBean);
    }

    #region dialog 回调
    public void Submit(DialogView dialogView)
    {
        InnHandler innHandler = GetUIMananger<UIGameManager>().innHandler;
        GameTimeHandler gameTimeHandler=  GetUIMananger<UIGameManager>().gameTimeHandler;

        GetUIMananger<UIGameManager>().gameTimeHandler.isStopTime = true;
        if (gameTimeHandler.dayStauts == GameTimeHandler.DayEnum.Work)
        {
            //如果是工作状态结束一天 则进入结算画面
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameSettle));
        }
        else if(gameTimeHandler.dayStauts == GameTimeHandler.DayEnum.Rest)
        {
            //如果是休息状态结束一天 则直接进入下一天画面
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
        }

        //关闭店面
        if (innHandler != null)
            innHandler.CloseInn();
    }

    public void Cancel(DialogView dialogView)
    {
    }
    #endregion
}