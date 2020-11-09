using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class UserInfiniteTowersBean
{
    public List<string> listMembers = new List<string>();//成员
    public bool isSend = false;//是否是派遣
    public int layer = 1;//层数
    public float proForSend = 0;//派遣进度
    public int teamFactor = 0;//团队总因子

    /// <summary>
    /// 检测是否成功进入下一层
    /// </summary>
    public bool CheckIsSccessNextLayer()
    {
        //成功因子 超过则必定成功 低于则按比例降低成功率
        //该因子为 层数+10  * 7大属性影响 * 3人团队
        int successFactor = (int)(((layer + 10) * 7 * 3) * 0.8f);
        //如果超过则必然成功
        if (teamFactor >= successFactor)
        {
            return true;
        }
        //如果低于则有几率成功
        else
        {
            float successRate = (teamFactor / (float)successFactor) - 0.5f;
            if (successRate <= 0)
            {
                return false;
            }
            else
            {
                float randomRate = UnityEngine.Random.Range(0f, 1f);
                if (randomRate <= successRate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    /// <summary>
    /// 初始化成功攀登几率
    /// </summary>
    /// <param name="gameData"></param>
    public void InitSuccessRate(GameItemsManager gameItemsManager, List<CharacterBean> listMemberData)
    {
        int totalLife = 0;
        int totalCook = 0;
        int totalSpeed = 0;
        int totalAccount = 0;
        int totalCharm = 0;
        int totalForce = 0;
        int totalLucky = 0;

        for (int i = 0; i < listMemberData.Count; i++)
        {
            CharacterBean itemCharacterData = listMemberData[i];
            itemCharacterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);

            totalLife += characterAttributes.life;
            totalCook += characterAttributes.cook;
            totalSpeed += characterAttributes.speed;
            totalAccount += characterAttributes.account;
            totalCharm += characterAttributes.charm;
            totalForce += characterAttributes.force;
            totalLucky += characterAttributes.lucky;
        }

        teamFactor = totalLife / 10 + totalCook + totalSpeed + totalAccount + totalCharm + totalForce + totalLucky;
    }
}