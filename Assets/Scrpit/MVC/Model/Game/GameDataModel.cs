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
        string userId = "UserId_" + SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        GameCommonInfo.gameUserId = userId;
        gameData.userId = userId;
        gameData.moneyS = 1000;
        gameData.innBuildData = new InnBuildBean();
        gameData.buildItemList = new List<ItemBean>();
        gameData.equipItemList = new List<ItemBean>();
        //添加地板
        gameData.innBuildData.listFloor = new List<InnResBean>();
        for(int i = 0; i < 6; i++)
        {
            for(int f = 0; f < 6; f++)
            {
                InnResBean itemData = new InnResBean();
                itemData.id = 10001;
                itemData.startPosition = new Vector3Bean(i, f);
                gameData.innBuildData.listFloor.Add(itemData);
            }
        }
        //添加墙壁
        gameData.innBuildData.listWall = new List<InnResBean>();
        for (int i = 0; i < 6; i++)
        {
            for (int f = 0; f < 6; f++)
            {
                bool isBuild = false;
                if (i == 0 || i == 5)
                {
                    isBuild = true;
                }
                else
                {
                    if (f == 0 || f == 5)
                    {
                        isBuild = true;
                    }
                }
                if (isBuild) {
                    InnResBean itemData = new InnResBean();
                    itemData.id = 20001;
                    itemData.startPosition = new Vector3Bean(i, f);
                    gameData.innBuildData.listWall.Add(itemData);
                }
            }
        }
        //添加家具
        gameData.buildItemList.Add(new ItemBean(30001,1));

        SetGameDataByUserId(userId, gameData);
    }

    /// <summary>
    /// 通过用户ID删除游戏数据
    /// </summary>
    public void DeleteGameDataByUserId(string userId)
    {
        mGameDataService.DeleteDataByUserId(userId);
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
                GameDataSimpleBean itemData = tempListData[i];
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

    /// <summary>
    /// 通过用户ID删除游戏简要数据
    /// </summary>
    public void DeleteGameDataSimpleByUserId(string userId)
    {
        List<GameDataSimpleBean> tempListData = GetSimpleGameDataList();
        if (!CheckUtil.ListIsNull(tempListData))
        {
            int deletePosition = -1;
            for (int i = 0; i < tempListData.Count; i++)
            {
                GameDataSimpleBean itemData = tempListData[i];
                if (itemData.userId.Equals(userId))
                {
                    deletePosition = i;
                }
            }
            if (deletePosition != -1)
            {
                tempListData.RemoveAt(deletePosition);
                mGameListDataService.UpdateData(tempListData);
            }
        }
    }
}