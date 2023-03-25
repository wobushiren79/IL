using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UIMiniGameAccount : BaseUIComponent
{
    public GameObject objMoneyL;
    public Text tvMoneyL;
    public GameObject objMoneyM;
    public Text tvMoneyM;
    public GameObject objMoneyS;
    public Text tvMoneyS;

    public Text tvTime;

    public GameObject objMoneyModel;
    public Sprite spMoneyL;
    public Sprite spMoneyM;
    public Sprite spMoneyS;

    public int moneyL;
    public int moneyM;
    public int moneyS;

    public int moneyLMax;
    public int moneyMMax;
    public int moneySMax;
    public float gameTime;

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        SetMoney();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="moneyLMax"></param>
    /// <param name="moneyMMax"></param>
    /// <param name="moneySMax"></param>
    public void SetData(float time,int moneyLMax,int moneyMMax,int moneySMax)
    {
        moneyL = 0;
        moneyM = 0;
        moneyS = 0;
        this.gameTime = time;
        this.moneyLMax = moneyLMax;
        this.moneyMMax = moneyMMax;
        this.moneySMax = moneySMax;

        if (moneyLMax == 0)
            objMoneyL.SetActive(false);
        else
            objMoneyL.SetActive(true);

        if (moneyMMax == 0)
            objMoneyM.SetActive(false);
        else
            objMoneyM.SetActive(true);

        if (moneySMax == 0)
            objMoneyS.SetActive(false);
        else
            objMoneyS.SetActive(true);
        RefreshUI();
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    public void SetMoney()
    {
        if (tvMoneyL != null)
            tvMoneyL.text = moneyL + "/" + moneyLMax;
        if (tvMoneyM != null)
            tvMoneyM.text = moneyM + "/" + moneyMMax;
        if (tvMoneyS != null)
            tvMoneyS.text = moneyS + "/" + moneySMax;
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    /// <param name="currentTime"></param>
    public  void SetTime(float currentTime)
    {
        if (tvTime != null)
        {
            tvTime.text = currentTime + "";
            tvTime.transform.localScale = new Vector3(1, 1, 1);
            tvTime.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f).From().SetEase(Ease.OutBack);
        }
    }

    /// <summary>
    /// 展示获取金钱特效
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void ShowMoneyGet(Vector3 startPosition, int moneyL, int moneyM, int moneyS)
    {
        CreateShowMoneyItemList(MoneyEnum.L, moneyL, startPosition);
        CreateShowMoneyItemList(MoneyEnum.M, moneyM, startPosition);
        CreateShowMoneyItemList(MoneyEnum.S, moneyS, startPosition);
    }

    /// <summary>
    /// 创建特效
    /// </summary>
    /// <param name="moneyType"></param>
    /// <param name="money"></param>
    /// <param name="startPosition"></param>
    private void CreateShowMoneyItemList(MoneyEnum moneyType, int money, Vector3 startPosition)
    {
        Sprite spMoney = null;
        Vector3 endPosition = Vector3.zero;
        switch (moneyType)
        {
            case MoneyEnum.L:
                spMoney = spMoneyL;
                endPosition = tvMoneyL.transform.position;
                break;
            case MoneyEnum.M:
                spMoney = spMoneyM;
                endPosition = tvMoneyM.transform.position;
                break;
            case MoneyEnum.S:
                spMoney = spMoneyS;
                endPosition = tvMoneyS.transform.position;
                break;
        }
        for (int i = 0; i < money; i++)
        {
            GameObject objMoney = Instantiate(gameObject, objMoneyModel);
            Image ivIcon = objMoney.GetComponent<Image>();
            ivIcon.sprite = spMoney;

            RectTransform uiTarget = objMoney.GetComponent<RectTransform>();
            uiTarget.anchoredPosition = GameUtil.WorldPointToUILocalPoint((RectTransform)transform, startPosition);

            objMoney.transform.DOMove(endPosition, 1).SetDelay(i * 0.08f).OnComplete(delegate() {
                AddMoney(moneyType, 1);
                RefreshUI();
                Destroy(objMoney);
            });
        }
    }

    /// <summary>
    /// 增加金钱
    /// </summary>
    /// <param name="moneyType"></param>
    /// <param name="money"></param>
    private void AddMoney(MoneyEnum moneyType,int money)
    {
        Text tvMoney = null;
        switch (moneyType)
        {
            case MoneyEnum.L:
                moneyL += money;
                tvMoney = tvMoneyL;
                break;
            case MoneyEnum.M:
                moneyM += money;
                tvMoney = tvMoneyM;
                break;
            case MoneyEnum.S:
                moneyS += money;
                tvMoney = tvMoneyS;
                break;
        }
        AudioHandler.Instance.PlaySound(AudioSoundEnum.PayMoney);
        tvMoney.transform.DOKill();
        tvMoney.transform.localScale = new Vector3(1,1,1);
        tvMoney.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f),0.2f);
    }
}