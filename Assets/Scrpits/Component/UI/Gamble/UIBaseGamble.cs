using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class UIBaseGamble<T, H, B> : BaseUIComponent, DialogView.IDialogCallBack

    where T : GambleBaseBean
    where H : BaseGambleHandler<T, B>
    where B : BaseGambleBuilder
{
    //游戏标题
    public Text tvTitle;
    //下注金额
    public Text tvBetMoneyS;

    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;
    public Text winRewardRate;

    //按钮-下注
    public Button btBet;
    //按钮-离开游戏
    public Button btExit;
    //按钮-开始游戏
    public Button btStart;

    public T gambleData;
    protected H _gambleHandler;
    protected B _gambleBuilder;

    //上一次下注金额
    protected int lastBetMoneyL = 0;
    protected int lastBetMoneyM = 0;
    protected int lastBetMoneyS = 0;
    public H gambleHandler
    {
        get
        {
            if (_gambleHandler == null)
            {
                _gambleHandler = FindWithTag<H>(TagInfo.Tag_Gamble);
            }
            return _gambleHandler;
        }
    }
    public B gambleBuilder
    {
        get
        {
            if (_gambleBuilder == null)
            {
                _gambleBuilder = FindWithTag<B>(TagInfo.Tag_Gamble);
            }
            return _gambleBuilder;
        }
    }
    public GameObject objWinMoneyModel;


    private void Start()
    {
        if (btBet)
            btBet.onClick.AddListener(OnClickBet);
        if (btExit)
            btExit.onClick.AddListener(OnClickExit);
        if (btStart)
            btStart.onClick.AddListener(OnClickStart);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameTimeHandler.Instance.SetTimeStatus(true);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        GameTimeHandler.Instance.SetTimeStatus(false);
    }

    public virtual void Update()
    {
        SetMoney();
        SetBetMoney();
    }

    public virtual void SetData(T gambleData)
    {
        this.gambleData = gambleData;
        SetMoney();
        //获取赌博名称
        gambleData.GetGambleName(out string gambleName);
        SetTitle(gambleName);
    }

    /// <summary>
    /// 重置数据
    /// </summary>
    public void ResetData()
    {
        if (gambleHandler != null)
            gambleHandler.BetMoney(lastBetMoneyL, lastBetMoneyM, lastBetMoneyS, false);
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    public void SetMoney()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (tvMoneyL != null)
            tvMoneyL.text = gameData.moneyL + "";
        if (tvMoneyM != null)
            tvMoneyM.text = gameData.moneyM + "";
        if (tvMoneyS != null)
            tvMoneyS.text = gameData.moneyS + "";
    }

    /// <summary>
    /// 设置下注金额
    /// </summary>
    public void SetBetMoney()
    {
        if (tvBetMoneyS != null)
            tvBetMoneyS.text = gambleData.betForMoneyS + "/" + gambleData.betMaxForMoneyS;
        if (winRewardRate != null)
            winRewardRate.text = TextHandler.Instance.manager.GetTextById(612) + "x" + gambleData.winRewardRate;
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (gambleData.GetGambleStatus() == GambleStatusType.Prepare)
        {
            DialogBean dialogData = new DialogBean();
            dialogData.title = TextHandler.Instance.manager.GetTextById(611);
            dialogData.dialogType = DialogEnum.PickForMoney;
            dialogData.callBack = this;

            PickForMoneyDialogView PickForMoneyDialog = UIHandler.Instance.ShowDialog<PickForMoneyDialogView>(dialogData);
            PickForMoneyDialog.SetData((int)gambleData.betMaxForMoneyL / 10, (int)gambleData.betMaxForMoneyM / 10, (int)gambleData.betMaxForMoneyS / 10);
            PickForMoneyDialog.SetMaxMoney(gambleData.betMaxForMoneyL, gambleData.betMaxForMoneyM, gambleData.betMaxForMoneyS);
        }
    }

    /// <summary>
    /// 点击离开游戏
    /// </summary>
    public void OnClickExit()
    {
        if (gambleData.GetGambleStatus() == GambleStatusType.Prepare)
        {
            //退还下注金额
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            gameData.AddMoney(gambleData.betForMoneyL, gambleData.betForMoneyM, gambleData.betForMoneyS);
            UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
        }

    }

    /// <summary>
    /// 点击开始游戏
    /// </summary>
    public void OnClickStart()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (gambleData.GetGambleStatus() == GambleStatusType.Prepare)
        {
            //如果没有下注
            if (gambleData.betForMoneyS == 0)
            {
                Sprite iconSp = IconHandler.Instance.GetIconSpriteByName("money_1");
                UIHandler.Instance.ToastHint<ToastView>(iconSp, TextHandler.Instance.manager.GetTextById(1301));
                return;
            }
            gambleHandler.StartChange();
        }
    }

    /// <summary>
    /// 胜利的动画
    /// </summary>
    public void AnimForWinMoney()
    {
        float screenWith = GameUtil.GetScreenWith();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        int moneyNumber = 10;
        for (int i = 0; i < moneyNumber; i++)
        {
            GameObject objWinMoney = Instantiate(gameObject, objWinMoneyModel);
            RectTransform trfItem = (RectTransform)objWinMoney.transform;
            Image ivIcon = trfItem.GetComponent<Image>();
            ivIcon
                .DOFade(0, 0.5f)
                .SetDelay(i * 0.1f);
            trfItem
                .DOAnchorPos(trfItem.anchoredPosition + new Vector2(0, 60), 0.5f)
                .SetDelay(i * 0.1f)
                .OnStart(delegate ()
                {
                    AudioHandler.Instance.PlaySound(AudioSoundEnum.PayMoney);
                })
                .OnComplete(delegate ()
                {
                    gameData.AddMoney(0, 0, (long)(gambleData.betForMoneyS * gambleData.winRewardRate / 10));
                    Destroy(objWinMoney);

                });

        }
        transform.DOScale(new Vector3(1, 1, 1), moneyNumber * 0.1f + 0.5f).OnComplete(delegate ()
        {
            gambleHandler.EndGame();
        });
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogView as PickForMoneyDialogView)
        {
            PickForMoneyDialogView pickForMoneyDialog = dialogView as PickForMoneyDialogView;
            pickForMoneyDialog.GetPickMoney(out int moneyL, out int moneyM, out int moneyS);
            gambleHandler.BetMoney(moneyL, moneyM, moneyS);
            lastBetMoneyL = gambleData.betForMoneyL;
            lastBetMoneyM = gambleData.betForMoneyM;
            lastBetMoneyS = gambleData.betForMoneyS;
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }
    #endregion
}