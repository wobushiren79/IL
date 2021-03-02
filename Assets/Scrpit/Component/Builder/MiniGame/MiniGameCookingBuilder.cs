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

    //所有的通告版
    public List<MiniGameCookingCallBoardCpt> listCallBoard = new List<MiniGameCookingCallBoardCpt>();
    //所有的评审桌子
    public List<MiniGameCookingAuditTableCpt> listAuditTable = new List<MiniGameCookingAuditTableCpt>();
    //所有的灶台
    public List<MiniGameCookingStoveCpt> listStove = new List<MiniGameCookingStoveCpt>();

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
    public List<NpcAIMiniGameCookingCpt> GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum npcType)
    {
        switch (npcType)
        {
            case NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player:
                List<NpcAIMiniGameCookingCpt> listData = new List<NpcAIMiniGameCookingCpt>();
                listData.AddRange(listEnemyCharacter);
                listData.Insert(Random.Range(0, listEnemyCharacter.Count+1), userCharacter);
                return listData;
            case NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer:
                return listAuditerCharacter;
            case NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Compere:
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
            CreateCharacterList(listEnemyData, listEnemyStartPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player);
        if (!CheckUtil.ListIsNull(listAuditerData))
            CreateCharacterList(listAuditerData, listAuditerStartPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer);
        if (!CheckUtil.ListIsNull(listCompereData))
            CreateCharacterList(listCompereData, listCompereStartPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Compere);
    }

    public void CreateUserCharacter(MiniGameCharacterBean userData, Vector3 startPosition)
    {
        userCharacter = CreateCharacter(userData, startPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player);
        userCharacter.characterMoveCpt.SetMoveSpeed(5);
    }

    public void CreateCharacterList(List<MiniGameCharacterBean> listCharacterData, List<Vector3> listCharacterPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum npcType)
    {
        if (CheckUtil.ListIsNull(listCharacterData))
            return;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            MiniGameCharacterBean itemCharacterData = listCharacterData[i];

            NpcAIMiniGameCookingCpt npcCpt = CreateCharacter(itemCharacterData, listCharacterPosition[i], npcType);
            switch (npcType)
            {
                case NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player:
                    listEnemyCharacter.Add(npcCpt);
                    npcCpt.characterMoveCpt.SetMoveSpeed(5);
                    break;
                case NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer:
                    listAuditerCharacter.Add(npcCpt);
                    break;
                case NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Compere:
                    listCompereCharacter.Add(npcCpt);
                    break;
            }
        }
    }

    private NpcAIMiniGameCookingCpt CreateCharacter(MiniGameCharacterBean characterGameData, Vector3 startPosition, NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum npcType)
    {
        GameObject objCharacter = Instantiate(objNpcContainer, objNpcModel, startPosition);
        NpcAIMiniGameCookingCpt npcCpt = objCharacter.GetComponent<NpcAIMiniGameCookingCpt>();
        npcCpt.startPosition = startPosition;
        npcCpt.SetNpcType(npcType);
        npcCpt.SetData((MiniGameCharacterForCookingBean)characterGameData);
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
    
    /// <summary>
    /// 设置通告板
    /// </summary>
    /// <param name="listCallBoard"></param>
    public void SetListCallBoard(List<MiniGameCookingCallBoardCpt> listCallBoard)
    {
        this.listCallBoard = listCallBoard;
    }
    /// <summary>
    /// 获取通告板
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCookingCallBoardCpt> GetListCallBoard()
    {
        return listCallBoard;
    }

    /// <summary>
    /// 设置审核桌子
    /// </summary>
    /// <param name="listAuditTable"></param>
    public void SetListAuditTable(List<MiniGameCookingAuditTableCpt> listAuditTable)
    {
        this.listAuditTable = listAuditTable;
    }

    /// <summary>
    /// 获取审核桌子
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCookingAuditTableCpt> GetListAuditTable()
    {
        return listAuditTable;
    }

    /// <summary>
    /// 设置桌子列表
    /// </summary>
    /// <param name="listStove"></param>
    public void SetListStove(List<MiniGameCookingStoveCpt> listStove)
    {
        this.listStove = listStove;
    }
    /// <summary>
    /// 获取灶台
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCookingStoveCpt> GetListStove()
    {
        return listStove;
    }

    public override void DestroyAll()
    {
        base.DestroyAll();
        listEnemyCharacter.Clear();
        listAuditerCharacter.Clear();
        listCompereCharacter.Clear();
        listCallBoard.Clear();
        listAuditTable.Clear();
        listStove.Clear();
        CptUtil.RemoveChildsByActive(objNpcContainer);
    }
}