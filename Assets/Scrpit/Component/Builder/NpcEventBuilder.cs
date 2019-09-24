using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class NpcEventBuilder : NpcNormalBuilder, IBaseObserver
{
    //捣乱者模型
    public GameObject objRascalModel;

    private void Start()
    {
        gameTimeHandler.AddObserver(this);
    }

    /// <summary>
    /// 开始事件
    /// </summary>
    public void StartEvent()
    {
        //TODO 各种事件的完善
        RascalEvent();
    }

    /// <summary>
    /// 恶棍事件
    /// </summary>
    public void RascalEvent()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        List<CharacterBean> listData = npcInfoManager.GetCharacterDataByType(21);
        BuildRascal(RandomUtil.GetRandomDataByList(listData), npcPosition);
    }

    /// <summary>
    /// 创建恶棍
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="npcPosition"></param>
    public void BuildRascal(CharacterBean characterData,Vector3 npcPosition)
    {
        //如果大于构建上线则不再创建新NPC
        if (objContainer.transform.childCount > buildMaxNumber)
            return;
        //生成NPC
        GameObject npcObj = BuildNpc(objRascalModel,characterData, npcPosition);
        //设置意图
        NpcAIRascalCpt rascalCpt = npcObj.GetComponent<NpcAIRascalCpt>();
        CharacterFavorabilityBean characterFavorability= gameDataManager.gameData.GetFavorabilityDataById(long.Parse(characterData.baseInfo.characterId));
        rascalCpt.SetFavorabilityData(characterFavorability);
        rascalCpt.StartEvil();
    }

    #region 时间回调通知
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : UnityEngine.Object
    {
        if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {
            int hour = (int)obj[0];
            if (hour > 9 && hour <= 20)
            {
                StartEvent();
            }
        }
    }
    #endregion
}