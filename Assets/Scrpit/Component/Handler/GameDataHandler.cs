using UnityEngine;
using UnityEditor;
using System.Collections;
public class GameDataHandler : BaseHandler
{
    protected GameDataManager gameDataManager;
    protected GameTimeHandler gameTimeHandler;

    private void Awake()
    {
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    private void Start()
    {
        StartCoroutine(CoroutineForPlayTime());
    }

    /// <summary>
    /// 协程-游玩时间记录
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForPlayTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (gameDataManager != null)
                gameDataManager.gameData.playTime.AddTimeForHMS(0, 0, 1);
        }
    }
}