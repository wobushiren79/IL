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

    public void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (GetUIMananger<UIGameManager>().controlHandler != null)
            GetUIMananger<UIGameManager>().controlHandler.StopControl();
    }

    public override void CloseUI()
    {
        if (gameObject.activeSelf)
        {
            if (GetUIMananger<UIGameManager>().controlHandler != null)
                GetUIMananger<UIGameManager>().controlHandler.RestoreControl();
        }
        base.CloseUI();
    }

    public void SetMoney()
    {
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
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