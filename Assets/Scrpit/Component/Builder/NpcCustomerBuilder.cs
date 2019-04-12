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

    // 开始点
    public Transform startPosition;
    // 结束点
    public Transform endPosition;
    public float startPositionRange=2.5f;
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
            yield return new WaitForSeconds(0.5f);
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
        //随机生成头型
        if (CheckUtil.StringIsNull(characterData.body.hair))
        {
           IconBean itemHair= RandomUtil.GetRandomDataByList(characterBodyManager.listIconBodyHair);
           characterData.body.hair = itemHair.key;
           characterData.body.hairColor = ColorBean.Random();
        }
        //随机生成眼睛
        if (CheckUtil.StringIsNull(characterData.body.eye))
        {
            IconBean itemEye = RandomUtil.GetRandomDataByList(characterBodyManager.listIconBodyEye);
            characterData.body.eye = itemEye.key;
            characterData.body.eyeColor = ColorBean.Random();
        }
        //随机生成嘴巴
        if (CheckUtil.StringIsNull(characterData.body.mouth))
        {
            IconBean itemMouth = RandomUtil.GetRandomDataByList(characterBodyManager.listIconBodyMouth);
            characterData.body.mouth = itemMouth.key;
            characterData.body.mouthColor = ColorBean.Random();
        }

        GameObject customerObj = Instantiate(objCustomerModel, objCustomerModel.transform);
        customerObj.transform.SetParent(objContainer.transform); 
        customerObj.SetActive(true);
        customerObj.transform.localScale = new Vector3(2, 2);
        float npcPositionY = Random.Range(startPosition.position.y - startPositionRange, startPosition.position.y + startPositionRange);
        customerObj.transform.position = new Vector3(startPosition.position.x, npcPositionY);

        NpcAICustomerCpt customerAI = customerObj.GetComponent<NpcAICustomerCpt>();
        customerAI.SetCharacterData(characterData);
        customerAI.SetEndPosition(new Vector3(endPosition.position.x, npcPositionY));
    }

}