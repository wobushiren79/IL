using UnityEngine;
using UnityEditor;

public class BaseGambleHandler<T> : BaseHandler where T: GambleBaseBean
{
    public T gambleData;

    /// <summary>
    /// 初始化游戏
    /// </summary>
    public virtual void InitGame(T gambleData)
    {
        this.gambleData = gambleData;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public virtual void StartGame()
    {

    }

    /// <summary>
    /// 离开游戏
    /// </summary>
    public virtual void ExitGame()
    {

    }

    /// <summary>
    /// 下注
    /// </summary>
    public virtual void BetMoney()
    {

    }


}