using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class NpcCustomerBuilder : NpcNormalBuilder
{
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
            if (gameTimeHandler.hour >= 11 && gameTimeHandler.hour <= 13)
            {
                personNumber = 1;
            }
            else if (gameTimeHandler.hour >= 17 && gameTimeHandler.hour <= 19)
            {
                personNumber = 1;
            }
            else if (gameTimeHandler.hour < 11)
            {
                personNumber = 1 + (10 - gameTimeHandler.hour);
            }
            else if (gameTimeHandler.hour > 13 && gameTimeHandler.hour < 17)
            {
                personNumber = 2.5f;
            }
            else if(gameTimeHandler.hour > 19)
            {
                personNumber = 2 + (gameTimeHandler.hour - 17);
            }

            yield return new WaitForSeconds(personNumber);
            BuildCustomer();
        }
    }

    public void BuildCustomer()
    {
        if (npcInfoManager == null)
            return;
        CharacterBean characterData = npcInfoManager.GetRandomCharacterData(1,1);
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