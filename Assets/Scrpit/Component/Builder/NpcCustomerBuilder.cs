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
    public CharacterBodyManager characterBodyManager;
    public GameTimeHandler gameTimeHandler;
    // 结束点
    public Transform endPosition;
    public float startPositionRange = 2.5f;
    public bool isBuild = true;

    private void Start()
    {
        StartCoroutine(StartBuild());
    }

    private void OnDestroy()
    {
        isBuild = false;
    }

    public IEnumerator StartBuild()
    {
        while (isBuild)
        {
            float personNumber = 1;
            if (gameTimeHandler.hours >= 11 && gameTimeHandler.hours <= 13)
            {
                personNumber = 1;
            }
            else if (gameTimeHandler.hours >= 17 && gameTimeHandler.hours <= 19)
            {
                personNumber = 1;
            }
            else if (gameTimeHandler.hours < 11)
            {
                personNumber = 1 + (10 - gameTimeHandler.hours);
            }
            else if (gameTimeHandler.hours > 13 && gameTimeHandler.hours < 17)
            {
                personNumber = 2.5f;
            }
            else if(gameTimeHandler.hours > 19)
            {
                personNumber = 2 + (gameTimeHandler.hours - 17);
            }

            yield return new WaitForSeconds(personNumber);
            BuildCustomer();
        }
    }

    public void BuildCustomer()
    {
        if (npcInfoManager == null)
            return;
        CharacterBean characterData = npcInfoManager.GetRandomCharacterData();
        if (characterData == null)
            return;
        //随机生成身体数据
        CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);

        GameObject customerObj = Instantiate(objCustomerModel, objContainer.transform);
        customerObj.SetActive(true);
        customerObj.transform.localScale = new Vector3(2, 2);
        float npcPositionY = Random.Range(transform.position.y - startPositionRange, transform.position.y + startPositionRange);
        customerObj.transform.position = new Vector3(transform.position.x, npcPositionY);
        NpcAICustomerCpt customerAI = customerObj.GetComponent<NpcAICustomerCpt>();
        customerAI.SetCharacterData(characterData);
        customerAI.SetEndPosition(new Vector3(endPosition.position.x, npcPositionY));
        //想要吃饭概率
        float eatProbability = Random.Range(0f, 1f);
        if (eatProbability > 0.9f)
        {
             customerAI.SetDestinationByIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
        }
        else
        {
             customerAI.SetDestinationByIntent(NpcAICustomerCpt.CustomerIntentEnum.Walk);
        }

    }

}