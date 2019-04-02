using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameDataController : BaseMVCController<GameDataModel, IGameDataView>
{
    public GameDataController(BaseMonoBehaviour content, IGameDataView view) : base(content, view)
    {

    }

    public override void InitData()
    {
        
    }

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="gameData"></param>
    public void CreateUserData(GameDataBean gameData)
    {
        if (gameData == null)
        {
            GetView().SetGameDataFail();
            return;
        }
        GetModel().AddGameData(gameData);
        GameDataSimpleBean gameDataSimple= GameDataSimpleBean.ToSimpleData(gameData);
        GetModel().SetSimpleGameDataByUserId(gameData.userId, gameDataSimple);
        GetView().SetGameDataSuccess();
    }

    /// <summary>
    /// 保存用户数据
    /// </summary>
    /// <param name="gameData"></param>
    public void SaveUserData(GameDataBean gameData)
    {
        if (gameData == null)
        {
            GetView().SetGameDataFail();
            return;
        }
        GetModel().SetGameDataByUserId(gameData.userId, gameData);
        GameDataSimpleBean gameDataSimple = GameDataSimpleBean.ToSimpleData(gameData);
        GetModel().SetSimpleGameDataByUserId(gameData.userId, gameDataSimple);
        GetView().SetGameDataSuccess();
    }

    /// <summary>
    /// 根据ID获取用户数据
    /// </summary>
    /// <param name="userId"></param>
    public void GetGameDataByUserId(string userId)
    {
        GameDataBean gameData=  GetModel().GetGameDataByUserId(userId);
        GetView().GetGameDataSuccess(gameData);
    }

    /// <summary>
    /// 获取用户数据列表
    /// </summary>
    public void GetSimpleGameData()
    {
        List<GameDataSimpleBean> listData= GetModel().GetSimpleGameDataList();
        GetView().GetGameDataSimpleListSuccess(listData);
    }
}