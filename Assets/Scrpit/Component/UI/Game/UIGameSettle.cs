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
    public Sprite spIconVegetables;
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
        if (innHandler.GetInnRecord().consumeIngOilsalt > 0)
            CreateIngItem(innHandler.GetInnRecord().consumeIngOilsalt, spIconOilsalt, consumeIngStr + " " + GameCommonInfo.GetUITextById(21));
        if (innHandler.GetInnRecord().consumeIngMeat > 0)
            CreateIngItem(innHandler.GetInnRecord().consumeIngMeat, spIconMeat, consumeIngStr + " " + GameCommonInfo.GetUITextById(22));
        if (innHandler.GetInnRecord().consumeIngRiverfresh > 0)
            CreateIngItem(innHandler.GetInnRecord().consumeIngRiverfresh, spIconRiverfresh, consumeIngStr + " " + GameCommonInfo.GetUITextById(23));
        if (innHandler.GetInnRecord().consumeIngSeafood > 0)
            CreateIngItem(innHandler.GetInnRecord().consumeIngSeafood, spIconSeafood, consumeIngStr + " " + GameCommonInfo.GetUITextById(24));
        if (innHandler.GetInnRecord().consumeIngVegetables > 0)
            CreateIngItem(innHandler.GetInnRecord().consumeIngVegetables, spIconVegetables, consumeIngStr + " " + GameCommonInfo.GetUITextById(25));
        if (innHandler.GetInnRecord().consumeIngMelonfruit > 0)
            CreateIngItem(innHandler.GetInnRecord().consumeIngMelonfruit, spIconMelonfruit, consumeIngStr + " " + GameCommonInfo.GetUITextById(26));
        if (innHandler.GetInnRecord().consumeIngWaterwine > 0)
            CreateIngItem(innHandler.GetInnRecord().consumeIngWaterwine, spIconWaterwine, consumeIngStr + " " + GameCommonInfo.GetUITextById(27));
        if (innHandler.GetInnRecord().consumeIngFlour > 0)
            CreateIngItem(innHandler.GetInnRecord().consumeIngFlour, spIconFlour, consumeIngStr + " " + GameCommonInfo.GetUITextById(28));
        //遍历食物
        foreach (ItemBean itemData in innHandler.GetInnRecord().listSellNumber)
        {
            CreateFoodItem(itemData.itemId, (int)itemData.itemNumber);
        }
        tvIncomeS.text = innHandler.GetInnRecord().incomeS + "";
        tvIncomeM.text = innHandler.GetInnRecord().incomeM + "";
        tvIncomeL.text = innHandler.GetInnRecord().incomeL + "";
        tvExpensesS.text = innHandler.GetInnRecord().expensesS + "";
        tvExpensesM.text = innHandler.GetInnRecord().expensesM + "";
        tvExpensesL.text = innHandler.GetInnRecord().expensesL + "";
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