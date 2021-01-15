using UnityEditor;
using UnityEngine;

public class MiniGameBirthHandler : BaseMiniGameHandler<MiniGameBirthBuilder, MiniGameBirthBean>
{
    public override void InitGame(MiniGameBirthBean miniGameData)
    {
        base.InitGame(miniGameData);
        //打开倒计时UI
        OpenCountDownUI(miniGameData);
    }

    public override void StartGame()
    {
        base.StartGame();
    }

    public override void EndGame(MiniGameResultEnum gameResult)
    {
        base.EndGame(gameResult);
    }
}