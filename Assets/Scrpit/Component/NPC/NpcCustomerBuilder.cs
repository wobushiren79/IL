using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class NpcCustomerBuilder : BaseMonoBehaviour
{
    //添加的容器
    public GameObject objContainer;
    //顾客模型
    public GameObject objCustomerModel;

    //NPC数据管理
    public NpcInfoManager npcInfoManager;

    // 开始点
    public Transform startPosition;
    // 结束点
    public Transform endPosition;
    public float startPositionRange=2.5f;
    public bool isBuild = true;

    public void BuildCustomer()
    {
        if (npcInfoManager == null)
            return;
        CharacterBean characterData= npcInfoManager.GetRandomCharacterData();
        GameObject customerObj =  Instantiate(objCustomerModel, objCustomerModel.transform);
        customerObj.transform.SetParent(objContainer.transform);
        customerObj.SetActive(true);
        float npcPositionY = Random.Range(startPosition.position.y- startPositionRange, startPosition.position.y + startPositionRange);
        customerObj.transform.position = new Vector3(startPosition.position.x, npcPositionY);

        NpcAICustomerCpt customerAI = customerObj.GetComponent<NpcAICustomerCpt>();
        customerAI.SetCharacterData(characterData);
        customerAI.SetEndPosition(new Vector3(endPosition.position.x, npcPositionY));
    }

    private void Start()
    {
        StartCoroutine(StartBuild());
    }

    public IEnumerator StartBuild()
    {
        while (isBuild)
        {
            yield return new WaitForSeconds(0.5f);
            BuildCustomer();
        }
    }

    private void OnDestroy()
    {
        isBuild = false;
    }
}