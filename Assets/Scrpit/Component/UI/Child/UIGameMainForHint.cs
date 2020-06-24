using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIGameMainForHint : BaseUIChildComponent<UIGameMain>
{
    public GameObject objHintContainer;
    public GameObject objMenuResearchModel;

    public List<ItemGameMainHintForMenuResearchCpt> listMenuResearch;

    public override void Close()
    {
        ClearData();
        base.Close();
    }

    public bool SetData(List<MenuOwnBean> listMenu)
    {
        if (listMenuResearch == null)
            listMenuResearch = new List<ItemGameMainHintForMenuResearchCpt>();
        //判断所有的数据都已经完成研究
        bool isAllComplete = true;
        foreach (MenuOwnBean itemData in listMenu)
        {
            if(itemData.GetMenuStatus() == MenuStatusEnum.Researching)
            {
                isAllComplete = false;
            }
            bool hasData = false;
            for (int i = 0; i < listMenuResearch.Count; i++)
            {
                ItemGameMainHintForMenuResearchCpt itemCpt = listMenuResearch[i];
                if (itemCpt.menuOwn == itemData)
                {
                    hasData = true;
                    itemCpt.RefreshData();
                }
                if (itemCpt.menuOwn.GetMenuStatus() != MenuStatusEnum.Researching)
                {
                    listMenuResearch.Remove(itemCpt);
                    Destroy(itemCpt.gameObject);
                    i--;
                }
            }
            if (!hasData)
            {
                CreateItemForMenuResearch(itemData);
            }
        }
        return isAllComplete;
    }

    /// <summary>
    /// 创建研究提示Item
    /// </summary>
    /// <param name="menuOwn"></param>
    public void CreateItemForMenuResearch(MenuOwnBean menuOwn)
    {
        GameObject objItem = Instantiate(objHintContainer, objMenuResearchModel);
        ItemGameMainHintForMenuResearchCpt itemCpt = objItem.GetComponent<ItemGameMainHintForMenuResearchCpt>();
        itemCpt.SetData(menuOwn);
        listMenuResearch.Add(itemCpt);
    }

    public void ClearData()
    {
        if (listMenuResearch != null)
            listMenuResearch.Clear();
        CptUtil.RemoveChildsByActive(objHintContainer);
    }


}