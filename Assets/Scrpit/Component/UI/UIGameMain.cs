using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameMain : BaseUIComponent, DialogView.IDialogCallBack
{
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


    public GameTimeHandler gameTimeHandler;
    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;
    public DialogManager dialogManager;
    public InnHandler innHandler;
    public InnWallBuilder innWall;

    private void Update()
    {
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
            tvMoneyS.text = gameDataManager.gameData.moneyS + "";
        if (tvMoneyM != null)
            tvMoneyM.text = gameDataManager.gameData.moneyM + "";
        if (tvMoneyL != null)
            tvMoneyL.text = gameDataManager.gameData.moneyL + "";
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

        string innLevelStr= gameDataManager.gameData.GetInnLevel(out int innLevelTitle,out int innLevelStar);
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

        InitInnData();
    }

    public void SaveData()
    {
        gameDataManager.SaveGameData();
    }

    public void OpenBuildUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Build");
    }

    public void OpenWorkerUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Worker");
    }

    public void OpenMenuUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Menu");
    }

    public void OpenBackpackUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Backpack");
    }

    public void EndDay()
    {
        DialogBean dialogBean = new DialogBean();
        dialogBean.content = "是否要结束今天？";
        dialogManager.CreateDialog(0, this, dialogBean);
    }

    #region dialog 回调
    public void Submit(DialogView dialogView)
    {
        gameTimeHandler.isStopTime = true;
        uiManager.OpenUIAndCloseOtherByName("Settle");
        innHandler.CloseInn();
    }

    public void Cancel(DialogView dialogView)
    {
    }
    #endregion
}