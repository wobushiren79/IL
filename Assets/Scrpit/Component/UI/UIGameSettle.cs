using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameSettle : BaseUIComponent
{
    public Button btSubmit;
    public Button btCancel;

    //收入支出
    public Text tvIncomeS;
    public Text tvIncomeM;
    public Text tvIncomeL;
    public Text tvExpensesS;
    public Text tvExpensesM;
    public Text tvExpensesL;

    public InnHandler innHandler;
    public ControlHandler controlHandler;
    public GameTimeHandler gameTimeHandler;

    public GameObject objListRecordContent;
    public GameObject objItemIngModel;
    public GameObject objItemFoodModel;

    public Sprite spIconOilsalt;
    public Sprite spIconMeat;
    public Sprite spIconRiverfresh;
    public Sprite spIconSeafood;
    public Sprite spIconVegetablest;
    public Sprite spIconMelonfruit;
    public Sprite spIconWaterwine;
    public Sprite spIconFlour;

    private float animDelay;
    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OpenInn);
        if (btCancel != null)
            btCancel.onClick.AddListener(CloseInn);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public void InitData()
    {
        animDelay = 0.1f;
        if (innHandler.innRecord.consumeIngOilsalt > 0)
            CreateIngItem(innHandler.innRecord.consumeIngOilsalt, spIconOilsalt, "消耗食材 油盐");
        if (innHandler.innRecord.consumeIngMeat > 0)
            CreateIngItem(innHandler.innRecord.consumeIngMeat, spIconMeat, "消耗食材 鲜肉");
        if (innHandler.innRecord.consumeIngRiverfresh > 0)
            CreateIngItem(innHandler.innRecord.consumeIngRiverfresh, spIconRiverfresh, "消耗食材 河鲜");
        if (innHandler.innRecord.consumeIngSeafood > 0)
            CreateIngItem(innHandler.innRecord.consumeIngSeafood, spIconSeafood, "消耗食材 海鲜");
        if (innHandler.innRecord.consumeIngVegetablest > 0)
            CreateIngItem(innHandler.innRecord.consumeIngVegetablest, spIconVegetablest, "消耗食材 蔬菜");
        if (innHandler.innRecord.consumeIngMelonfruit > 0)
            CreateIngItem(innHandler.innRecord.consumeIngMelonfruit, spIconMelonfruit, "消耗食材 瓜果");
        if (innHandler.innRecord.consumeIngWaterwine > 0)
            CreateIngItem(innHandler.innRecord.consumeIngWaterwine, spIconWaterwine, "消耗食材 酒水");
        if (innHandler.innRecord.consumeIngFlour > 0)
            CreateIngItem(innHandler.innRecord.consumeIngFlour, spIconFlour, "消耗食材 面粉");
        //遍历食物
        foreach (KeyValuePair<long, int> kvp in innHandler.innRecord.sellNumber)
        {
            CreateFoodItem(kvp.Key, kvp.Value);
        }
        tvIncomeS.text = innHandler.innRecord.incomeS + "";
        tvIncomeM.text = innHandler.innRecord.incomeM + "";
        tvIncomeL.text = innHandler.innRecord.incomeL + "";
        tvExpensesS.text = innHandler.innRecord.expensesS + "";
        tvExpensesM.text = innHandler.innRecord.expensesM + "";
        tvExpensesL.text = innHandler.innRecord.expensesL + "";
    }

    public void CreateIngItem(int number, Sprite ingIcon, string name)
    {
        GameObject objIngItem = Instantiate(objItemIngModel, objListRecordContent.transform);
        objIngItem.SetActive(true);
        ItemSettleConsumeIngCpt itemCpt = objIngItem.GetComponent<ItemSettleConsumeIngCpt>();
        itemCpt.SetData(number, ingIcon, name);
        ItemAnim(objIngItem);
    }

    public void CreateFoodItem(long foodId, int number)
    {
        GameObject objFoodItem = Instantiate(objItemFoodModel, objListRecordContent.transform);
        objFoodItem.SetActive(true);
        ItemSettleFoodCpt itemCpt = objFoodItem.GetComponent<ItemSettleFoodCpt>();
        itemCpt.SetData(foodId, number);
        ItemAnim(objFoodItem);
    }

    public void ItemAnim(GameObject objItem)
    {
        objItem.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).SetDelay(animDelay + 0.1f);
        animDelay += 0.1f;
    }

    public void OpenInn()
    {
        uiManager.OpenUIAndCloseOtherByName("Attendance");
    }

    public void CloseInn()
    {
        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Rest;
        uiManager.OpenUIAndCloseOtherByName("Main");
        controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
    }
}