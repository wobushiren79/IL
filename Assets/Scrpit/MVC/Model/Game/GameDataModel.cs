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
        GameCommonInfo.GameUserId = userId;
        gameData.userId = userId;
        gameData.moneyS = 9999;
        gameData.moneyM = 0;
        gameData.moneyL = 0;
        gameData.userCharacter.attributes.cook = 5;
        gameData.userCharacter.attributes.speed = 5;
        gameData.userCharacter.attributes.account = 5;
        gameData.userCharacter.attributes.charm = 5;
        gameData.userCharacter.attributes.force = 5;
        gameData.userCharacter.attributes.lucky = 5;
        gameData.innBuildData = new InnBuildBean();
        gameData.listBuild = new List<ItemBean>();
        gameData.listItems = new List<ItemBean>();
        gameData.listWorkerCharacter = new List<CharacterBean>();
        gameData.listMenu = new List<MenuOwnBean>();

        //添加门
        List<Vector3> doorPositionList = new List<Vector3>();
        doorPositionList.Add(new Vector3(4, 0, 0));
        doorPositionList.Add(new Vector3(5, 0, 0));
        doorPositionList.Add(new Vector3(6, 0, 0));
        InnResBean innResDoor = new InnResBean(90001, new Vector3(4.5f, 0.5f, 0), doorPositionList, Direction2DEnum.Left);
        gameData.innBuildData.AddFurniture(innResDoor);
        //修改客栈大小
        gameData.innBuildData.ChangeInnSize(new List<InnResBean>() { innResDoor }, 9, 9);

        //添加家具
        //TODO 测试 
        gameData.listBuild.Add(new ItemBean(30001, 100));
        gameData.listBuild.Add(new ItemBean(40001, 5));
        gameData.listBuild.Add(new ItemBean(50001, 5));
        gameData.listBuild.Add(new ItemBean(90001, 5));

        //添加菜单
        gameData.listMenu.Add(new MenuOwnBean(1));
        gameData.listMenu.Add(new MenuOwnBean(2));

        //设置时间
        TimeBean gameTime = new TimeBean();
        gameTime.SetTimeForYMD(221, 1, 0);
        gameData.gameTime = gameTime;

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