using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIBaseGamble : UIGameComponent
{
    //游戏标题
    public Text tvTitle;
    //下注金额
    public Text tvBetMoneyS;

    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;

    //按钮-下注
    public Button btBet;
    //按钮-离开游戏
    public Button btExit;
    //按钮-开始游戏
    public Button btStart;

    public GambleBaseBean gambleData;

    private void Start()
    {
        if (btBet)
            btBet.onClick.AddListener (OnClickBet);
        if (btExit)
            btExit.onClick.AddListener(OnClickExit);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        uiGameManager.gameTimeHandler.SetTimeStatus(true);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        uiGameManager.gameTimeHandler.SetTimeStatus(false);
    }

    public virtual void Update()
    {
        SetMoney();
    }

    public void SetData(GambleBaseBean gambleData)
    {
        this.gambleData=  gambleData;
        SetMoney();
        //获取赌博名称
        gambleData.GetGambleName(out string gambleName);
        SetTitle(gambleName);
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

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="gambleName"></param>
    public void SetTitle(string gambleName)
    {
        if (tvTitle != null)
            tvTitle.text = gambleName;
    }

    /// <summary>
    /// 点击下注
    /// </summary>
    public void OnClickBet()
    {

    }

    /// <summary>
    /// 点击离开游戏
    /// </summary>
    public void OnClickExit()
    {
        uiManager.OpenUIAndCloseOther(UIEnum.GameMain);
    }
}