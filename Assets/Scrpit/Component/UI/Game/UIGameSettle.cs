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
        UserAchievementBean userAchievement = gameDataManager.gameData.GetAchievementData();
        //停止时间
        gameTimeHandler.SetTimeStatus(true);
        CptUtil.RemoveChildsByActive(objListRecordContent.transform);
        animDelay = 0f;
        InnRecordBean innRecord = innHandler.GetInnRecord();
        long totalCustomerForFood = innRecord.GetTotalCompleteCustomerForFood();
        long totalCustomerForHotel = innRecord.GetTotalCompleteCustomerForHotel();
        //住客流量
        if (innRecord.GetTotalCustomerForHotel() > 0)
            CreateItemForOther(innRecord.GetTotalCustomerForHotel() + "", iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2"), GameCommonInfo.GetUITextById(342), Color.green);
        if (totalCustomerForHotel > 0)
            CreateItemForOther(totalCustomerForHotel + "", iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2"), GameCommonInfo.GetUITextById(343), Color.green);
        //食客流量
        CreateItemForOther(innRecord.GetTotalCustomerForFood() + "", iconDataManager.GetIconSpriteByName("ach_ordernumber_1"), GameCommonInfo.GetUITextById(323), Color.green);
        CreateItemForOther(totalCustomerForFood + "", iconDataManager.GetIconSpriteByName("ach_ordernumber_1"), GameCommonInfo.GetUITextById(338), Color.green);

        //记录单日最高成功接客
        userAchievement.SetMaxDayCompleteOrder(totalCustomerForFood, totalCustomerForHotel);
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
                innRecord.AddPayLoans(0, 0, itemPayLoans.moneySForDay);
            }
        }
        //食材消耗
        string consumeIngStr = GameCommonInfo.GetUITextById(4002);
        string ingName = "";
        if (innRecord.consumeIngOilsalt > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Oilsalt);
            CreateItemForOther("-" + innRecord.consumeIngOilsalt + "(" + GameCommonInfo.GetUITextById(93) + gameDataManager.gameData.ingOilsalt + ")", spIconOilsalt, consumeIngStr + " " + ingName, Color.red);
            //CreateItemForMoney(spIconOilsalt, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngOilsalt * 5);
            //innRecord.AddPayIng(0, 0, innRecord.consumeIngOilsalt * 5);
        }
        if (innRecord.consumeIngMeat > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Meat);
            CreateItemForOther("-" + innRecord.consumeIngMeat + "(" + GameCommonInfo.GetUITextById(93) + gameDataManager.gameData.ingMeat + ")", spIconMeat, consumeIngStr + " " + ingName, Color.red);
            //CreateItemForMoney(spIconMeat, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngMeat * 10);
            //innRecord.AddPayIng(0, 0, innRecord.consumeIngMeat * 10);
        }
        if (innRecord.consumeIngRiverfresh > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Riverfresh);
            CreateItemForOther("-" + innRecord.consumeIngRiverfresh + "(" + GameCommonInfo.GetUITextById(93) + gameDataManager.gameData.ingRiverfresh + ")", spIconRiverfresh, consumeIngStr + " " + ingName, Color.red);
            //CreateItemForMoney(spIconRiverfresh, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngRiverfresh * 10);
            //innRecord.AddPayIng(0, 0, innRecord.consumeIngRiverfresh * 10);
        }
        if (innRecord.consumeIngSeafood > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Seafood);
            CreateItemForOther("-" + innRecord.consumeIngSeafood + "(" + GameCommonInfo.GetUITextById(93) + gameDataManager.gameData.ingSeafood + ")", spIconSeafood, consumeIngStr + " " + ingName, Color.red);
            //CreateItemForMoney(spIconSeafood, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngSeafood * 50);
            //innRecord.AddPayIng(0, 0, innRecord.consumeIngSeafood * 50);
        }
        if (innRecord.consumeIngVegetables > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Vegetables);
            CreateItemForOther("-" + innRecord.consumeIngVegetables + "(" + GameCommonInfo.GetUITextById(93) + gameDataManager.gameData.ingVegetables + ")", spIconVegetables, consumeIngStr + " " + ingName, Color.red);
            //CreateItemForMoney(spIconVegetables, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngVegetables * 5);
            //innRecord.AddPayIng(0, 0, innRecord.consumeIngVegetables * 5);
        }
        if (innRecord.consumeIngMelonfruit > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Melonfruit);
            CreateItemForOther("-" + innRecord.consumeIngMelonfruit + "(" + GameCommonInfo.GetUITextById(93) + gameDataManager.gameData.ingMelonfruit + ")", spIconMelonfruit, consumeIngStr + " " + ingName, Color.red);
            //CreateItemForMoney(spIconMelonfruit, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngMelonfruit * 5);
            // innRecord.AddPayIng(0, 0, innRecord.consumeIngMelonfruit * 5);
        }
        if (innRecord.consumeIngWaterwine > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Waterwine);
            CreateItemForOther("-" + innRecord.consumeIngWaterwine + "(" + GameCommonInfo.GetUITextById(93) + gameDataManager.gameData.ingWaterwine + ")", spIconWaterwine, consumeIngStr + " " + ingName, Color.red);
            // CreateItemForMoney(spIconWaterwine, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngWaterwine * 10);
            // innRecord.AddPayIng(0, 0, innRecord.consumeIngWaterwine * 10);
        }
        if (innRecord.consumeIngFlour > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Flour);
            CreateItemForOther("-" + innRecord.consumeIngFlour + "(" + GameCommonInfo.GetUITextById(93) + gameDataManager.gameData.ingFlour + ")", spIconFlour, consumeIngStr + " " + ingName, Color.red);
            // CreateItemForMoney(spIconFlour, string.Format(GameCommonInfo.GetUITextById(339), ingName), 0, 0, 0, innRecord.consumeIngFlour * 5);
            // innRecord.AddPayIng(0, 0, innRecord.consumeIngFlour * 5);
        }

        //住宿金额
        if (innRecord.incomeForHotelS != 0)
        {
            CreateItemForMoney(
                iconDataManager.GetIconSpriteByName("money_1"),
                GameCommonInfo.GetUITextById(344),
                1,
                innRecord.incomeForHotelL,
                innRecord.incomeForHotelM,
                innRecord.incomeForHotelS);
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
        innRecord.GetTotalIncome(out long incomeL, out long incomeM, out long incomeS);
        innRecord.GetTotalExpenses(out long expensesL, out long expensesM, out long expensesS);
        if (incomeL <= 0)
            objIncomeL.SetActive(false);
        if (incomeM <= 0)
            objIncomeM.SetActive(false);
        tvIncomeL.text = incomeL + "";
        tvIncomeM.text = incomeM + "";
        tvIncomeS.text = incomeS + "";
        //记录单日最高收入
        userAchievement.SetMaxDayGetMoney(innRecord.incomeL, innRecord.incomeM, innRecord.incomeS);
        userAchievement.SetMaxDayGetMoneyForHotel(innRecord.incomeForHotelL, innRecord.incomeForHotelM, innRecord.incomeForHotelS);

        if (innRecord.expensesL <= 0)
            objExpensesL.SetActive(false);
        if (innRecord.expensesM <= 0)
            objExpensesM.SetActive(false);
        tvExpensesL.text = expensesL + "";
        tvExpensesM.text = expensesM + "";
        tvExpensesS.text = expensesS + "";
    }

    public void CreateItemForMoney(Sprite spIcon, string name, int status, long moneyL, long moneyM, long moneyS)
    {
        GameObject objMoneyItem = Instantiate(objListRecordContent, objItemModelForMoney);
        ItemSettleForMoneyCpt itemCpt = objMoneyItem.GetComponent<ItemSettleForMoneyCpt>();
        itemCpt.SetData(spIcon, name, status, moneyL, moneyM, moneyS);
        AnimForItemShow(objMoneyItem);
    }

    public void CreateItemForOther(string number, Sprite ingIcon, string name, Color numberColor)
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
            //最多只播放10个音效
            if (animDelay <= 1.1f)
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