using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildStairsCpt : BaseBuildItemCpt
{
    public GameObject objUpBox;
    public GameObject objDownBox;

    public GameObject objStairs;

    public int layer = 2;
    public string remarkId = "";


    public override void SetData(BuildItemBean buildItemData, ItemBean itemData = null)
    {
        base.SetData(buildItemData);
        SetLayer(2);
    }

    public void SetRemarkId(string remarkId)
    {
        this.remarkId = remarkId;
    }

    public void SetLayer(int layer)
    {
        this.layer = layer;
        List<string> listIcon = buildItemData.GetIconList();
        objUpBox.SetActive(false);
        objDownBox.SetActive(false);
        if (layer == 1)
        {
            Sprite spFirst = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_0");
            SetSprite(spFirst, spFirst, spFirst, spFirst);
            objUpBox.SetActive(true);
        }
        else if (layer == 2)
        {
            Sprite spSecond= IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_1");
            SetSprite(spSecond, spSecond, spSecond, spSecond);
            objDownBox.SetActive(true);
        }
    }

    public Vector3 GetStairsPosition()
    {
        return objStairs.transform.position;
    }
}