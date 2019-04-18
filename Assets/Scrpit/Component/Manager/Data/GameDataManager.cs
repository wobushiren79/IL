using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameDataManager : BaseManager, IGameDataView
{

    //游戏数据控制
    public GameDataController gameDataController;

    //游戏数据
    public GameDataBean gameData;
    //简略游戏数据
    public List<GameDataSimpleBean> listGameDataSimple;

    private void Awake()
    {
        gameData = new GameDataBean();
        gameDataController = new GameDataController(this, this);
    }

    /// <summary>
    /// 删除游戏数据
    /// </summary>
    /// <param name="userId"></param>
    public void DeleteGameDataByUserId(string userId)
    {
        gameDataController.DeleteUserDataByUserId(userId);
    }

    /// <summary>
    /// 创建新数据
    /// </summary>
    /// <param name="gameDataBean"></param>
    public void CreateGameData(GameDataBean gameDataBean)
    {
        gameDataController.CreateUserData(gameDataBean);
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    public void SaveGameData()
    {
        gameDataController.SaveUserData(gameData);
    }

    #region 数据回调
    public void DeleteGameDataFail()
    {
    }

    public void DeleteGameDataSuccess()
    {
        gameDataController.GetSimpleGameData();
    }

    public void GetGameDataFail()
    {
    }

    public void GetGameDataSimpleListFail()
    {
    }

    public void GetGameDataSimpleListSuccess(List<GameDataSimpleBean> listData)
    {
        this.listGameDataSimple = listData;
    }

    public void GetGameDataSuccess(GameDataBean gameData)
    {
        if (gameData == null)
            gameData = new GameDataBean();
        this.gameData = gameData;

        //TODO 测试 需要删除
        gameData.buildItemList.Add(new ItemBean(30001, 3));
        gameData.buildItemList.Add(new ItemBean(40001, 3));
        gameData.menuList.Add(new MenuOwnBean(1));
    }

    public void SetGameDataFail()
    {
    }

    public void SetGameDataSuccess()
    {
    }
    #endregion

    public interface GameDataCallBack
    {
        
    }

    public interface GameDataSimpleCallBack
    {

    }
}