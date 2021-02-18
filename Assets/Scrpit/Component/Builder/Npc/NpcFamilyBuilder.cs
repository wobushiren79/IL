using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFamilyBuilder : NpcNormalBuilder
{
    /// <summary>
    /// 建造家族成员
    /// </summary>
    public void BuildFamily()
    {
        //获取家族数据
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData = gameData.GetFamilyData();
        List<CharacterForFamilyBean> listFamilyCharacterData = familyData.GetAllFamilyData();
        for (int i = 0; i < listFamilyCharacterData.Count; i++)
        {
            CharacterForFamilyBean characterForFamily = listFamilyCharacterData[i];
            CreateFamilyCharacter(characterForFamily);
        }
    }

    /// <summary>
    /// 创建家族成员
    /// </summary>
    /// <param name="characterForFamily"></param>
    public void CreateFamilyCharacter(CharacterForFamilyBean characterForFamily)
    {
        BuildNpc(objNormalModel, characterForFamily,Vector3.zero);
    }

}