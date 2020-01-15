using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMiniGameCookingSelect : BaseUIComponent
{
    public Text tvTheme;

    public GameObject objMenuContainer;
    public GameObject objMenuModel;

    public MiniGameCookingBean gameCookingData;

    private ICallBack mCallBack;
    

    public void SetCallBack(ICallBack callBack)
    {
        this.mCallBack = callBack;
    }

    public void SetData(MiniGameCookingBean gameCookingData)
    {
        this.gameCookingData =  gameCookingData;
        SetTheme(gameCookingData.cookingTheme.name);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public void InitData()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        InnFoodManager innFoodManager = GetUIManager<UIGameManager>().innFoodManager;
        List<MenuOwnBean> listOwnMenu = gameDataManager.gameData.GetMenuList();
        for (int i = 0; i < listOwnMenu.Count; i++)
        {
            MenuOwnBean itemData= listOwnMenu[i];
            GameObject objItem = Instantiate(objMenuContainer, objMenuModel);
            ItemMiniGameCookingSelectMenuCpt itemCpt = objItem.GetComponent<ItemMiniGameCookingSelectMenuCpt>();
            itemCpt.SetData(itemData, innFoodManager.GetFoodDataById(itemData.menuId));
        }
    }

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
        if (mCallBack != null)
            mCallBack.UIMiniGameCookingSelect(menuInfo);
    }

    public interface ICallBack
    {
        void UIMiniGameCookingSelect(MenuInfoBean menuInfo);
    }
}