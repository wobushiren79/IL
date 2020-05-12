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
        AudioHandler audioHandler = uiGameManager.audioHandler;
        GameTimeHandler gameTimeHandler = uiGameManager.gameTimeHandler;
        //停止时间
        gameTimeHandler.SetTimeStatus(true);
        CptUtil.RemoveChildsByActive(objListRecordContent.transform);
        animDelay =0f;
        InnRecordBean innRecord = innHandler.GetInnRecord();
        //员工支出
        CreateItemForMoney(
                iconDataManager.GetIconSpriteByName("money_1"),
                GameCommonInfo.GetUITextById(183),
                0,
                innRecord.payWageL,
                innRecord.payWageM,
                innRecord.payWageS);
        //食材消耗
        string consumeIngStr = GameCommonInfo.GetUITextById(4002);
        if (innHandler.GetInnRecord().consumeIngOilsalt > 0)
            CreateItemForOther(innHandler.GetInnRecord().consumeIngOilsalt, spIconOilsalt, consumeIngStr + " " + GameCommonInfo.GetUITextById(21));
        if (innHandler.GetInnRecord().consumeIngMeat > 0)
            CreateItemForOther(innHandler.GetInnRecord().consumeIngMeat, spIconMeat, consumeIngStr + " " + GameCommonInfo.GetUITextById(22));
        if (innHandler.GetInnRecord().consumeIngRiverfresh > 0)
            CreateItemForOther(innHandler.GetInnRecord().consumeIngRiverfresh, spIconRiverfresh, consumeIngStr + " " + GameCommonInfo.GetUITextById(23));
        if (innHandler.GetInnRecord().consumeIngSeafood > 0)
            CreateItemForOther(innHandler.GetInnRecord().consumeIngSeafood, spIconSeafood, consumeIngStr + " " + GameCommonInfo.GetUITextById(24));
        if (innHandler.GetInnRecord().consumeIngVegetables > 0)
            CreateItemForOther(innHandler.GetInnRecord().consumeIngVegetables, spIconVegetables, consumeIngStr + " " + GameCommonInfo.GetUITextById(25));
        if (innHandler.GetInnRecord().consumeIngMelonfruit > 0)
            CreateItemForOther(innHandler.GetInnRecord().consumeIngMelonfruit, spIconMelonfruit, consumeIngStr + " " + GameCommonInfo.GetUITextById(26));
        if (innHandler.GetInnRecord().consumeIngWaterwine > 0)
            CreateItemForOther(innHandler.GetInnRecord().consumeIngWaterwine, spIconWaterwine, consumeIngStr + " " + GameCommonInfo.GetUITextById(27));
        if (innHandler.GetInnRecord().consumeIngFlour > 0)
            CreateItemForOther(innHandler.GetInnRecord().consumeIngFlour, spIconFlour, consumeIngStr + " " + GameCommonInfo.GetUITextById(28));
        //遍历食物
        foreach (ItemBean itemData in innHandler.GetInnRecord().listSellNumber)
        {
            MenuInfoBean foodData = innFoodManager.GetFoodDataById(itemData.itemId);
            Sprite foodIcon = innFoodManager.GetFoodSpriteByName(foodData.icon_key);
            CreateItemForMoney(
                foodIcon,
                foodData.name + " x" + itemData.itemNumber,
                1,
                foodData.price_l * itemData.itemNumber,
                foodData.price_m * itemData.itemNumber,
                foodData.price_s * itemData.itemNumber);
            audioHandler.PlaySound(AudioSoundEnum.PayMoney);
        }
        tvIncomeS.text = innHandler.GetInnRecord().incomeS + "";
        tvIncomeM.text = innHandler.GetInnRecord().incomeM + "";
        tvIncomeL.text = innHandler.GetInnRecord().incomeL + "";
        tvExpensesS.text = innHandler.GetInnRecord().expensesS + "";
        tvExpensesM.text = innHandler.GetInnRecord().expensesM + "";
        tvExpensesL.text = innHandler.GetInnRecord().expensesL + "";
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
        objItem.transform.DOScale(new Vector3(0, 0, 0), 0.2f).From().SetDelay(animDelay + 0.1f);
        animDelay += 0.1f;
    }

    public void OpenDateUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
    }

}