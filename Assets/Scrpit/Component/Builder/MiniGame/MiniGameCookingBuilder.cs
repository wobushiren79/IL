using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCookingBuilder : BaseMiniGameBuilder
{
    public GameObject objNpcContainer;
    public GameObject objNpcModel;

    public NpcAIMiniGameCookingCpt userCharacter;
    public List<NpcAIMiniGameCookingCpt> listEnemyCharacter = new List<NpcAIMiniGameCookingCpt>();
    public List<NpcAIMiniGameCookingCpt> listAuditerCharacter = new List<NpcAIMiniGameCookingCpt>();
    public List<NpcAIMiniGameCookingCpt> listCompereCharacter = new List<NpcAIMiniGameCookingCpt>();

    /// <summary>
    /// 获取用户的角色
    /// </summary>
    /// <returns></returns>
    public NpcAIMiniGameCookingCpt GetUserCharacter()
    {
        return userCharacter;
    }

    /// <summary>
    /// 根据角色类型获取角色
    /// </summary>
    /// <param name="npcType"></param>
    /// <returns></returns>
    public List<NpcAIMiniGameCookingCpt> GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcType npcType)
    {
        switch (npcType)
        {
            case NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Player:
                List<NpcAIMiniGameCookingCpt> listData = new List<NpcAIMiniGameCookingCpt>();
                listData.Add(userCharacter);
                listData.AddRange(listAuditerCharacter);
                return listData;
            case NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Auditer:
                return listAuditerCharacter;
            case NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Compere:
                return listCompereCharacter;
        }
        return null;
    }

    /// <summary>
    /// 创建所有角色
    /// </summary>
    /// <param name="listUserData">玩家</param>
    /// <param name="listEnemyData">敌人</param>
    /// <param name="listAuditerData">评审员</param>
    /// <param name="listCompereData">主持人</param>
    public void CreateAllCharacter(
        List<MiniGameCharacterBean> listUserData, Vector3 userStartPosition,
        List<MiniGameCharacterBean> listEnemyData, List<Vector3> listEnemyStartPosition,
        List<MiniGameCharacterBean> listAuditerData, List<Vector3> listAuditerStartPosition,
        List<MiniGameCharacterBean> listCompereData, List<Vector3> listCompereStartPosition)
    {
        if (!CheckUtil.ListIsNull(listUserData))
            CreateUserCharacter(listUserData[0], userStartPosition);
        if (!CheckUtil.ListIsNull(listEnemyData))
            CreateCharacterList(listEnemyData, listEnemyStartPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Player);
        if (!CheckUtil.ListIsNull(listAuditerData))
            CreateCharacterList(listAuditerData, listAuditerStartPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Auditer);
        if (!CheckUtil.ListIsNull(listCompereData))
            CreateCharacterList(listCompereData, listCompereStartPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Compere);
    }

    public void CreateUserCharacter(MiniGameCharacterBean userData, Vector3 startPosition)
    {
        userCharacter = CreateCharacter(userData, startPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Player);
    }

    public void CreateCharacterList(List<MiniGameCharacterBean> listCharacterData, List<Vector3> listCharacterPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcType npcType)
    {
        if (CheckUtil.ListIsNull(listCharacterData))
            return;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            MiniGameCharacterBean itemCharacterData = listCharacterData[i];

            NpcAIMiniGameCookingCpt npcCpt = CreateCharacter(itemCharacterData, listCharacterPosition[i], npcType);
            switch (npcType)
            {
                case NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Player:
                    listEnemyCharacter.Add(npcCpt);
                    break;
                case NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Auditer:
                    listAuditerCharacter.Add(npcCpt);
                    break;
                case NpcAIMiniGameCookingCpt.MiniGameCookingNpcType.Compere:
                    listCompereCharacter.Add(npcCpt);
                    break;
            }
        }
    }

    private NpcAIMiniGameCookingCpt CreateCharacter(MiniGameCharacterBean characterGameData, Vector3 startPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcType npcType)
    {
        GameObject objCharacter = Instantiate(objNpcContainer, objNpcModel, startPosition);
        NpcAIMiniGameCookingCpt npcCpt = objCharacter.GetComponent<NpcAIMiniGameCookingCpt>();
        npcCpt.SetNpcType(npcType);
        npcCpt.SetData(characterGameData);
        npcCpt.OpenAI();
        return npcCpt;
    }

    /// <summary>
    ///  设置主持人是否显示
    /// </summary>
    /// <param name="value"></param>
    public void SetCompereCharacterActive(bool value)
    {
        foreach (NpcAIMiniGameCookingCpt itemCompere in  listCompereCharacter)
        {
            itemCompere.gameObject.SetActive(value);
        }
    }
}