using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcImportantBuilder : BaseMonoBehaviour
{

    public GameObject objNpcModel;
    public GameObject objNpcContainer;
    //NPC数据管理
    public NpcInfoManager npcInfoManager;


    public void BuildImportant()
    {
        //创建小镇居民
        List<CharacterBean> listTownCharacter=  npcInfoManager.GetCharacterDataByType((int)NPCTypeEnum.Town);
        foreach (CharacterBean itemData in listTownCharacter)
        {
            BuildNpc(itemData);
        }
        //创建小镇招募居民
        List<CharacterBean> listRecruitTownCharacter = npcInfoManager.GetCharacterDataByType((int)NPCTypeEnum.RecruitTown);
        foreach (CharacterBean itemData in listRecruitTownCharacter)
        {
            BuildNpc(itemData);
        }
    }

    private void BuildNpc(CharacterBean characterData)
    {
        if (objNpcModel==null|| objNpcContainer==null)
            return;
        GameObject npcObj = Instantiate(objNpcContainer,objNpcModel);
        npcObj.transform.position = new Vector3(characterData.npcInfoData.position_x, characterData.npcInfoData.position_y);
        npcObj.transform.localScale = new Vector3(1,1);
        NpcAIImportantCpt aiCpt= npcObj.GetComponent<NpcAIImportantCpt>();
        aiCpt.SetCharacterData(characterData);
    }
}