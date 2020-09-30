using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIGameMainForHint : BaseUIChildComponent<UIGameMain>
{
    public GameObject objHintContainer;
    public GameObject objMenuResearchModel;

    public List<ItemGameMainHintForResearchCpt> listResearch = new List<ItemGameMainHintForResearchCpt>();

    public override void Close()
    {
        ClearData();
        base.Close();
    }

    public bool SetData(List<MenuOwnBean> listMenu)
    {
        if (listResearch == null)
            listResearch = new List<ItemGameMainHintForResearchCpt>();
        foreach (MenuOwnBean itemData in listMenu)
        {
            bool hasData = false;
            for (int i = 0; i < listResearch.Count; i++)
            {
                ItemGameMainHintForResearchCpt itemCpt = listResearch[i];
                MenuOwnBean menuOwn = itemCpt.GetMenuData();
                if (menuOwn == null)
                    continue;
                if (menuOwn == itemData)
                {
                    hasData = true;
                    itemCpt.RefreshData();
                    if (menuOwn.GetMenuStatus() != ResearchStatusEnum.Researching)
                    {
                        listResearch.Remove(itemCpt);
                        Destroy(itemCpt.gameObject);
                        i--;
                    }
                }
            }
            if (!hasData)
            {
                CreateItemForResearch(itemData,null);
            }
        }
        if (listResearch.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SetData(List<BuildBedBean> listBed)
    {
        if (listResearch == null)
            listResearch = new List<ItemGameMainHintForResearchCpt>();
        foreach (BuildBedBean itemData in listBed)
        {
            bool hasData = false;
            for (int i = 0; i < listResearch.Count; i++)
            {
                ItemGameMainHintForResearchCpt itemCpt = listResearch[i];
                BuildBedBean buildBed = itemCpt.GetBedData();
                if (buildBed == null)
                    continue;
                if (buildBed == itemData)
                {
                    hasData = true;
                    itemCpt.RefreshData();
                    if (buildBed.GetBedStatus() != ResearchStatusEnum.Researching)
                    {
                        listResearch.Remove(itemCpt);
                        Destroy(itemCpt.gameObject);
                        i--;
                    }
                }
            }
            if (!hasData)
            {
                CreateItemForResearch(null,itemData);
            }
        }
        if (listResearch.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 创建研究提示Item
    /// </summary>
    /// <param name="menuOwn"></param>
    public void CreateItemForResearch(MenuOwnBean menuOwn,BuildBedBean buildBedData)
    {
        GameObject objItem = Instantiate(objHintContainer, objMenuResearchModel);
        ItemGameMainHintForResearchCpt itemCpt = objItem.GetComponent<ItemGameMainHintForResearchCpt>();
        if(menuOwn!=null)
            itemCpt.SetData(menuOwn);
        if (buildBedData != null)
            itemCpt.SetData(buildBedData);
        listResearch.Add(itemCpt);
    }

    public void ClearData()
    {
        if (listResearch != null)
            listResearch.Clear();
        CptUtil.RemoveChildsByActive(objHintContainer);
    }


}