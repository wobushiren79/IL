using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIGameWorkerDetailsBookInfo : BaseUIView
{
    public Text tvNull;
    public GameObject objBookItemContainer;
    public GameObject objBookItemModel;

    public List<long> listBook = new List<long>();
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="listSkill"></param>
    public void SetData(List<long> listBook)
    {
        this.listBook = listBook;
        List<ItemsInfoBean> listData = GameItemsHandler.Instance.manager.GetItemsListByType(GeneralEnum.Book);
        CreateBookList(listData);
    }

    /// <summary>
    /// 创建图书列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateBookList(List<ItemsInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objBookItemContainer);
        if (listData.IsNull())
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        else
        {
            tvNull.gameObject.SetActive(false);
        }
        for (int i = 0; i < listData.Count; i++)
        {
            ItemsInfoBean itemData = listData[i];
            if (!listBook.Contains(itemData.id))
            {
                continue;
            }
            GameObject objItem = Instantiate(objBookItemContainer, objBookItemModel);
            ItemBaseTextCpt itemBaseText = objItem.GetComponent<ItemBaseTextCpt>();
            UIPopupItemsButton infoItemsPopup = objItem.GetComponent<UIPopupItemsButton>();
            Sprite spIcon = IconHandler.Instance.GetIconSpriteByName(itemData.icon_key);
            itemBaseText.SetData(spIcon, itemData.name, "");
            infoItemsPopup.SetData(itemData, spIcon);
        }
    }

}