using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UIMiniGameCookingSelect : BaseUIComponent
{
    public Text tvTheme;

    public ScrollGridVertical gridVertical;

    public MiniGameCookingBean gameCookingData;
    List<MenuOwnBean> listOwnMenu;

    public void Start()
    {
        gridVertical.AddCellListener(OnCellForItem);
    }

    public void SetData(MiniGameCookingBean gameCookingData)
    {
        this.gameCookingData =  gameCookingData;
        SetTheme(gameCookingData.GetCookingTheme().name);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
        AnimForInit();
    }

    public void OnCellForItem(ScrollGridCell itemCell)
    {
        ItemMiniGameCookingSelectMenuCpt itemCpt = itemCell.GetComponent<ItemMiniGameCookingSelectMenuCpt>();
        MenuOwnBean itemData = listOwnMenu[itemCell.index];
        itemCpt.SetData(itemData, InnFoodHandler.Instance.manager.GetFoodDataById(itemData.menuId));
    }

    

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        listOwnMenu = gameData.GetMenuList();
        gridVertical.SetCellCount(listOwnMenu.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From();
    }

    /// <summary>
    /// 设置主题
    /// </summary>
    /// <param name="theme"></param>
    public void SetTheme(string theme)
    {
        if (tvTheme!=null)
        {
            tvTheme.text = theme;
        }
    }

    /// <summary>
    /// 选择菜单
    /// </summary>
    /// <param name="menuInfo"></param>
    public void SelectMenu(MenuInfoBean menuInfo)
    {
        EventHandler.Instance.TriggerEvent(EventsInfo.MiniGameCooking_MenuSelect, menuInfo);
    }
}