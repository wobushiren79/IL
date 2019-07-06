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
        List<NpcInfoBean> listCharacter=  npcInfoManager.GetNpcInfoByType(new int[] {1,2,3});
        foreach (NpcInfoBean itemData in listCharacter)
        {
            //生成
            CharacterBean characterData = NpcInfoManager.NpcInfoToCharacterData(itemData);
            BuildNpc(itemData,characterData);
        }
    }

    private void BuildNpc(NpcInfoBean infoData, CharacterBean characterData)
    {
        if (objNpcModel==null|| objNpcContainer==null)
            return;
        GameObject npcObj = Instantiate(objNpcModel, objNpcContainer.transform);
        npcObj.SetActive(true);
        npcObj.transform.position = new Vector3(infoData.position_x,infoData.position_y);
        npcObj.transform.localScale = new Vector3(2,2);
        NpcAIImportantCpt aiCpt= npcObj.GetComponent<NpcAIImportantCpt>();
        aiCpt.SetCharacterData(characterData);
    }
}