using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcImportantBuilder : BaseMonoBehaviour
{
    public GameObject objNpcModel;
    public GameObject objNpcContainer;

    public List<NpcAIImportantCpt> listTownNpc = new List<NpcAIImportantCpt>();
    public List<NpcAIImportantCpt> listRecruitTownNpc = new List<NpcAIImportantCpt>();
    public List<NpcAIImportantCpt> listSpecialTownNpc = new List<NpcAIImportantCpt>();
    public List<NpcAIImportantCpt> listMountainNpc = new List<NpcAIImportantCpt>();


    public void BuildImportantForTown()
    {
        CptUtil.RemoveChildsByActive(objNpcContainer);
        listTownNpc.Clear();
        listRecruitTownNpc.Clear();
        listSpecialTownNpc.Clear();
        //创建小镇居民
        List<CharacterBean> listTownCharacter = NpcInfoHandler.Instance.manager.GetCharacterDataByType((int)NpcTypeEnum.Town);
        foreach (CharacterBean itemData in listTownCharacter)
        {
            NpcAIImportantCpt itemNpc = BuildNpc(itemData);
            if (itemNpc != null)
                listTownNpc.Add(itemNpc);
        }
        //创建特殊NPC
        List<CharacterBean> listSpecialCharacter = NpcInfoHandler.Instance.manager.GetCharacterDataByType((int)NpcTypeEnum.Special);
        foreach (CharacterBean itemData in listSpecialCharacter)
        {
            NpcAIImportantCpt itemNpc = BuildNpc(itemData);
            if (itemNpc != null)
                listSpecialTownNpc.Add(itemNpc);
        }
        //创建小镇招募居民
        List<CharacterBean> listRecruitTownCharacter =NpcInfoHandler.Instance.manager.GetCharacterDataByType((int)NpcTypeEnum.RecruitTown);
        foreach (CharacterBean itemData in listRecruitTownCharacter)
        {
            NpcAIImportantCpt itemNpc = BuildNpc(itemData);
            if (itemNpc != null)
                listRecruitTownNpc.Add(itemNpc);
        }
    }

    public void BuildImportantForMountain()
    {
        CptUtil.RemoveChildsByActive(objNpcContainer);
        listMountainNpc.Clear();
        //创建山顶NPC
        List<CharacterBean> listMountainCharacter =NpcInfoHandler.Instance.manager.GetCharacterDataByType((int)NpcTypeEnum.Mountain);
        for (int i = 0; i < listMountainCharacter.Count; i++)
        {
            CharacterBean itemCharacterData = listMountainCharacter[i];
            NpcAIImportantCpt itemNpc = BuildNpc(itemCharacterData);
            if (itemNpc != null)
                listMountainNpc.Add(itemNpc);
        }
    }

    private NpcAIImportantCpt BuildNpc(CharacterBean characterData)
    {
        GameObject npcObj = null;
        try
        {
            if (objNpcModel == null || objNpcContainer == null)
                return null;
            //检测是否已经招募
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            if (gameData.CheckHasWorker(characterData.baseInfo.characterId))
            {
                return null;
            }

            if (gameData.gameTime.year == 0
                && gameData.gameTime.month == 0
                && gameData.gameTime.day == 0)
            {
                //如果是测试 这默认生成所有NPC
            }
            else
            {
                //检测是否满足出现条件
                if (!CheckUtil.StringIsNull(characterData.npcInfoData.condition) && !ShowConditionTools.CheckIsMeetAllCondition(gameData, characterData.npcInfoData.condition))
                {
                    return null;
                }
            }

            npcObj = Instantiate(objNpcContainer, objNpcModel);
            npcObj.transform.position = new Vector3(characterData.npcInfoData.position_x, characterData.npcInfoData.position_y);
            npcObj.transform.localScale = new Vector3(1, 1);
            NpcAIImportantCpt aiCpt = npcObj.GetComponent<NpcAIImportantCpt>();
            aiCpt.SetCharacterData(characterData);

            if (characterData.npcInfoData.GetNpcType() == NpcTypeEnum.Special || characterData.npcInfoData.GetNpcType() == NpcTypeEnum.RecruitTown)
            {
                if (!GameCommonInfo.DailyLimitData.CheckIsTalkNpc(characterData.npcInfoData.id))
                {
                    if (characterData.npcInfoData.GetTalkTypes().Contains(NpcTalkTypeEnum.OneTalk)
                        || characterData.npcInfoData.GetTalkTypes().Contains(NpcTalkTypeEnum.Recruit))
                    {
                        aiCpt.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Doubt, -1);
                    }
                }
            }

            //如果没有对话选项则不能互动
            BoxCollider2D talkBox = npcObj.GetComponent<BoxCollider2D>();
            if (talkBox != null)
            {
                if (CheckUtil.StringIsNull(characterData.npcInfoData.talk_types))
                {
                    talkBox.enabled = false;
                }
            }
            return aiCpt;
        }
        catch
        {
            if (npcObj != null)
            {
                Destroy(npcObj);
            }
            return null;
        }
    }

    public void HideNpc()
    {
        foreach (NpcAIImportantCpt itemNpc in listTownNpc)
        {
            itemNpc.gameObject.SetActive(false);
        }
        foreach (NpcAIImportantCpt itemNpc in listRecruitTownNpc)
        {
            itemNpc.gameObject.SetActive(false);
        }
        foreach (NpcAIImportantCpt itemNpc in listSpecialTownNpc)
        {
            itemNpc.gameObject.SetActive(false);
        }
        foreach (NpcAIImportantCpt itemNpc in listMountainNpc)
        {
            itemNpc.gameObject.SetActive(false);
        }
    }

    public void ShowNpc()
    {
        foreach (NpcAIImportantCpt itemNpc in listTownNpc)
        {
            itemNpc.gameObject.SetActive(true);
        }
        foreach (NpcAIImportantCpt itemNpc in listRecruitTownNpc)
        {
            itemNpc.gameObject.SetActive(true);
        }
        foreach (NpcAIImportantCpt itemNpc in listSpecialTownNpc)
        {
            itemNpc.gameObject.SetActive(true);
        }
        foreach (NpcAIImportantCpt itemNpc in listMountainNpc)
        {
            itemNpc.gameObject.SetActive(true);
        }
    }
}