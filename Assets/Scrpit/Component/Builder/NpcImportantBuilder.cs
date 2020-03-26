using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcImportantBuilder : BaseMonoBehaviour
{
    public GameObject objNpcModel;
    public GameObject objNpcContainer;
    //NPC数据管理
    protected NpcInfoManager npcInfoManager;
    protected GameDataManager gameDataManager;

    public List<NpcAIImportantCpt> listTownNpc = new List<NpcAIImportantCpt>();
    public List<NpcAIImportantCpt> listRecruitTownNpc = new List<NpcAIImportantCpt>();

    public void Awake()
    {
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    public void BuildImportant()
    {
        //创建小镇居民
        List<CharacterBean> listTownCharacter = npcInfoManager.GetCharacterDataByType((int)NpcTypeEnum.Town);
        foreach (CharacterBean itemData in listTownCharacter)
        {
            NpcAIImportantCpt itemNpc = BuildNpc(itemData);
            if (itemNpc != null)
                listTownNpc.Add(itemNpc);
        }
        //创建小镇招募居民
        List<CharacterBean> listRecruitTownCharacter = npcInfoManager.GetCharacterDataByType((int)NpcTypeEnum.RecruitTown);
        foreach (CharacterBean itemData in listRecruitTownCharacter)
        {
            NpcAIImportantCpt itemNpc = BuildNpc(itemData);
            if (itemNpc != null)
                listRecruitTownNpc.Add(itemNpc);
        }
    }

    private NpcAIImportantCpt BuildNpc(CharacterBean characterData)
    {
        if (objNpcModel == null || objNpcContainer == null)
            return null;
        if (!CheckUtil.StringIsNull(characterData.npcInfoData.condition) && !ShowConditionTools.CheckIsMeetAllCondition(gameDataManager.gameData, characterData.npcInfoData.condition))
        {
            return null;
        }
        GameObject npcObj = Instantiate(objNpcContainer, objNpcModel);
        npcObj.transform.position = new Vector3(characterData.npcInfoData.position_x, characterData.npcInfoData.position_y);
        npcObj.transform.localScale = new Vector3(1, 1);
        NpcAIImportantCpt aiCpt = npcObj.GetComponent<NpcAIImportantCpt>();
        aiCpt.SetCharacterData(characterData);
        return aiCpt;
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
    }
}