using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIGameMainForHint : BaseUIChildComponent<UIGameMain>
{
    public GameObject objHintContainer;
    public GameObject objMenuResearchModel;
    public GameObject objInfiniteTowersModel;

    protected List<ItemGameMainHintForResearchCpt> listResearch = new List<ItemGameMainHintForResearchCpt>();
    protected List<ItemGameMainHintForInfiniteTowersCpt> listInfiniteTowers = new List<ItemGameMainHintForInfiniteTowersCpt>();
    public override void Close()
    {
        ClearData();
        base.Close();
    }

    private void Update()
    {
        if (!CheckUtil.ListIsNull(listResearch)|| !CheckUtil.ListIsNull(listInfiniteTowers))
        {
            for (int i = 0; i < listResearch.Count; i++)
            {
                ItemGameMainHintForResearchCpt itemCpt = listResearch[i];
                MenuOwnBean menuOwn = itemCpt.GetMenuData();
                BuildBedBean bedData = itemCpt.GetBedData();
                if (menuOwn != null)
                {
                    if (menuOwn.GetMenuStatus() != ResearchStatusEnum.Researching)
                    {
                        listResearch.Remove(itemCpt);
                        Destroy(itemCpt.gameObject);
                        i--;
                        continue;
                    }
                }
                if (bedData != null)
                {
                    if (bedData.GetBedStatus() != ResearchStatusEnum.Researching)
                    {
                        listResearch.Remove(itemCpt);
                        Destroy(itemCpt.gameObject);
                        i--;
                        continue;
                    }
                }
            }
            for (int i = 0; i < listInfiniteTowers.Count; i++)
            {
                ItemGameMainHintForInfiniteTowersCpt itemCpt = listInfiniteTowers[i];
                if (itemCpt.infiniteTowersData.proForSend==-1)
                {
                    listInfiniteTowers.Remove(itemCpt);
                    Destroy(itemCpt.gameObject);
                    i--;
                    continue;
                }
            }
            CheckHasData();
        }
    }

    public void SetData(List<MenuOwnBean> listMenu)
    {
        if (listResearch == null)
            listResearch = new List<ItemGameMainHintForResearchCpt>();
        for (int f = 0; f < listMenu.Count; f++)
        {
            MenuOwnBean itemData = listMenu[f];
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
                CreateItemForResearch(itemData, null);
            }
        }
        CheckHasData();
    }
    public void SetData(List<BuildBedBean> listBed)
    {
        if (listResearch == null)
            listResearch = new List<ItemGameMainHintForResearchCpt>();
        for (int f = 0; f < listBed.Count; f++)
        {
            BuildBedBean itemData = listBed[f];
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
                CreateItemForResearch(null, itemData);
            }
        }

        CheckHasData();
    }

    public void SetData(List<UserInfiniteTowersBean> listData)
    {
        if (listInfiniteTowers == null)
            listInfiniteTowers = new List<ItemGameMainHintForInfiniteTowersCpt>();
        for (int f = 0; f < listData.Count; f++)
        {
            UserInfiniteTowersBean itemData = listData[f];
            bool hasData = false;
            for (int i = 0; i < listInfiniteTowers.Count; i++)
            {
                ItemGameMainHintForInfiniteTowersCpt itemCpt = listInfiniteTowers[i];
                if (itemCpt.infiniteTowersData == itemData)
                {
                    hasData = true;
                    itemCpt.RefreshData();
                    if (itemCpt.infiniteTowersData .proForSend==-1)
                    {
                        listInfiniteTowers.Remove(itemCpt);
                        Destroy(itemCpt.gameObject);
                        i--;
                    }
                }
            }
            if (!hasData)
            {
                CreateItemForInfiniteTowers(itemData);
            }
        }
        CheckHasData();
    }

    /// <summary>
    /// 检测是否还有数据
    /// </summary>
    public void CheckHasData()
    {
        if (listResearch.Count > 0 || listInfiniteTowers.Count > 0)
        {
            Open();
        }
        else
        {
            Close();
        }

    }

    /// <summary>
    /// 创建研究提示Item
    /// </summary>
    /// <param name="menuOwn"></param>
    public void CreateItemForResearch(MenuOwnBean menuOwn, BuildBedBean buildBedData)
    {
        GameObject objItem = Instantiate(objHintContainer, objMenuResearchModel);
        ItemGameMainHintForResearchCpt itemCpt = objItem.GetComponent<ItemGameMainHintForResearchCpt>();
        if (menuOwn != null)
            itemCpt.SetData(menuOwn);
        if (buildBedData != null)
            itemCpt.SetData(buildBedData);
        listResearch.Add(itemCpt);
    }

    /// <summary>
    /// 创建爬塔数据
    /// </summary>
    /// <param name="infiniteTowersData"></param>
    public void CreateItemForInfiniteTowers(UserInfiniteTowersBean infiniteTowersData)
    {
        GameObject objItem = Instantiate(objHintContainer, objInfiniteTowersModel);
        ItemGameMainHintForInfiniteTowersCpt itemCpt = objItem.GetComponent<ItemGameMainHintForInfiniteTowersCpt>();
        itemCpt.SetData(infiniteTowersData);
        listInfiniteTowers.Add(itemCpt);
    }

    public void ClearData()
    {
        if (listResearch != null)
            listResearch.Clear();
        if (listInfiniteTowers != null)
            listInfiniteTowers.Clear();
        CptUtil.RemoveChildsByActive(objHintContainer);
    }


}