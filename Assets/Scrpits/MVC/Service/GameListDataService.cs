using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class GameListDataService : BaseDataStorageImpl<GameDataSimpleBean>
{
    private readonly string mSaveFileName;

    public GameListDataService()
    {
        mSaveFileName = "GameDataSimpleList";
    }

    /// <summary>
    /// 查询存档文件数量
    /// </summary>
    public List<string> QueryDataAllUserId()
    {
        List<string> listData = new List<string>();
        try
        {
            DirectoryInfo dires = new DirectoryInfo(Application.persistentDataPath);
            DirectoryInfo[] directoryInfos = dires.GetDirectories();

            if (directoryInfos != null)
            {
                foreach (DirectoryInfo itemDir in directoryInfos)
                {
                    if (itemDir.Name.Contains("UserId_"))
                    {
                        listData.Add(itemDir.Name);
                    }
                }
            }
        }
        catch
        {

        }

        return listData;
    }

    /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public List<GameDataSimpleBean> QueryData()
    {
        return BaseLoadDataForList(mSaveFileName);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateData(List<GameDataSimpleBean> gameDataList)
    {
        BaseSaveDataForList(mSaveFileName, gameDataList);
    }
}