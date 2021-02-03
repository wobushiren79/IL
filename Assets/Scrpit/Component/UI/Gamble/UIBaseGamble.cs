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
    protected H gambleHandler;
    protected B gambleBuilder;

    public GameObject objWinMoneyModel;

    public override void Awake()
    {
        base.Awake();
        gambleHandler = FindWithTag<H>(TagInfo.Tag_GambleHandler);
        gambleBuilder = FindWithTag<B>(TagInfo.Tag_GambleBuilder);
    }

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
            winRewardRate.text = GameCommonInfo.GetUITextById(612) + "x" + gambleData.winRewardRate;
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
            dialogData.title = GameCommonInfo.GetUITextById(611);
            PickForMoneyDialogView PickForMoneyDialog = DialogHandler.Instance.CreateDialog<PickForMoneyDialogView>(DialogEnum.PickForMoney, this, dialogData);
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
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
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
                Sprite iconSp = IconDataHandler.Instance.manager.GetIconSpriteByName("money_1");
                ToastHandler.Instance.ToastHint(iconSp, GameCommonInfo.GetUITextById(1301));
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
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }
    #endregion
}