using UnityEngine;
using UnityEditor;

public interface IGameDataCallBack : IBaseObserver
{
    /// <summary>
    /// 商品数量改变
    /// </summary>
    /// <param name="level"></param>
    /// <param name="number"></param>
    void GoodsNumberChange(int level,int number,int totalNumber);

    /// <summary>
    /// 场地数量改变
    /// </summary>
    /// <param name="level"></param>
    /// <param name="number"></param>
    void SpaceNumberChange(int level,int number, int totalNumber);

    /// <summary>
    /// 分数改变
    /// </summary>
    /// <param name="score"></param>
    void ScoreChange(double score);

    /// <summary>
    /// 分数等级改变
    /// </summary>
    /// <param name="level"></param>
    void ScoreLevelChange(int level);

    /// <summary>
    /// 商品等级改变
    /// </summary>
    /// <param name="level"></param>
    void GoodsLevelChange(int level);
}
