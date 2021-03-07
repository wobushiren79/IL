using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFamilyBuilder : NpcNormalBuilder
{

    protected List<NpcAIFamilyCpt> listFamilyCharacter = new List<NpcAIFamilyCpt>();

    private void Start()
    {
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
    }
    private void OnDestroy()
    {
        GameTimeHandler.Instance.UnRegisterNotifyForTime(NotifyForTime);
    }

    /// <summary>
    /// 建造家族成员
    /// </summary>
    public void BuildFamily()
    {
        //获取家族数据
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData = gameData.GetFamilyData();
        List<CharacterForFamilyBean> listFamilyCharacterData = familyData.GetAllFamilyData();
        for (int i = 0; i < listFamilyCharacterData.Count; i++)
        {
            CharacterForFamilyBean characterForFamily = listFamilyCharacterData[i];
            CreateFamilyCharacter(characterForFamily);
        }
    }

    /// <summary>
    /// 创建家族成员
    /// </summary>
    /// <param name="characterForFamily"></param>
    public void CreateFamilyCharacter(CharacterForFamilyBean characterForFamily)
    {
        if (characterForFamily == null)
            return;
        FamilyTypeEnum familyType = characterForFamily.GetFamilyType(); 
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData= gameData.GetFamilyData();
        if (familyType == FamilyTypeEnum.Daughter || familyType == FamilyTypeEnum.Son)
        {
            //如果是女儿或者儿子 需要3年后才能移动
            if (!characterForFamily.CheckIsGrowUp(gameData.gameTime))
            {
                return;
            }
        } 
        else if (familyType == FamilyTypeEnum.Mate) 
        {
            //如果是伴侣需要结婚日之后才刷新
            if (!familyData.CheckMarry(gameData.gameTime))
            {
                return;
            }
        }


        //获取门的坐标 并在门周围生成NPC
        Vector3 doorPosition = InnHandler.Instance.GetRandomEntrancePosition();
        //向下3个单位
        doorPosition += new Vector3(0, -3f, 0);
        GameObject objFamily = BuildNpc(objNormalModel, characterForFamily, doorPosition);
        if (objFamily == null)
            return;
        NpcAIFamilyCpt npcAIFamily = objFamily.GetComponent<NpcAIFamilyCpt>();
        listFamilyCharacter.Add(npcAIFamily);
    }

    /// <summary>
    /// 时间回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observable"></param>
    /// <param name="type"></param>
    /// <param name="obj"></param>
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            ClearNpc();
        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            ClearNpc();
        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {

        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.StartRest)
        {
            BuildFamily();
        }
    }

    public override void ClearNpc()
    {
        base.ClearNpc();
        listFamilyCharacter.Clear();
    }

    /// <summary>
    /// 刷新工作者数据
    /// </summary>
    public void RefreshFamilyData()
    {
        if (listFamilyCharacter == null)
            return;
        foreach (NpcAIFamilyCpt npcFamily in listFamilyCharacter)
        {
            npcFamily.SetCharacterData(npcFamily.characterData);
        }
    }
    public void HideNpc()
    {
        foreach (NpcAIFamilyCpt itemNpc in listFamilyCharacter)
        {
            itemNpc.gameObject.SetActive(false);
        }

    }

    public void ShowNpc()
    {
        foreach (NpcAIFamilyCpt itemNpc in listFamilyCharacter)
        {
            itemNpc.gameObject.SetActive(true);
        }
    }
}