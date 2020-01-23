using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameMain : BaseUIComponent, DialogView.IDialogCallBack
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

    protected AudioHandler audioHandler;
    protected InnHandler innHandler;
    protected GameTimeHandler gameTimeHandler;
    protected GameDataManager gameDataManager;
    protected InfoPromptPopupShow infoPromptPopup;
    protected DialogManager dialogManager;
    protected ToastManager toastManager;
    protected GameItemsManager gameItemsManager;
    private void Awake()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        audioHandler = uiGameManager.audioHandler;
        innHandler = uiGameManager.innHandler;
        gameTimeHandler = uiGameManager.gameTimeHandler;
        gameDataManager = uiGameManager.gameDataManager;
        infoPromptPopup = uiGameManager.infoPromptPopup;
        dialogManager = uiGameManager.dialogManager;
        toastManager = uiGameManager.toastManager;
        gameItemsManager = uiGameManager.gameItemsManager;
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

        if (btSave != null)
            btSave.onClick.AddListener(SaveData);

        if (btSleep != null)
            btSleep.onClick.AddListener(EndDay);

        if (popupWorker != null)
            popupWorker.SetPopupShowView(infoPromptPopup);
        if (popupBuild != null)
            popupBuild.SetPopupShowView(infoPromptPopup);
        if (popupMenu != null)
            popupMenu.SetPopupShowView(infoPromptPopup);
        if (popupBackpack != null)
            popupBackpack.SetPopupShowView(infoPromptPopup);
        if (popupFavorability != null)
            popupFavorability.SetPopupShowView(infoPromptPopup);
        if (popupSave != null)
            popupSave.SetPopupShowView(infoPromptPopup);

        if (popupAesthetics != null)
            popupAesthetics.SetPopupShowView(infoPromptPopup);
        if (popupPraise != null)
            popupPraise.SetPopupShowView(infoPromptPopup);
        if (popupRichness != null)
            popupRichness.SetPopupShowView(infoPromptPopup);
        if (popupInnLevel != null)
            popupInnLevel.SetPopupShowView(infoPromptPopup);
        //设置美观值
        if (tvAesthetics != null)
            tvAesthetics.text = gameDataManager.gameData.GetInnAttributesData().GetAesthetics() + "";
        InitInnData();
    }

    private void Update()
    {
        InnAttributesBean innAttributes = gameDataManager.gameData.GetInnAttributesData();
        if (tvInnStatus != null && innHandler != null)
            if (innHandler.GetInnStatus() == InnHandler.InnStatusEnum.Close)
            {
                tvInnStatus.text = GameCommonInfo.GetUITextById(2002);
            }
            else
            {
                tvInnStatus.text = GameCommonInfo.GetUITextById(2001);
            }
        if (tvMoneyS != null)
            tvMoneyS.text = gameDataManager.gameData.moneyS + "";
        if (tvMoneyM != null)
            tvMoneyM.text = gameDataManager.gameData.moneyM + "";
        if (tvMoneyL != null)
            tvMoneyL.text = gameDataManager.gameData.moneyL + "";
        if (clockView != null && gameTimeHandler != null)
        {
            gameTimeHandler.GetTime(out float hour, out float min);
            gameTimeHandler.GetTime(out int year, out int month, out int day);
            clockView.SetTime(month, day, (int)hour, (int)min);
        }
        if (sliderPraise != null)
        {
            sliderPraise.value = innAttributes.GetPraise() / 100f;
        }
        if (tvPraise != null)
        {
            tvPraise.text = innAttributes.GetPraise() + "%";
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
        InnAttributesBean innAttributes = gameDataManager.gameData.GetInnAttributesData();
        if (innAttributes == null)
            return;

        if (popupWorker != null)
            popupWorker.SetContent(GameCommonInfo.GetUITextById(2003));
        if (popupBuild != null)
            popupBuild.SetContent(GameCommonInfo.GetUITextById(2003));
        if (popupMenu != null)
            popupMenu.SetContent(GameCommonInfo.GetUITextById(2003));
        if (popupBackpack != null)
            popupBackpack.SetContent(GameCommonInfo.GetUITextById(2003));
        if (popupFavorability != null)
            popupFavorability.SetContent(GameCommonInfo.GetUITextById(2003));
        if (popupSave != null)
            popupSave.SetContent(GameCommonInfo.GetUITextById(2003));

        if (popupAesthetics != null)
            popupAesthetics.SetContent(GameCommonInfo.GetUITextById(2003) + " " + innAttributes.aesthetics);
        if (tvAesthetics != null)
            tvAesthetics.text = innAttributes.aesthetics + "";
        if (popupPraise != null)
            popupPraise.SetContent(GameCommonInfo.GetUITextById(2004) + " " + innAttributes.praise + "%");
        if (popupRichness != null)
            popupRichness.SetContent(GameCommonInfo.GetUITextById(2005) + " " + innAttributes.richness);
        if (tvRichness != null)
            tvRichness.text = innAttributes.richness + "";

        string innLevelStr = gameDataManager.gameData.innAttributes.GetInnLevel(out int innLevelTitle, out int innLevelStar);
        if (popupInnLevel != null)
        {
            popupInnLevel.SetContent(GameCommonInfo.GetUITextById(2006) + " " + innLevelStr);
        }

        if (ivInnLevel != null)
        {
            Sprite spIcon = gameItemsManager.GetItemsSpriteByName("inn_level_" + innLevelTitle + "_" + (innLevelStar - 1));
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
        GetUIManager<UIGameManager>().gameDataManager.SaveGameData();
    }

    public void OpenBuildUI()
    {
        InnHandler innHandler = GetUIManager<UIGameManager>().innHandler;

        if (CheckUtil.ListIsNull(innHandler.rascalrQueue))
        {
            DialogBean dialogBean = new DialogBean();
            dialogBean.content = GameCommonInfo.GetUITextById(3007);
            dialogBean.dialogPosition = 1;
            dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
        }
        else
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1016));
        }
    }

    public void OpenWorkerUI()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
    }

    public void OpenMenuUI()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMenu));
    }

    public void OpenBackpackUI()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameBackpack));
    }

    public void OpenFavorabilityUI()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameFavorability));
    }

    public void EndDay()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        DialogBean dialogBean = new DialogBean();
        dialogBean.content = GameCommonInfo.GetUITextById(3004);
        dialogBean.dialogPosition = 0;
        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
    }

    #region dialog 回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        if (dialogData.dialogPosition == 0)
        {
            //结束一天
            BaseSceneInit sceneInit = GetUIManager<UIGameManager>().sceneInit;
            Scene scene = SceneManager.GetActiveScene();
            //如果是客栈场景
            if (EnumUtil.GetEnumName(ScenesEnum.GameInnScene).Equals(scene.name))
            {
                ((SceneGameInnInit)sceneInit).EndDay();
            }
            //如果是城镇 则先回到客栈
            else if (EnumUtil.GetEnumName(ScenesEnum.GameTownScene).Equals(scene.name))
            {
                ((SceneGameTownInit)sceneInit).EndDay();
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
}