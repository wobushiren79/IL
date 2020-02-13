using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class NpcEventBuilder : NpcNormalBuilder, IBaseObserver
{
    //捣乱者模型
    public GameObject objRascalModel;
    //团队模型
    public GameObject objGuestTeamModel;
    //好友模型
    public GameObject objFriendModel;
    //当天已经生成过的NPCID
    public List<long> listExistNpcId = new List<long>();

    private void Start()
    {
        gameTimeHandler.AddObserver(this);
    }

    /// <summary>
    /// 开始事件
    /// </summary>
    public void StartEvent()
    {
        //TODO 各种事件的完善
        //RascalEvent();
    }

    /// <summary>
    /// 团队事件
    /// </summary>
    public void TeamEvent()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        List<CharacterBean> listTeam = npcInfoManager.GetCharacterDataByType(NPCTypeEnum.GuestTeam);
        CharacterBean randomCharacterData = RandomUtil.GetRandomDataByList(listTeam);
        BuildGuestTeam(randomCharacterData, npcPosition);
    }

    public void TeamEvent(long teamNpcId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        CharacterBean characterData = npcInfoManager.GetCharacterDataById(teamNpcId);
        BuildGuestTeam(characterData, npcPosition);
    }

    /// <summary>
    /// 好友事件
    /// </summary>
    public void FriendsEvent()
    {
        List<CharacterFavorabilityBean> listFavorabilityData = gameDataManager.gameData.listCharacterFavorability;
        //获取小镇角色
        List<CharacterFavorabilityBean> listTownFavorabilityData = new List<CharacterFavorabilityBean>();
        foreach (CharacterFavorabilityBean itemFavorability in listFavorabilityData)
        {
            CharacterBean characterData = npcInfoManager.GetCharacterDataById(itemFavorability.characterId);
            if (characterData == null)
                continue;

            if (characterData.npcInfoData.npc_type == (int)NPCTypeEnum.Town
                //生成的NPC中不包含这个角色
                && !listExistNpcId.Contains(characterData.npcInfoData.id))
            {
                listTownFavorabilityData.Add(itemFavorability);
            }
        }
        //符合条件的NPC中随机取一人
        CharacterFavorabilityBean randomFavorabilityData = RandomUtil.GetRandomDataByList(listTownFavorabilityData);
        if (randomFavorabilityData == null)
            return;
        CharacterBean randomCharacterData = npcInfoManager.GetCharacterDataById(randomFavorabilityData.characterId);
        Vector3 npcPosition = GetRandomStartPosition();
        BuildTownFriends(randomCharacterData, randomFavorabilityData, npcPosition);
    }

    public void FriendsEvent(long npcId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        CharacterBean characterData = npcInfoManager.GetCharacterDataById(npcId);
        CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(characterData.npcInfoData.id);
        BuildTownFriends(characterData, characterFavorability, npcPosition);
    }

    /// <summary>
    /// 恶棍事件
    /// </summary>
    public void RascalEvent()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        List<CharacterBean> listData = npcInfoManager.GetCharacterDataByType(21);
        BuildRascal(RandomUtil.GetRandomDataByList(listData), npcPosition);
    }

    /// <summary>
    /// 创建恶棍
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="npcPosition"></param>
    public void BuildRascal(CharacterBean characterData, Vector3 npcPosition)
    {
        //如果大于构建上线则不再创建新NPC
        if (objContainer.transform.childCount > buildMaxNumber)
            return;
        //生成NPC
        GameObject npcObj = BuildNpc(objRascalModel, characterData, npcPosition);
        //设置意图
        NpcAIRascalCpt rascalCpt = npcObj.GetComponent<NpcAIRascalCpt>();
        CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(long.Parse(characterData.baseInfo.characterId));
        rascalCpt.SetFavorabilityData(characterFavorability);
        rascalCpt.StartEvil();
    }

    /// <summary>
    /// 创建团队顾客
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="npcPosition"></param>
    public void BuildGuestTeam(CharacterBean characterData, Vector3 npcPosition)
    {
        List<NpcShowConditionBean> listConditionData = NpcShowConditionTools.GetListConditionData(characterData.npcInfoData.condition);
        int npcNumber = 1;
        MenuOwnBean loveMenu = null;
        string teamId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Color teamColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        bool isWant = IsWantEat();
        foreach (NpcShowConditionBean itemCondition in listConditionData)
        {
            NpcShowConditionTools.GetConditionDetails(itemCondition);
            switch (itemCondition.dataType)
            {
                case NpcShowConditionEnum.NpcNumber:
                    npcNumber = UnityEngine.Random.Range(2, itemCondition.npcNumber + 1);
                    break;
            }
        }
        //判断是否有自己喜欢的菜
        List<long> loveMenus = characterData.npcInfoData.GetLoveMenus();
        if (gameDataManager.gameData.CheckHasLoveMenus(loveMenus, out List<MenuOwnBean> ownLoveMenus))
        {
            //随机获取一个喜欢的菜
            loveMenu = RandomUtil.GetRandomDataByList(ownLoveMenus);
        }

        for (int i = 0; i < npcNumber; i++)
        {
            //随机生成身体数据
            // CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            GameObject npcObj = Instantiate(objContainer, objGuestTeamModel);

            npcObj.transform.localScale = new Vector3(1, 1);
            npcObj.transform.position = npcPosition;

            BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
            baseNpcAI.SetCharacterData(characterData);
            baseNpcAI.AddStatusIconForGuestTeam(teamColor);

            NpcAICustomerForGuestTeamCpt customerAI = baseNpcAI.GetComponent<NpcAICustomerForGuestTeamCpt>();
            customerAI.SetTeamId(teamId);
            if (isWant && loveMenu != null)
            {
                customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
                customerAI.SetMenu(loveMenu);
            }
            else
            {
                customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Walk);
            }
        }
    }

    /// <summary>
    /// 创建小镇好友
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="npcPosition"></param>
    public void BuildTownFriends(CharacterBean characterData, CharacterFavorabilityBean characterFavorability, Vector3 npcPosition)
    {
        //随机生成身体数据
        GameObject npcObj = Instantiate(objContainer, objFriendModel);

        npcObj.transform.localScale = new Vector3(1, 1);
        npcObj.transform.position = npcPosition;

        BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
        baseNpcAI.SetCharacterData(characterData);
        baseNpcAI.SetFavorabilityData(characterFavorability);
        baseNpcAI.AddStatusIconForFriend();

        NpcAICostomerForFriendCpt customerAI = baseNpcAI.GetComponent<NpcAICostomerForFriendCpt>();
        customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);

    }

    /// <summary>
    /// 通过团队ID获取团队成员
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public List<NpcAICustomerForGuestTeamCpt> GetGuestTeamByTeamId(string teamId)
    {
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = new List<NpcAICustomerForGuestTeamCpt>();
        NpcAICustomerForGuestTeamCpt[] arrayTeam = objContainer.GetComponentsInChildren<NpcAICustomerForGuestTeamCpt>();
        foreach (NpcAICustomerForGuestTeamCpt itemData in arrayTeam)
        {
            if (itemData.teamId.EndsWith(teamId))
            {
                listTeamMember.Add(itemData);
            }
        }
        return listTeamMember;
    }

    /// <summary>
    ///  计算是否想要吃饭
    /// </summary>
    /// <returns></returns>
    private bool IsWantEat()
    {
        //想要吃饭概率
        float eatProbability = UnityEngine.Random.Range(0f, 1f);
        float rateWant = gameDataManager.gameData.GetInnAttributesData().CalculationCustomerWantRate();
        //设定是否吃饭
        if (eatProbability <= rateWant)
        {
            return true;
        }
        return false;
    }

    #region 时间回调通知
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : UnityEngine.Object
    {
        if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            ClearNpc();
            listExistNpcId.Clear();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {
            int hour = Convert.ToInt32(obj[0]);
            if (hour > 9 && hour <= 20)
            {
                StartEvent();
            }
        }
    }
    #endregion
}