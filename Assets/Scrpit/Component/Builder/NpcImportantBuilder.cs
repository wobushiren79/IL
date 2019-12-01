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
        List<CharacterBean> listCharacter=  npcInfoManager.GetCharacterDataByType(new int[] {1});
        foreach (CharacterBean itemData in listCharacter)
        {
            BuildNpc(itemData);
        }
    }

    private void BuildNpc(CharacterBean characterData)
    {
        if (objNpcModel==null|| objNpcContainer==null)
            return;
        GameObject npcObj = Instantiate(objNpcModel, objNpcContainer.transform);
        npcObj.SetActive(true);
        npcObj.transform.position = new Vector3(characterData.npcInfoData.position_x, characterData.npcInfoData.position_y);
        npcObj.transform.localScale = new Vector3(1,1);
        NpcAIImportantCpt aiCpt= npcObj.GetComponent<NpcAIImportantCpt>();
        aiCpt.SetCharacterData(characterData);
        InteractiveTalkCpt interactiveTalk = npcObj.GetComponent<InteractiveTalkCpt>();
        interactiveTalk.markIds = StringUtil.SplitBySubstringForArrayLong(characterData.npcInfoData.talk_ids,',');
    }
}