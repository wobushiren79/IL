using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIBaseOne : UIGameComponent
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
        if (uiGameManager.audioHandler != null)
            uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (uiGameManager.controlHandler != null)
            uiGameManager.controlHandler.StopControl();
        if (uiGameManager.gameTimeHandler != null)
            uiGameManager.gameTimeHandler.SetTimeStatus(true);
    }

    public override void CloseUI()
    {
        if (uiGameManager.audioHandler != null)
            uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        if (gameObject.activeSelf)
        {
            if (uiGameManager.controlHandler != null)
                uiGameManager.controlHandler.RestoreControl();
        }
        if (uiGameManager.gameTimeHandler != null)
            uiGameManager.gameTimeHandler.SetTimeRestore();
        base.CloseUI();
    }

    public void SetMoney()
    {
        if (uiGameManager.gameDataManager != null)
        {
            if (tvMoneyL != null)
            {
                tvMoneyL.text = uiGameManager.gameDataManager.gameData.moneyL + "";
            }
            if (tvMoneyM != null)
            {
                tvMoneyM.text = uiGameManager.gameDataManager.gameData.moneyM + "";
            }
            if (tvMoneyS != null)
            {
                tvMoneyS.text = uiGameManager.gameDataManager.gameData.moneyS + "";
            }
            if (tvGuildCoin != null)
            {
                tvGuildCoin.text = uiGameManager.gameDataManager.gameData.guildCoin + "";
            }
            if (tvTrophy1 != null)
            {
                tvTrophy1.text = uiGameManager.gameDataManager.gameData.trophyElementary+"";
            }
            if (tvTrophy2 != null)
            {
                tvTrophy2.text = uiGameManager.gameDataManager.gameData.trophyIntermediate + "";
            }
            if (tvTrophy3 != null)
            {
                tvTrophy3.text = uiGameManager.gameDataManager.gameData.trophyAdvanced + "";
            }
            if (tvTrophy4 != null)
            {
                tvTrophy4.text = uiGameManager.gameDataManager.gameData.trophyLegendary + "";
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