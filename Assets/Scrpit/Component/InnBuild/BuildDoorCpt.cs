using UnityEngine;
using UnityEditor;

public class BuildDoorCpt : BaseBuildItemCpt
{
    public GameObject entranceObj;

    public TextMesh tvInnName;
    public TextMesh tvInnNameShadow;

    protected GameDataManager gameDataManager;
    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);


    }

    /// <summary>
    /// 获取入口位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEntrancePosition()
    {
        return entranceObj.transform.position;
    }

    public override void SetData(BuildItemBean buildItemData)
    {
        base.SetData(buildItemData);
        SetInnName(gameDataManager.gameData.GetInnAttributesData().innName);
    }

    public void SetInnName(string name)
    {
        if (name.Length > 6)
        {
            name = name.Substring(0, 6);
        }
        if (tvInnName != null)
            tvInnName.text = name;
        if (tvInnNameShadow != null)
            tvInnNameShadow.text = name;
    }
}