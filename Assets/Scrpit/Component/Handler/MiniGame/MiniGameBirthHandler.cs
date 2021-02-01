using Boo.Lang;
using UnityEditor;
using UnityEngine;

public class MiniGameBirthHandler : BaseMiniGameHandler<MiniGameBirthBuilder, MiniGameBirthBean>
{

    public List<MiniGameBirthSpermBean> listSperm = new List<MiniGameBirthSpermBean>();

    protected override void Awake()
    {
        builderName = "MiniGameBirthBuilder";
        base.Awake();
    }

    public override void InitGame(MiniGameBirthBean miniGameData)
    {
        listSperm.Clear();
        base.InitGame(miniGameData);
        //打开倒计时UI
        // OpenCountDownUI(miniGameData);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIMiniGameBirth>(UIEnum.MiniGameBirth);
    }

    public override void StartGame()
    {
        base.StartGame();
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIMiniGameBirth>(UIEnum.MiniGameBirth);
    }

    public override void EndGame(MiniGameResultEnum gameResult)
    {
        base.EndGame(gameResult);
        listSperm.Clear();
    }

    /// <summary>
    /// 到达卵子
    /// </summary>
    public void ArriveEgg(MiniGameBirthSpermBean spermData)
    {
       GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
       FamilyDataBean familyData =  gameData.GetFamilyData();
       familyData.addBirthPro(0.05f);
       DestroySperm(spermData);
    }

    /// <summary>
    /// 删除精子
    /// </summary>
    /// <param name="spermData"></param>
    public void DestroySperm(MiniGameBirthSpermBean spermData)
    {
        if (listSperm.Contains(spermData))
        {
            listSperm.Remove(spermData);
        }
        CheckGameOver();
    }

    /// <summary>
    /// 发射精子
    /// </summary>
    public bool FireSperm(out MiniGameBirthSpermBean spermData)
    {
        spermData = null;
        //没有次数了
        if (miniGameData.life <= 0)
            return false;
        spermData = new MiniGameBirthSpermBean
        {
            timeForSpeed = 10
        };
        listSperm.Add(spermData);
        return true;
    }

    /// <summary>
    /// 检测游戏是否结束
    /// </summary>
    /// <returns></returns>
    public bool CheckGameOver()
    {
        if (miniGameData.life <= 0 && listSperm.Count < 0)
        {
            EndGame(MiniGameResultEnum.Win);
            return true;
        }
        return false;     
    }
}