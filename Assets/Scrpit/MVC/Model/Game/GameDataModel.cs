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
        gameData.workCharacterList = new List<CharacterBean>();
        gameData.menuList = new List<MenuOwnBean>();

        gameData.innBuildData.innWidth = 9;
        gameData.innBuildData.innHeight = 9;
        //添加门
        List<Vector3> doorPositionList = new List<Vector3>();
        doorPositionList.Add(new Vector3(4, 0, 0));
        doorPositionList.Add(new Vector3(5, 0, 0));
        doorPositionList.Add(new Vector3(6, 0, 0));
        gameData.innBuildData.AddFurniture(new InnResBean(90001, new Vector3(6, 0, 0), doorPositionList, Direction2DEnum.Left));
        //添加地板
        gameData.innBuildData.InitFloor();
        //添加墙壁
        gameData.innBuildData.InitWall();

        //添加家具
        //TODO 测试 
        gameData.buildItemList.Add(new ItemBean(30001, 5));
        gameData.buildItemList.Add(new ItemBean(40001, 1));
        gameData.buildItemList.Add(new ItemBean(50001, 1));

        //添加一个员工
        CharacterBean worker = new CharacterBean();
        CharacterBean worker2 = new CharacterBean();
        worker.baseInfo.name = "张三";
        worker2.baseInfo.name = "李四";
        gameData.workCharacterList.Add(worker);
        gameData.workCharacterList.Add(worker2);

        //添加菜单
        gameData.menuList.Add(new MenuOwnBean(1));
        gameData.menuList.Add(new MenuOwnBean(101));
        gameData.menuList.Add(new MenuOwnBean(201));
        gameData.menuList.Add(new MenuOwnBean(301));
        gameData.menuList.Add(new MenuOwnBean(401));
        gameData.menuList.Add(new MenuOwnBean(10001));
        gameData.ingOilsalt=50;
        gameData.ingVegetables = 50;
        gameData.ingMelonfruit = 50;
        gameData.ingWaterwine = 50;
        gameData.ingMeat = 10;
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