using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTownCerpenterCpt : ItemTownStoreCpt, DialogView.IDialogCallBack
{
    public GameObject objAttribute;
    public Text tvAttribute;

    public GameObject objOwn;
    public BuildItemBean buildItemData;

    public InfoPromptPopupButton infoPromptPopup;

    public override void Start()
    {
        base.Start();
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        if (infoPromptPopup != null)
            infoPromptPopup.SetPopupShowView(uiGameManager.infoPromptPopup);
    }

    public override void RefreshUI()
    {
        SetOwn((StoreForCarpenterTypeEnum)storeInfo.store_goods_type);
    }

    public void SetData(StoreInfoBean itemData)
    {
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;

        storeInfo = itemData;
        float aesthetics = 0;
        string iconKey = "";
        string name = "";
        string content = "";
        StoreForCarpenterTypeEnum type = (StoreForCarpenterTypeEnum)storeInfo.store_goods_type;
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            iconKey = storeInfo.icon_key;
            name = storeInfo.name;
            content = storeInfo.content;
        }
        else
        {
            buildItemData = innBuildManager.GetBuildDataById(itemData.mark_id);
            if (buildItemData != null)
            {
                aesthetics = buildItemData.aesthetics;
                iconKey = buildItemData.icon_key;
                name = buildItemData.name;
                content = buildItemData.content;
            }
        }

        SetPrice(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s,
                   storeInfo.guild_coin,
                   storeInfo.trophy_elementary, storeInfo.trophy_intermediate, storeInfo.trophy_advanced, storeInfo.trophy_legendary);
        SetName(name);
        SetIcon(type, buildItemData, itemData);
        SetAttribute(type, aesthetics);
        SetContent(content);
        SetOwn(type);
        SetGetNumber(storeInfo.get_number);
        SetPopup(content);
    }

    /// <summary>
    /// 设置弹出框
    /// </summary>
    /// <param name="content"></param>
    public void SetPopup(string content)
    {
        if (infoPromptPopup != null)
            infoPromptPopup.SetContent(content);
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="type"></param>
    /// <param name="a"></param>
    public void SetAttribute(StoreForCarpenterTypeEnum type, float aesthetics)
    {
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            objAttribute.gameObject.SetActive(false);
        }
        else
        {
            objAttribute.gameObject.SetActive(true);
        }
        if (tvAttribute != null)
            tvAttribute.text = GameCommonInfo.GetUITextById(10) + "+" + aesthetics;
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="mark"></param>
    /// <param name="markId"></param>
    public void SetIcon(StoreForCarpenterTypeEnum type, BuildItemBean buildItem, StoreInfoBean storeInfo)
    {
        IconDataManager iconDataManager = GetUIManager<UIGameManager>().iconDataManager;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;
        if (iconDataManager == null)
            return;
        Sprite spIcon = null;
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            spIcon = iconDataManager.GetIconSpriteByName(storeInfo.icon_key);
        }
        else
        {
            spIcon = BuildItemTypeEnumTools.GetBuildItemSprite(innBuildManager, buildItem);
        }

        if (ivIcon != null && spIcon != null)
            ivIcon.sprite = spIcon;
    }



    /// <summary>
    /// 设置拥有数量
    /// </summary>
    public void SetOwn(StoreForCarpenterTypeEnum type)
    {
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            objOwn.gameObject.SetActive(false);
        }
        else
        {
            objOwn.gameObject.SetActive(true);
            GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
            if (tvOwn == null)
                return;
            tvOwn.text = (GameCommonInfo.GetUITextById(4001) + "\n" + gameDataManager.gameData.GetBuildNumber(storeInfo.mark_id));
        }
    }

    /// <summary>
    /// 购买确认
    /// </summary>
    public override void OnClickSubmitBuy()
    {
        base.OnClickSubmitBuy();

        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        ToastManager toastManager = uiGameManager.toastManager;
        DialogManager dialogManager = uiGameManager.dialogManager;

        InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
        if (gameDataManager == null || storeInfo == null)
            return;

        //检测是否正在修建客栈
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion && innBuildData.listBuildDay.Count != 0)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1019));
            return;
        }
        //检测金钱
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion && !gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
        {
            //生成普通的对话框
            DialogBean dialogBean = new DialogBean();
            if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
            {
                dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3010), 1 + "");
            }
            else
                dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3002), buildItemData.name);
            dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
        }
        else
        {
            DialogBean dialogBean = new DialogBean();
            PickForNumberDialogView dialogView = (PickForNumberDialogView)dialogManager.CreateDialog(DialogEnum.PickForNumber, this, dialogBean);
            dialogView.SetData(ivIcon.sprite, 999);
        }

    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        ToastManager toastManager = uiGameManager.toastManager;
        GameTimeHandler gameTimeHandler = uiGameManager.gameTimeHandler;
        InnBuildManager innBuildManager = uiGameManager.innBuildManager;

        if (dialogView as PickForNumberDialogView)
        {
            PickForNumberDialogView pickForNumberDialog = dialogView as PickForNumberDialogView;
            long number= pickForNumberDialog.GetPickNumber();
            //检测金钱
            if (!gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l* number, storeInfo.price_m* number, storeInfo.price_s* number))
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
                return;
            }
            if (!gameDataManager.gameData.HasEnoughGuildCoin(storeInfo.guild_coin * number))
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1012));
                return;
            }
            if (!gameDataManager.gameData.HasEnoughTrophy(storeInfo.trophy_elementary * number, storeInfo.trophy_intermediate * number, storeInfo.trophy_advanced * number, storeInfo.trophy_legendary * number))
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1021));
                return;
            }
            gameDataManager.gameData.PayMoney(storeInfo.price_l * number, storeInfo.price_m * number, storeInfo.price_s * number);
            gameDataManager.gameData.PayGuildCoin(storeInfo.guild_coin * number);
            gameDataManager.gameData.PayTrophy(storeInfo.trophy_elementary * number, storeInfo.trophy_intermediate * number, storeInfo.trophy_advanced * number, storeInfo.trophy_legendary * number);
            
            //加上获取数量
            int getNumber = 1;
            if (storeInfo.get_number != 0)
            {
                getNumber = storeInfo.get_number;
            }
            gameDataManager.gameData.AddBuildNumber(buildItemData.id, number * getNumber);
            RefreshUI();
            string  toastStr = string.Format(GameCommonInfo.GetUITextById(1010), buildItemData.name+"x"+ (number * getNumber));
            toastManager.ToastHint(ivIcon.sprite, toastStr);
        }
        else 
        {

            //检测金钱
            if (!gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l , storeInfo.price_m , storeInfo.price_s ))
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
                return;
            }
            if (!gameDataManager.gameData.HasEnoughGuildCoin(storeInfo.guild_coin ))
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1012));
                return;
            }
            if (!gameDataManager.gameData.HasEnoughTrophy(storeInfo.trophy_elementary , storeInfo.trophy_intermediate , storeInfo.trophy_advanced , storeInfo.trophy_legendary ))
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1021));
                return;
            }
            gameDataManager.gameData.PayMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
            gameDataManager.gameData.PayGuildCoin(storeInfo.guild_coin );
            gameDataManager.gameData.PayTrophy(storeInfo.trophy_elementary , storeInfo.trophy_intermediate , storeInfo.trophy_advanced , storeInfo.trophy_legendary );

            string toastStr;
            if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
            {
                InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
                innBuildData.buildLevel = int.Parse(storeInfo.mark);
                innBuildData.buildInnWidth = storeInfo.mark_x;
                innBuildData.buildInnHeight = storeInfo.mark_y;
                //设置修建天数
                List<TimeBean> listBuildDay = new List<TimeBean>();
                listBuildDay.Add(gameTimeHandler.GetAfterDay(1));
                innBuildData.listBuildDay = listBuildDay;

                GetUIComponent<UITownCarpenter>().RefreshUI();
                toastStr = string.Format(GameCommonInfo.GetUITextById(1011), storeInfo.name);
            }
            else
            {
                //加上获取数量
                int getNumber = 1;
                if (storeInfo.get_number != 0)
                {
                    getNumber = storeInfo.get_number;
                }
                gameDataManager.gameData.AddBuildNumber(buildItemData.id, 1 * getNumber);

                RefreshUI();
                toastStr = string.Format(GameCommonInfo.GetUITextById(1010), buildItemData.name+"x"+ 1 * getNumber);
            }
            toastManager.ToastHint(ivIcon.sprite, toastStr);
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion
}