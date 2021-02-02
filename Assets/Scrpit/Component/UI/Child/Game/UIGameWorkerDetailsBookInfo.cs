﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIGameWorkerDetailsBookInfo : BaseUIChildComponent<UIGameWorkerDetails>
{
    public Text tvNull;
    public GameObject objBookItemContainer;
    public GameObject objBookItemModel;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="listSkill"></param>
    public void SetData(List<long> listBook)
    {
        List<ItemsInfoBean> listData = GameItemsHandler.Instance.manager.GetItemsByIds(listBook);
        CreateBookList(listData);
    }

    /// <summary>
    /// 创建图书列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateBookList(List<ItemsInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objBookItemContainer);
        if (CheckUtil.ListIsNull(listData))
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        else
        {
            tvNull.gameObject.SetActive(false);
        }

        foreach (ItemsInfoBean itemData in listData)
        {
            GameObject objItem = Instantiate(objBookItemContainer, objBookItemModel);
            ItemBaseTextCpt itemBaseText = objItem.GetComponent<ItemBaseTextCpt>();
            PopupItemsButton infoItemsPopup = objItem.GetComponent<PopupItemsButton>();
            Sprite spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName(itemData.icon_key);
            itemBaseText.SetData(spIcon, itemData.name, "");
            infoItemsPopup.SetData(itemData, spIcon);
        }
    }

}