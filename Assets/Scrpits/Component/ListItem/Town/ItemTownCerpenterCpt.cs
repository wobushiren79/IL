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

    public UIPopupPromptButton infoPromptPopup;


    public override void RefreshUI()
    {
        SetOwn((StoreForCarpenterTypeEnum)storeInfo.store_goods_type);
    }

    public void SetData(StoreInfoBean itemData)
    {
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
            buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(itemData.mark_id);
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
        if (objAttribute == null)
            return;
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            objAttribute.gameObject.SetActive(false);
        }
        else
        {
            objAttribute.gameObject.SetActive(true);
        }
        if (tvAttribute != null)
            tvAttribute.text = TextHandler.Instance.manager.GetTextById(10) + "+" + aesthetics;
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="mark"></param>
    /// <param name="markId"></param>
    public void SetIcon(StoreForCarpenterTypeEnum type, BuildItemBean buildItem, StoreInfoBean storeInfo)
    {
        Sprite spIcon = null;
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            spIcon = IconHandler.Instance.GetIconSpriteByName(storeInfo.icon_key);
        }
        else
        {
            spIcon = BuildItemTypeEnumTools.GetBuildItemSprite(buildItem);
        }

        if (ivIcon != null && spIcon != null)
            ivIcon.sprite = spIcon;
    }



    /// <summary>
    /// 设置拥有数量
    /// </summary>
    public void SetOwn(StoreForCarpenterTypeEnum type)
    {
        if (objOwn == null)
            return;
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            objOwn.gameObject.SetActive(false);
        }
        else
        {
            objOwn.gameObject.SetActive(true);
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            if (tvOwn == null)
                return;
            tvOwn.text = (TextHandler.Instance.manager.GetTextById(4001) + "\n" + gameData.GetBuildNumber(storeInfo.mark_id));
        }
    }

    /// <summary>
    /// 购买确认
    /// </summary>
    public override void OnClickSubmitBuy()
    {
        base.OnClickSubmitBuy();

        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();

        InnBuildBean innBuildData = gameData.GetInnBuildData();
        if (storeInfo == null)
            return;

        //检测是否正在修建客栈
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion && innBuildData.listBuildDay.Count != 0)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1019));
            return;
        }
        //检测金钱
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion && !gameData.HasEnoughMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s))
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
            return;
        }
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
        {
            //生成普通的对话框
            DialogBean dialogBean = new DialogBean();
            if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
            {
                //如果是庭院
                if (storeInfo.mark_type == 3)
                {
                    dialogBean.content = string.Format(TextHandler.Instance.manager.GetTextById(3016));
                }
                else
                {
                    dialogBean.content = string.Format(TextHandler.Instance.manager.GetTextById(3010), 1 + "");
                }
            }
            else
                dialogBean.content = string.Format(TextHandler.Instance.manager.GetTextById(3002), buildItemData.name);
            dialogBean.dialogType = DialogEnum.Normal;
            dialogBean.callBack = this;
            UIHandler.Instance.ShowDialog<DialogView>(dialogBean);
        }
        else
        {
            DialogBean dialogBean = new DialogBean();
            dialogBean.dialogType = DialogEnum.PickForNumber;
            dialogBean.callBack = this;
            PickForNumberDialogView dialogView = UIHandler.Instance.ShowDialog<PickForNumberDialogView>(dialogBean);
            dialogView.SetData(ivIcon.sprite, 999);
        }

    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (dialogView as PickForNumberDialogView)
        {
            PickForNumberDialogView pickForNumberDialog = dialogView as PickForNumberDialogView;
            long number= pickForNumberDialog.GetPickNumber();
            //检测金钱
            if (!gameData.HasEnoughMoney(storeInfo.price_l* number, storeInfo.price_m* number, storeInfo.price_s* number))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
                return;
            }
            if (!gameData.HasEnoughGuildCoin(storeInfo.guild_coin * number))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1012));
                return;
            }
            if (!gameData.HasEnoughTrophy(storeInfo.trophy_elementary * number, storeInfo.trophy_intermediate * number, storeInfo.trophy_advanced * number, storeInfo.trophy_legendary * number))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1021));
                return;
            }
            gameData.PayMoney(storeInfo.price_l * number, storeInfo.price_m * number, storeInfo.price_s * number);
            gameData.PayGuildCoin(storeInfo.guild_coin * number);
            gameData.PayTrophy(storeInfo.trophy_elementary * number, storeInfo.trophy_intermediate * number, storeInfo.trophy_advanced * number, storeInfo.trophy_legendary * number);
            
            //加上获取数量
            int getNumber = 1;
            if (storeInfo.get_number != 0)
            {
                getNumber = storeInfo.get_number;
            }
            gameData.AddBuildNumber(buildItemData.id, number * getNumber);
            RefreshUI();
            string  toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1010), buildItemData.name+"x"+ (number * getNumber));
            UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, toastStr);
        }
        else 
        {

            //检测金钱
            if (!gameData.HasEnoughMoney(storeInfo.price_l , storeInfo.price_m , storeInfo.price_s ))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
                return;
            }
            if (!gameData.HasEnoughGuildCoin(storeInfo.guild_coin ))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1012));
                return;
            }
            if (!gameData.HasEnoughTrophy(storeInfo.trophy_elementary , storeInfo.trophy_intermediate , storeInfo.trophy_advanced , storeInfo.trophy_legendary ))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1021));
                return;
            }
            gameData.PayMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
            gameData.PayGuildCoin(storeInfo.guild_coin );
            gameData.PayTrophy(storeInfo.trophy_elementary , storeInfo.trophy_intermediate , storeInfo.trophy_advanced , storeInfo.trophy_legendary );

            string toastStr;
            if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
            {
                InnBuildBean innBuildData = gameData.GetInnBuildData();
                InnCourtyardBean innCourtyardData = gameData.GetInnCourtyardData();
                if (storeInfo.mark_type == 1)
                {
                    //1楼扩建
                    innBuildData.buildLevel = int.Parse(storeInfo.mark);
                    innBuildData.buildInnWidth = storeInfo.mark_x;
                    innBuildData.buildInnHeight = storeInfo.mark_y;
                }
                else if (storeInfo.mark_type == 2)
                {
                    //2楼扩建
                    innBuildData.buildSecondLevel = int.Parse(storeInfo.mark);
                    innBuildData.buildInnSecondWidth = storeInfo.mark_x;
                    innBuildData.buildInnSecondHeight = storeInfo.mark_y;
                }
                else if (storeInfo.mark_type == 3)
                {
                    //庭院扩建
                    innCourtyardData.courtyardLevel = int.Parse(storeInfo.mark);
                }

                if (storeInfo.mark_type != 3)
                {
                    //设置修建天数
                    List<TimeBean> listBuildDay = new List<TimeBean>();
                    listBuildDay.Add(GameTimeHandler.Instance.GetAfterDay(1));
                    innBuildData.listBuildDay = listBuildDay;
                }

                GetUIComponent<UITownCarpenter>().RefreshUI();
                toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1011), storeInfo.name);
            }
            else
            {
                //加上获取数量
                int getNumber = 1;
                if (storeInfo.get_number != 0)
                {
                    getNumber = storeInfo.get_number;
                }
                gameData.AddBuildNumber(buildItemData.id, 1 * getNumber);

                RefreshUI();
                toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1010), buildItemData.name+"x"+ 1 * getNumber);
            }
            UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, toastStr);
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion
}