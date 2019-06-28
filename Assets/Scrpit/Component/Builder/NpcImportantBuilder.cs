using UnityEngine;
using UnityEditor;

public class NpcImportantBuilder : BaseMonoBehaviour
{
    public Transform marketNpc;
    public Transform GroceryNpc;

    public GameObject objNpcModel;
    public GameObject objNpcContainer;
    //NPC数据管理
    public NpcInfoManager npcInfoManager;


    public void BuildImportant()
    {
        if (npcInfoManager != null)
        {
            //生成市场老板
            CharacterBean characterMarketData=  npcInfoManager.GetCharacterDataByType(1);
            BuildNpc(marketNpc, characterMarketData);
            //生成杂货店老板
            CharacterBean characterGroceryData = npcInfoManager.GetCharacterDataByType(2);
            BuildNpc(GroceryNpc, characterGroceryData);
        }
    }

    private void BuildNpc(Transform npcPosition, CharacterBean characterData)
    {
        if (npcPosition == null|| objNpcModel==null|| objNpcContainer==null)
            return;
        GameObject npcObj = Instantiate(objNpcModel, objNpcContainer.transform);
        npcObj.SetActive(true);
        npcObj.transform.position = npcPosition.position;
        npcObj.transform.localScale = new Vector3(2,2);
        NpcAIImportantCpt aiCpt= npcObj.GetComponent<NpcAIImportantCpt>();
        aiCpt.SetCharacterData(characterData);
    }
}