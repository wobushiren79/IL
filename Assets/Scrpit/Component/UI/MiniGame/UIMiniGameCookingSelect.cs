using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

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
        SetTheme(gameCookingData.GetCookingTheme().name);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
        AnimForInit();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        
        List<MenuOwnBean> listOwnMenu = gameData.GetMenuList();
        for (int i = 0; i < listOwnMenu.Count; i++)
        {
            MenuOwnBean itemData= listOwnMenu[i];
            GameObject objItem = Instantiate(objMenuContainer, objMenuModel);
            ItemMiniGameCookingSelectMenuCpt itemCpt = objItem.GetComponent<ItemMiniGameCookingSelectMenuCpt>();
            itemCpt.SetData(itemData, InnFoodHandler.Instance.manager.GetFoodDataById(itemData.menuId));
        }
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
        if (mCallBack != null)
            mCallBack.UIMiniGameCookingSelect(menuInfo);
    }

    public interface ICallBack
    {
        void UIMiniGameCookingSelect(MenuInfoBean menuInfo);
    }
}