using UnityEngine;
using UnityEditor;

public class UIBaseMiniGame<T> : BaseUIComponent where T: MiniGameBaseBean
{
    //游戏数据
    public T miniGameData;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="gameData"></param>
    public virtual void SetData(T gameData)
    {
        this.miniGameData = gameData;
        RefreshUI();
    }
}