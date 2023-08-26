using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static GameControlHandler;

public partial class UIItemGameBuildCourtyard : BaseUIView
{
    protected Button btSumit;
    protected ItemBean itemData;
    protected ItemsInfoBean itemsInfo;
    public override void Awake()
    {
        base.Awake();
        btSumit = GetComponent<Button>();
        btSumit.onClick.AddListener(OnClickForSubmit);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemData"></param>
    public void SetData(ItemBean itemData)
    {
        this.itemData = itemData;
        itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(itemData.itemId);
        SetName(itemsInfo.name);
        SetIcon(itemsInfo.icon_key);
        SetNum(itemData.itemNumber);
        var seedInfo = SeedInfoCfg.GetItemDataByItemId(itemData.itemId);
        SetDays(seedInfo.growup_oneloopday * seedInfo.growup_totleloop);
        seedInfo.GetIng(out Dictionary<IngredientsEnum, int> dicIng);
        if (dicIng.IsNull())
        {
            ui_Ing.ShowObj(false);
        }
        else
        {
            ui_Ing.ShowObj(true);
            foreach (var itemIng in dicIng)
            {
                SetIngNum(itemIng.Key, itemIng.Value);
                break;                
            }
        }
    }

    /// <summary>
    /// 设置天数
    /// </summary>
    /// <param name="days"></param>
    public void SetDays(int days)
    {
        ui_DaysTex.text = $"{days}";
    }

    /// <summary>
    /// 设置食材数量
    /// </summary>
    public void SetIngNum(IngredientsEnum ingredients, int num)
    {
        Sprite spIcon = IngredientsEnumTools.GetIngredientIcon(ingredients);
        ui_IngIcon.sprite = spIcon;
        ui_IngTex.text = $"{num}";
    }


    /// <summary>
    /// 设置名字
    /// </summary>
    public void SetName(string content)
    {
        ui_Name.text = content;

    }

    /// <summary>
    /// 设置图标
    /// </summary>
    public void SetIcon(string iconKey)
    {
        IconHandler.Instance.manager.GetItemsSpriteByName(iconKey,(sp)=> 
        {
            ui_Icon.sprite = sp;
        });
    }


    /// <summary>
    /// 设置数量
    /// </summary>
    public void SetNum(long num)
    {
        ui_Number.text = $"x{num}";
    }


    /// <summary>
    /// 点击选中
    /// </summary>
    public void OnClickForSubmit()
    {
        ControlForBuildCourtyardCpt controlForBuild = GameControlHandler.Instance.manager.GetControl<ControlForBuildCourtyardCpt>(ControlEnum.BuildCourtyard);
        //设置种子建筑
        controlForBuild.ShowBuildItem(980001, itemData: itemData);
    }

}