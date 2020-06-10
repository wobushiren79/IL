using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameSettle : UIGameComponent
{
    public Button btSubmit;

    //收入支出
    public Text tvIncomeS;
    public Text tvIncomeM;
    public Text tvIncomeL;
    public Text tvExpensesS;
    public Text tvExpensesM;
    public Text tvExpensesL;

    public GameObject objListRecordContent;

    public GameObject objItemModelForMoney;
    public GameObject objItemModelForOther;

    public Sprite spIconOilsalt;
    public Sprite spIconMeat;
    public Sprite spIconRiverfresh;
    public Sprite spIconSeafood;
    public Sprite spIconVegetables;
    public Sprite spIconMelonfruit;
    public Sprite spIconWaterwine;
    public Sprite spIconFlour;

    private float animDelay;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OpenDateUI);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public void InitData()
    {
        InnHandler innHandler = uiGameManager.innHandler;
        InnFoodManager innFoodManager = uiGameManager.innFoodManager;
        IconDataManager iconDataManager = uiGameManager.iconDataManager;
        GameTimeHandler gameTimeHandler = uiGameManager.gameTimeHandler;
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        //停止时间
        gameTimeHandler.SetTimeStatus(true);
        CptUtil.RemoveChildsByActive(objListRecordContent.transform);
        animDelay = 0f;
        InnRecordBean innRecord = innHandler.GetInnRecord();
        //员工支出
        CreateItemForMoney(
                iconDataManager.GetIconSpriteByName("money_1"),
                GameCommonInfo.GetUITextById(183),
                0,
                innRecord.payWageL,
                innRecord.payWageM,
                innRecord.payWageS);
        //借贷还款
        if (gameDataManager.gameData.listLoans.Count > 0)
        {
            gameDataManager.gameData.PayLoans(out List<UserLoansBean> listPayLoans);
            foreach (UserLoansBean itemPayLoans in listPayLoans)
            {
                CreateItemForMoney(
                    iconDataManager.GetIconSpriteByName("money_1"),
                    GameCommonInfo.GetUITextById(184),
                    0,
                    0,
                    0,
                    itemPayLoans.moneySForDay);
                innRecord.AddPayLoans(0,0, itemPayLoans.moneySForDay);
            }
        }
        //食材消耗
        string consumeIngStr = GameCommonInfo.GetUITextById(4002);
        if (innRecord.consumeIngOilsalt > 0)
            CreateItemForOther(innRecord.consumeIngOilsalt, spIconOilsalt, consumeIngStr + " " + GameCommonInfo.GetUITextById(21));
        if (innRecord.consumeIngMeat > 0)
            CreateItemForOther(innRecord.consumeIngMeat, spIconMeat, consumeIngStr + " " + GameCommonInfo.GetUITextById(22));
        if (innRecord.consumeIngRiverfresh > 0)
            CreateItemForOther(innRecord.consumeIngRiverfresh, spIconRiverfresh, consumeIngStr + " " + GameCommonInfo.GetUITextById(23));
        if (innRecord.consumeIngSeafood > 0)
            CreateItemForOther(innRecord.consumeIngSeafood, spIconSeafood, consumeIngStr + " " + GameCommonInfo.GetUITextById(24));
        if (innRecord.consumeIngVegetables > 0)
            CreateItemForOther(innRecord.consumeIngVegetables, spIconVegetables, consumeIngStr + " " + GameCommonInfo.GetUITextById(25));
        if (innRecord.consumeIngMelonfruit > 0)
            CreateItemForOther(innRecord.consumeIngMelonfruit, spIconMelonfruit, consumeIngStr + " " + GameCommonInfo.GetUITextById(26));
        if (innRecord.consumeIngWaterwine > 0)
            CreateItemForOther(innRecord.consumeIngWaterwine, spIconWaterwine, consumeIngStr + " " + GameCommonInfo.GetUITextById(27));
        if (innRecord.consumeIngFlour > 0)
            CreateItemForOther(innRecord.consumeIngFlour, spIconFlour, consumeIngStr + " " + GameCommonInfo.GetUITextById(28));
        //遍历食物
        foreach (GameItemsBean itemData in innRecord.listSellNumber)
        {
            MenuInfoBean foodData = innFoodManager.GetFoodDataById(itemData.itemId);
            Sprite foodIcon = innFoodManager.GetFoodSpriteByName(foodData.icon_key);
            CreateItemForMoney(
                foodIcon,
                foodData.name + " x" + itemData.itemNumber,
                1,
                itemData.priceL,
                itemData.priceM,
                itemData.priceS);
        }
        tvIncomeS.text =innRecord.incomeS + "";
        tvIncomeM.text =innRecord.incomeM + "";
        tvIncomeL.text =innRecord.incomeL + "";
        tvExpensesS.text =innRecord.expensesS + "";
        tvExpensesM.text =innRecord.expensesM + "";
        tvExpensesL.text =innRecord.expensesL + "";
    }

    public void CreateItemForMoney(Sprite spIcon, string name, int status, long moneyL, long moneyM, long moneyS)
    {
        GameObject objMoneyItem = Instantiate(objListRecordContent, objItemModelForMoney);
        ItemSettleForMoneyCpt itemCpt = objMoneyItem.GetComponent<ItemSettleForMoneyCpt>();
        itemCpt.SetData(spIcon, name, status, moneyL, moneyM, moneyS);
        AnimForItemShow(objMoneyItem);
    }

    public void CreateItemForOther(int number, Sprite ingIcon, string name)
    {
        GameObject objOtherItem = Instantiate(objListRecordContent, objItemModelForOther);
        ItemSettleForOtherCpt itemCpt = objOtherItem.GetComponent<ItemSettleForOtherCpt>();
        itemCpt.SetData(number, ingIcon, name);
        AnimForItemShow(objOtherItem);
    }

    /// <summary>
    /// item出现动画
    /// </summary>
    /// <param name="objItem"></param>
    public void AnimForItemShow(GameObject objItem)
    {
        objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From().SetDelay(animDelay + 0.1f).OnComplete(delegate ()
        {
            AudioHandler audioHandler = uiGameManager.audioHandler;
            audioHandler.PlaySound(AudioSoundEnum.PayMoney);
        });
        animDelay += 0.1f;
    }

    public void OpenDateUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
    }

}