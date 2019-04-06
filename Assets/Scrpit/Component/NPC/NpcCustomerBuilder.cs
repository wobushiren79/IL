using UnityEngine;
using UnityEditor;

public class NpcCustomerBuilder : BaseMonoBehaviour
{
    //添加的容器
    public GameObject objContainer;
    //顾客模型
    public GameObject objCustomerModel;

    //NPC数据管理
    public NpcInfoManager npcInfoManager;



    public void BuildCustomer()
    {
        if (npcInfoManager == null)
            return;
        CharacterBean characterData= npcInfoManager.GetRandomCharacterData();
        GameObject customerObj =  Instantiate(objCustomerModel, objCustomerModel.transform);
        NpcAICustomerCpt customerAI = customerObj.GetComponent<NpcAICustomerCpt>();
        customerAI.SetCharacterData(characterData);
    }

}