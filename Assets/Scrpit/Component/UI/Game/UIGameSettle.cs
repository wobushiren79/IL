using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameSettle : BaseUIComponent
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

    protected ControlHandler controlHandler;

    public override void Awake()
    {
        base.Awake();
        controlHandler = GetUIManager<UIGameManager>().controlHandler;
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
        if (controlHandler != null)
            controlHandler.StopControl();
    }

    public void InitData()
    {
        InnHandler innHandler = GetUIManager<UIGameManager>().innHandler;
        CptUtil.RemoveChildsByActive(objListRecordContent.transform);
        animDelay = 0.1f;
        string consumeIngStr = GameCommonInfo.GetUITextById(4002);
        if (innHandler.innRecord.consumeIngOilsalt > 0)
            CreateIngItem(innHandler.innRecord.consumeIngOilsalt, spIconOilsalt, consumeIngStr + " " + GameCommonInfo.GetUITextById(21));
        if (innHandler.innRecord.consumeIngMeat > 0)
            CreateIngItem(innHandler.innRecord.consumeIngMeat, spIconMeat, consumeIngStr + " " + GameCommonInfo.GetUITextById(22));
        if (innHandler.innRecord.consumeIngRiverfresh > 0)
            CreateIngItem(innHandler.innRecord.consumeIngRiverfresh, spIconRiverfresh, consumeIngStr + " " + GameCommonInfo.GetUITextById(23));
        if (innHandler.innRecord.consumeIngSeafood > 0)
            CreateIngItem(innHandler.innRecord.consumeIngSeafood, spIconSeafood, consumeIngStr + " " + GameCommonInfo.GetUITextById(24));
        if (innHandler.innRecord.consumeIngVegetablest > 0)
            CreateIngItem(innHandler.innRecord.consumeIngVegetablest, spIconVegetablest, consumeIngStr + " " + GameCommonInfo.GetUITextById(25));
        if (innHandler.innRecord.consumeIngMelonfruit > 0)
            CreateIngItem(innHandler.innRecord.consumeIngMelonfruit, spIconMelonfruit, consumeIngStr + " " + GameCommonInfo.GetUITextById(26));
        if (innHandler.innRecord.consumeIngWaterwine > 0)
            CreateIngItem(innHandler.innRecord.consumeIngWaterwine, spIconWaterwine, consumeIngStr + " " + GameCommonInfo.GetUITextById(27));
        if (innHandler.innRecord.consumeIngFlour > 0)
            CreateIngItem(innHandler.innRecord.consumeIngFlour, spIconFlour, consumeIngStr + " " + GameCommonInfo.GetUITextById(28));
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

    public void OpenDateUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
    }

}