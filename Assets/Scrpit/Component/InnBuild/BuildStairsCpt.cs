using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildStairsCpt : BaseBuildItemCpt
{
    protected InnBuildManager innBuildManager;

    public GameObject objUpBox;
    public GameObject objDownBox;

    public int layer = 2;

    private void Awake()
    {
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
    }

    public override void SetData(BuildItemBean buildItemData)
    {
        base.SetData(buildItemData);
        SetLayer(2);
    }

    public void SetLayer(int layer)
    {
        this.layer = layer;
        List<string> listIcon = buildItemData.GetIconList();
        objUpBox.SetActive(false);
        objDownBox.SetActive(false);
        if (layer == 1)
        {
            Sprite spFirst = innBuildManager.GetFurnitureSpriteByName(listIcon[0] + "_0");
            SetSprite(spFirst, spFirst, spFirst, spFirst);
            objUpBox.SetActive(true);
        }
        else if (layer == 2)
        {
            Sprite spSecond= innBuildManager.GetFurnitureSpriteByName(listIcon[0] + "_1");
            SetSprite(spSecond, spSecond, spSecond, spSecond);
            objDownBox.SetActive(true);
        }
    }
}