using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameSettle : BaseUIComponent
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

    public ScrollGridVertical gridVertical;

    public Sprite spIconOilsalt;
    public Sprite spIconMeat;
    public Sprite spIconRiverfresh;
    public Sprite spIconSeafood;
    public Sprite spIconVegetables;
    public Sprite spIconMelonfruit;
    public Sprite spIconWaterwine;
    public Sprite spIconFlour;

    List<ItemData> listData = new List<ItemData>();
    public struct ItemData
    {
        public Sprite icon;
        public int type;
        public int state;
        public string name;
        public string content;
        public Color colorContent;
        public long moneyL;
        public long moneyM;
        public long moneyS;
    }

    public override void Awake()
    {
        base.Awake();
        if (gridVertical != null)
            gridVertical.AddCellListener(OnCellForItem);
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

    public void OnCellForItem(ScrollGridCell itemCell)
    {
        ItemSettleCpt itemSettle = itemCell.GetComponent<ItemSettleCpt>();
        ItemData itemData = listData[itemCell.index];
        if (itemData.type == 1)
        {
            itemSettle.SetData(itemData.icon, itemData.name, itemData.state, itemData.moneyL, itemData.moneyM, itemData.moneyS);
        }
        else
        {
            itemSettle.SetData(itemData.content, itemData.icon, itemData.name, itemData.colorContent);
        }
    }

    public void InitData()
    {
        listData.Clear();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        //停止时间
        GameTimeHandler.Instance.SetTimeStatus(true);

        InnRecordBean innRecord = InnHandler.Instance.GetInnRecord();
        long totalCustomerForFood = innRecord.GetTotalCompleteCustomerForFood();
        long totalCustomerForHotel = innRecord.GetTotalCompleteCustomerForHotel();
        //住客流量
        if (innRecord.GetTotalCustomerForHotel() > 0)
            CreateItemForOther(innRecord.GetTotalCustomerForHotel() + "", IconDataHandler.Instance.manager.GetIconSpriteByName("worker_waiter_bed_pro_2"), TextHandler.Instance.manager.GetTextById(342), Color.green);
        if (totalCustomerForHotel > 0)
            CreateItemForOther(totalCustomerForHotel + "", IconDataHandler.Instance.manager.GetIconSpriteByName("worker_waiter_bed_pro_2"), TextHandler.Instance.manager.GetTextById(343), Color.green);
        //食客流量
        CreateItemForOther(innRecord.GetTotalCustomerForFood() + "", IconDataHandler.Instance.manager.GetIconSpriteByName("ach_ordernumber_1"), TextHandler.Instance.manager.GetTextById(323), Color.green);
        CreateItemForOther(totalCustomerForFood + "", IconDataHandler.Instance.manager.GetIconSpriteByName("ach_ordernumber_1"), TextHandler.Instance.manager.GetTextById(338), Color.green);

        //记录单日最高成功接客
        userAchievement.SetMaxDayCompleteOrder(totalCustomerForFood, totalCustomerForHotel);
        //员工支出
        CreateItemForMoney(
                IconDataHandler.Instance.manager.GetIconSpriteByName("money_1"),
                TextHandler.Instance.manager.GetTextById(183),
                0,
                innRecord.payWageL,
                innRecord.payWageM,
                innRecord.payWageS);
        //借贷还款
        if (gameData.listLoans.Count > 0)
        {
            gameData.PayLoans(out List<UserLoansBean> listPayLoans);
            foreach (UserLoansBean itemPayLoans in listPayLoans)
            {
                CreateItemForMoney(
                    IconDataHandler.Instance.manager.GetIconSpriteByName("money_1"),
                    TextHandler.Instance.manager.GetTextById(184),
                    0,
                    0,
                    0,
                    itemPayLoans.moneySForDay);
                innRecord.AddPayLoans(0, 0, itemPayLoans.moneySForDay);
            }
        }
        //食材消耗
        string consumeIngStr = TextHandler.Instance.manager.GetTextById(4002);
        string ingName = "";
        if (innRecord.consumeIngOilsalt > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Oilsalt);
            CreateItemForOther("-" + innRecord.consumeIngOilsalt + "(" + TextHandler.Instance.manager.GetTextById(93) + gameData.ingOilsalt + ")", spIconOilsalt, consumeIngStr + " " + ingName, Color.red);
        }
        if (innRecord.consumeIngMeat > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Meat);
            CreateItemForOther("-" + innRecord.consumeIngMeat + "(" + TextHandler.Instance.manager.GetTextById(93) + gameData.ingMeat + ")", spIconMeat, consumeIngStr + " " + ingName, Color.red);
        }
        if (innRecord.consumeIngRiverfresh > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Riverfresh);
            CreateItemForOther("-" + innRecord.consumeIngRiverfresh + "(" + TextHandler.Instance.manager.GetTextById(93) + gameData.ingRiverfresh + ")", spIconRiverfresh, consumeIngStr + " " + ingName, Color.red);
        }
        if (innRecord.consumeIngSeafood > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Seafood);
            CreateItemForOther("-" + innRecord.consumeIngSeafood + "(" + TextHandler.Instance.manager.GetTextById(93) + gameData.ingSeafood + ")", spIconSeafood, consumeIngStr + " " + ingName, Color.red);
        }
        if (innRecord.consumeIngVegetables > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Vegetables);
            CreateItemForOther("-" + innRecord.consumeIngVegetables + "(" + TextHandler.Instance.manager.GetTextById(93) + gameData.ingVegetables + ")", spIconVegetables, consumeIngStr + " " + ingName, Color.red);
        }
        if (innRecord.consumeIngMelonfruit > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Melonfruit);
            CreateItemForOther("-" + innRecord.consumeIngMelonfruit + "(" + TextHandler.Instance.manager.GetTextById(93) + gameData.ingMelonfruit + ")", spIconMelonfruit, consumeIngStr + " " + ingName, Color.red);
        }
        if (innRecord.consumeIngWaterwine > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Waterwine);
            CreateItemForOther("-" + innRecord.consumeIngWaterwine + "(" + TextHandler.Instance.manager.GetTextById(93) + gameData.ingWaterwine + ")", spIconWaterwine, consumeIngStr + " " + ingName, Color.red);
        }
        if (innRecord.consumeIngFlour > 0)
        {
            ingName = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Flour);
            CreateItemForOther("-" + innRecord.consumeIngFlour + "(" + TextHandler.Instance.manager.GetTextById(93) + gameData.ingFlour + ")", spIconFlour, consumeIngStr + " " + ingName, Color.red);
        }
        //评价
        if (innRecord.praiseExcitedNumber != 0)
            CreateItemForOther(innRecord.praiseExcitedNumber + "", IconDataHandler.Instance.manager.GetIconSpriteByName("customer_mood_0"), "", Color.green);
        if (innRecord.praiseHappyNumber != 0)
            CreateItemForOther(innRecord.praiseHappyNumber + "", IconDataHandler.Instance.manager.GetIconSpriteByName("customer_mood_1"), "", Color.green);
        if (innRecord.praiseOkayNumber != 0)
            CreateItemForOther(innRecord.praiseOkayNumber + "", IconDataHandler.Instance.manager.GetIconSpriteByName("customer_mood_2"), "", Color.green);
        if (innRecord.praiseOrdinaryNumber != 0)
            CreateItemForOther(innRecord.praiseOrdinaryNumber + "", IconDataHandler.Instance.manager.GetIconSpriteByName("customer_mood_3"), "", Color.red);
        if (innRecord.praiseDisappointedNumber != 0)
            CreateItemForOther(innRecord.praiseDisappointedNumber + "", IconDataHandler.Instance.manager.GetIconSpriteByName("customer_mood_4"), "", Color.red);
        if (innRecord.praiseAngerNumber != 0)
            CreateItemForOther(innRecord.praiseAngerNumber + "", IconDataHandler.Instance.manager.GetIconSpriteByName("customer_mood_5"), "", Color.red);

        //住宿金额
        if (innRecord.incomeForHotelS != 0)
        {
            CreateItemForMoney(
                IconDataHandler.Instance.manager.GetIconSpriteByName("money_1"),
                TextHandler.Instance.manager.GetTextById(344),
                1,
                innRecord.incomeForHotelL,
                innRecord.incomeForHotelM,
                innRecord.incomeForHotelS);
        }
        //遍历食物
        foreach (GameItemsBean itemData in innRecord.listSellNumber)
        {
            MenuInfoBean foodData = InnFoodHandler.Instance.manager.GetFoodDataById(itemData.itemId);
            Sprite foodIcon = InnFoodHandler.Instance.manager.GetFoodSpriteByName(foodData.icon_key);
            CreateItemForMoney(
                foodIcon,
                foodData.name + " x" + itemData.itemNumber,
                1,
                itemData.priceL,
                itemData.priceM,
                itemData.priceS);
        }
        AudioHandler.Instance.PlaySound(AudioSoundEnum.PayMoney);


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

        gridVertical.SetCellCount(listData.Count);
    }

    public void CreateItemForMoney(Sprite spIcon, string name, int status, long moneyL, long moneyM, long moneyS)
    {
        ItemData itemData = new ItemData();
        itemData.icon = spIcon;
        itemData.name = name;
        itemData.state = status;
        itemData.moneyL = moneyL;
        itemData.moneyM = moneyM;
        itemData.moneyS = moneyS;
        itemData.type = 1;
        listData.Add(itemData);
    }

    public void CreateItemForOther(string number, Sprite ingIcon, string name, Color numberColor)
    {
        ItemData itemData = new ItemData();
        itemData.icon = ingIcon;
        itemData.name = name;
        itemData.content = number;
        itemData.colorContent = numberColor;
        itemData.type = 2;
        listData.Add(itemData);
    }


    public void OpenDateUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameDate>(UIEnum.GameDate);
    }

}