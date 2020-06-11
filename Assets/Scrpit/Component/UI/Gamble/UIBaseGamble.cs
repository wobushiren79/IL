using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIBaseGamble : UIGameComponent
{
    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;

    public virtual void Update()
    {
        SetMoney();
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    public void SetMoney()
    {
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        if (tvMoneyL != null)
            tvMoneyL.text = gameData.moneyL + "";
        if (tvMoneyM != null)
            tvMoneyM.text = gameData.moneyM + "";
        if (tvMoneyS != null)
            tvMoneyS.text = gameData.moneyS + "";
    }
}