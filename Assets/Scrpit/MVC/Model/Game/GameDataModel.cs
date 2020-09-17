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
        GameDataBean gameData = mGameDataService.QueryDataByUserId(userId);
        //错误纠正
        if (gameData != null)
        {
            if (CheckUtil.StringIsNull(gameData.userCharacter.baseInfo.characterId))
            {
                gameData.userCharacter.baseInfo.characterId = userId;
            }
            if (!CheckUtil.ListIsNull(gameData.listWorkerCharacter))
            {
                foreach (CharacterBean characterData in gameData.listWorkerCharacter)
                {
                    if (CheckUtil.StringIsNull(characterData.baseInfo.characterId))
                    {
                        characterData.baseInfo.characterId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
                    }
                }
            }
        }
        return gameData;
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
        gameData.userCharacter.baseInfo.characterId = userId;
        gameData.userId = userId;
        gameData.moneyS = 1000;
        gameData.moneyM = 0;
        gameData.moneyL = 0;
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
        //添加家具
        List<Vector3> counterPositionList = new List<Vector3>();
        counterPositionList.Add(new Vector3(7, 1, 0));
        counterPositionList.Add(new Vector3(6, 1, 0));
        counterPositionList.Add(new Vector3(7, 2, 0));
        counterPositionList.Add(new Vector3(6, 2, 0));
        counterPositionList.Add(new Vector3(7, 3, 0));
        counterPositionList.Add(new Vector3(6, 3, 0));
        counterPositionList.Add(new Vector3(8, 2, 0));
        InnResBean innResCounter = new InnResBean(50001, new Vector3(6.5f, 2.5f, 0), counterPositionList, Direction2DEnum.Right);

        List<Vector3> stovePositionList = new List<Vector3>();
        stovePositionList.Add(new Vector3(3, 6, 0));
        stovePositionList.Add(new Vector3(4, 6, 0));
        stovePositionList.Add(new Vector3(3, 5, 0));
        stovePositionList.Add(new Vector3(4, 5, 0));
        stovePositionList.Add(new Vector3(3, 4, 0));
        stovePositionList.Add(new Vector3(4, 4, 0));
        stovePositionList.Add(new Vector3(2, 5, 0));
        InnResBean innResStove = new InnResBean(40001, new Vector3(2.5f, 5.5f, 0), stovePositionList, Direction2DEnum.Left);

        List<Vector3> tablePositionList = new List<Vector3>();
        tablePositionList.Add(new Vector3(2, 2, 0));
        tablePositionList.Add(new Vector3(3, 2, 0));
        InnResBean innResTable = new InnResBean(30001, new Vector3(1.5f, 2.5f, 0), tablePositionList, Direction2DEnum.Left);

        gameData.innBuildData.AddFurniture(1, innResDoor);
        gameData.innBuildData.AddFurniture(1, innResCounter);
        gameData.innBuildData.AddFurniture(1, innResStove);
        gameData.innBuildData.AddFurniture(1, innResTable);
        //修改客栈大小
        gameData.innBuildData.ChangeInnSize(1 , new List<InnResBean>() { innResDoor }, 9, 9);
        //母亲的信
        gameData.listItems.Add(new ItemBean(1500001, 1));
        //添加家具
        gameData.listBuild.Add(new ItemBean(30001, 2));

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