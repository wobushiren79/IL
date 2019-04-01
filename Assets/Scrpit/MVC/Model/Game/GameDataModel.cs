using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameDataModel : BaseMVCModel
{
    private GameDataService mGameDataService;
    private GameListDataService mGameListDataService;

    public override void InitData()
    {
        mGameDataService = new GameDataService();
        mGameListDataService = new GameListDataService();
    }

    /// <summary>
    /// 通过用户ID获取游戏数据
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public GameDataBean GetGameDataByUserId(string userId)
    {
        return mGameDataService.QueryDataByUserId(userId);
    }

    /// <summary>
    /// 通过用户ID保存用户数据
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="gameData"></param>
    public void SetGameDataByUserId(string userId, GameDataBean gameData)
    {
        mGameDataService.UpdateDataByUserId(userId, gameData);
    }

    /// <summary>
    /// 增加一个用户数据
    /// </summary>
    /// <param name="gameData"></param>
    public void AddGameData(GameDataBean gameData)
    {
        string userId ="UserId_"+ SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.B);
        gameData.userId = userId;
        SetGameDataByUserId(userId, gameData);
    }

    /// <summary>
    /// 查询所有简要用户数据
    /// </summary>
    /// <returns></returns>
    public List<GameDataSimpleBean> GetSimpleGameDataList()
    {
        return mGameListDataService.QueryData();
    }

    /// <summary>
    /// 通过用户ID设置简要用户信息
    /// </summary>
    public void SetSimpleGameDataByUserId(string userId, GameDataSimpleBean gameDataSimple)
    {
        List<GameDataSimpleBean> tempListData = GetSimpleGameDataList();
        if (tempListData == null)
        {
            tempListData = new List<GameDataSimpleBean>();
            tempListData.Add(gameDataSimple);
        }
        else
        {
            int hasDataPosition = -1;
            for (int i = 0; i < tempListData.Count; i++)
            {
                GameDataSimpleBean itemData= tempListData[i];
                if (itemData.userId.Equals(userId))
                {
                    hasDataPosition = i;
                }
            }
            if (hasDataPosition != -1)
                tempListData[hasDataPosition] = gameDataSimple;
            else
                tempListData.Add(gameDataSimple);        
        }
        mGameListDataService.UpdateData(tempListData);
    }
}