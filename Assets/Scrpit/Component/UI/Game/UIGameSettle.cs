using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameSettle : UIGameComponent
{
    public Button btSubmit;

    //收入支出
    public GameObject objIncomeL;
    public Text tvIncomeL;
    public GameObject objIncomeM;
    public Text tvIncomeM;
    public GameObject objIncomeS;
    public Text tvIncomeS;


    public GameObject objExpensesL;
    public Text tvExpensesL;
    public GameObject objExpensesM;
    public Text tvExpensesM;
    public GameObject objExpensesS;
    public Text tvExpensesS;



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
        //客流量
        CreateItemForOther(innRecord.GetTotalCustomer() + "", iconDataManager.GetIconSpriteByName("ach_ordernumber_1"), GameCommonInfo.GetUITextById(323), Color.green);
        CreateItemForOther(innRecord.GetTotalPayCustomer() + "", iconDataManager.GetIconSpriteByName("ach_ordernumber_1"), GameCommonInfo.GetUITextById(338), Color.green);

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
        string ingName = "";
        if (innRecord.consumeIngOilsalt > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName( IngredientsEnum.Oilsalt);
            CreateItemForOther("-" + innRecord.consumeIngOilsalt, spIconOilsalt, consumeIngStr + " " + ingName, Color.red);
            CreateItemForMoney(spIconOilsalt, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngOilsalt * 5);
            innRecord.AddPayIng(0,0, innRecord.consumeIngOilsalt * 5);
        }
        if (innRecord.consumeIngMeat > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Meat);
            CreateItemForOther("-" + innRecord.consumeIngMeat, spIconMeat, consumeIngStr + " " + ingName, Color.red);
            CreateItemForMoney(spIconMeat, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngMeat * 10);
            innRecord.AddPayIng(0, 0, innRecord.consumeIngMeat * 10);
        }
        if (innRecord.consumeIngRiverfresh > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Riverfresh);
            CreateItemForOther("-" + innRecord.consumeIngRiverfresh, spIconRiverfresh, consumeIngStr + " " + ingName, Color.red);
            CreateItemForMoney(spIconRiverfresh, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngRiverfresh * 10);
            innRecord.AddPayIng(0, 0, innRecord.consumeIngRiverfresh * 10);
        }  
        if (innRecord.consumeIngSeafood > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Seafood);
            CreateItemForOther("-" + innRecord.consumeIngSeafood, spIconSeafood, consumeIngStr + " " + ingName, Color.red);
            CreateItemForMoney(spIconSeafood, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngSeafood * 50);
            innRecord.AddPayIng(0, 0, innRecord.consumeIngSeafood * 50);
        }
        if (innRecord.consumeIngVegetables > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Vegetables);
            CreateItemForOther("-" + innRecord.consumeIngVegetables, spIconVegetables, consumeIngStr + " " + ingName, Color.red);
            CreateItemForMoney(spIconVegetables, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngVegetables * 5);
            innRecord.AddPayIng(0, 0, innRecord.consumeIngVegetables * 5);
        }
        if (innRecord.consumeIngMelonfruit > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Melonfruit);
            CreateItemForOther("-" + innRecord.consumeIngMelonfruit, spIconMelonfruit, consumeIngStr + " " + ingName, Color.red);
            CreateItemForMoney(spIconMelonfruit, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngMelonfruit * 5);
            innRecord.AddPayIng(0, 0, innRecord.consumeIngMelonfruit * 5);
        }
        if (innRecord.consumeIngWaterwine > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Waterwine);
            CreateItemForOther("-" + innRecord.consumeIngWaterwine, spIconWaterwine, consumeIngStr + " " + ingName, Color.red);
            CreateItemForMoney(spIconWaterwine, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngWaterwine * 10);
            innRecord.AddPayIng(0, 0, innRecord.consumeIngWaterwine * 10);
        }
        if (innRecord.consumeIngFlour > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Flour);
            CreateItemForOther("-" + innRecord.consumeIngFlour, spIconFlour, consumeIngStr + " " + ingName, Color.red);
            CreateItemForMoney(spIconFlour, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngFlour * 5);
            innRecord.AddPayIng(0, 0, innRecord.consumeIngFlour * 5);
        }
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
        if (innRecord.incomeL <= 0)
            objIncomeL.SetActive(false);
        if (innRecord.incomeM <= 0)
            objIncomeM.SetActive(false);
        tvIncomeL.text = innRecord.incomeL + "";
        tvIncomeM.text = innRecord.incomeM + "";
        tvIncomeS.text =innRecord.incomeS + "";

        if (innRecord.expensesL <= 0)
            objExpensesL.SetActive(false);
        if (innRecord.expensesM <= 0)
            objExpensesM.SetActive(false);
        tvExpensesL.text = innRecord.expensesL + "";
        tvExpensesM.text = innRecord.expensesM + "";
        tvExpensesS.text =innRecord.expensesS + "";
    }

    public void CreateItemForMoney(Sprite spIcon, string name, int status, long moneyL, long moneyM, long moneyS)
    {
        GameObject objMoneyItem = Instantiate(objListRecordContent, objItemModelForMoney);
        ItemSettleForMoneyCpt itemCpt = objMoneyItem.GetComponent<ItemSettleForMoneyCpt>();
        itemCpt.SetData(spIcon, name, status, moneyL, moneyM, moneyS);
        AnimForItemShow(objMoneyItem);
    }

    public void CreateItemForOther(string  number, Sprite ingIcon, string name,Color numberColor)
    {
        GameObject objOtherItem = Instantiate(objListRecordContent, objItemModelForOther);
        ItemSettleForOtherCpt itemCpt = objOtherItem.GetComponent<ItemSettleForOtherCpt>();
        itemCpt.SetData(number, ingIcon, name, numberColor);
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