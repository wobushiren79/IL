using UnityEngine;
using UnityEditor;

public class BuildDoorCpt : BaseBuildItemCpt
{
    public GameObject entranceObj;

    public TextMesh tvInnName;
    public TextMesh tvInnNameShadow;


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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        SetInnName(gameData.GetInnAttributesData().innName);
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