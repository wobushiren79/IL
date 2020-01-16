using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIBaseOne : BaseUIComponent
{
    //返回按钮
    public Button btBack;
    //金钱
    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;
    //公会硬币
    public Text tvGuildCoin;
    //斗技场奖杯
    public Text tvTrophy1;
    public Text tvTrophy2;
    public Text tvTrophy3;
    public Text tvTrophy4;
    public virtual void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (GetUIManager<UIGameManager>().controlHandler != null)
            GetUIManager<UIGameManager>().controlHandler.StopControl();
    }

    public override void CloseUI()
    {
        if (gameObject.activeSelf)
        {
            if (GetUIManager<UIGameManager>().controlHandler != null)
                GetUIManager<UIGameManager>().controlHandler.RestoreControl();
        }
        base.CloseUI();
    }

    public void SetMoney()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        if (gameDataManager != null)
        {
            if (tvMoneyL != null)
            {
                tvMoneyL.text = gameDataManager.gameData.moneyL + "";
            }
            if (tvMoneyM != null)
            {
                tvMoneyM.text = gameDataManager.gameData.moneyM + "";
            }
            if (tvMoneyS != null)
            {
                tvMoneyS.text = gameDataManager.gameData.moneyS + "";
            }
            if (tvGuildCoin != null)
            {
                tvGuildCoin.text = gameDataManager.gameData.guildCoin + "";
            }
            if (tvTrophy1 != null)
            {
                tvTrophy1.text = gameDataManager.gameData.trophyElementary+"";
            }
            if (tvTrophy2 != null)
            {
                tvTrophy2.text = gameDataManager.gameData.trophyIntermediate + "";
            }
            if (tvTrophy3 != null)
            {
                tvTrophy3.text = gameDataManager.gameData.trophyAdvanced + "";
            }
            if (tvTrophy4 != null)
            {
                tvTrophy4.text = gameDataManager.gameData.trophyLegendary + "";
            }
        }
    }

    public void Update()
    {
        SetMoney();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }
}