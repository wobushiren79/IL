using UnityEngine;
using UnityEditor;

public class NpcImportantBuilder : BaseMonoBehaviour
{
    public Transform marketNpc;
    public Transform GroceryNpc;
    public Transform ClothesNpc1;
    public Transform ClothesNpc2;

    public GameObject objNpcModel;
    public GameObject objNpcContainer;
    //NPC数据管理
    public NpcInfoManager npcInfoManager;


    public void BuildImportant()
    {
        if (npcInfoManager != null)
        {
            //生成市场老板
            CharacterBean characterMarketData=  npcInfoManager.GetCharacterDataById(10001);
            BuildNpc(marketNpc, characterMarketData);
            //生成杂货店老板
            CharacterBean characterGroceryData = npcInfoManager.GetCharacterDataById(20001);
            BuildNpc(GroceryNpc, characterGroceryData);
            //生成服装店老板
            CharacterBean characterClothesData1 = npcInfoManager.GetCharacterDataById(30001);
            BuildNpc(ClothesNpc1, characterClothesData1);
            CharacterBean characterClothesData2 = npcInfoManager.GetCharacterDataById(30002);
            BuildNpc(ClothesNpc2, characterClothesData2);
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