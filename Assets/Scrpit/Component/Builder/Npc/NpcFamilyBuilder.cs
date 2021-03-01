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
        FamilyTypeEnum familyType = characterForFamily.GetFamilyType();
        if (familyType == FamilyTypeEnum.Daughter || familyType == FamilyTypeEnum.Son)
        {
            //如果是女儿或者儿子 需要3年后才能移动
            GameDataBean gameData= GameDataHandler.Instance.manager.GetGameData();
            if (!characterForFamily.CheckIsGrowUp(gameData.gameTime))
            {
                return;
            }
        }
        GameObject objFamily = BuildNpc(objNormalModel, characterForFamily, Vector3.zero);
        NpcAIFamilyCpt npcAIFamily = objFamily.GetComponent<NpcAIFamilyCpt>();
        npcAIFamily.SetIntent(NpcAIFamilyCpt.FamilyIntentEnum.Idle);
    }

}