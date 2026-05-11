using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class UITownBankChangeMoney : BaseUIView
{
    public ItemTownBankExchangeCpt SToM;
    public ItemTownBankExchangeCpt MToS;
    public ItemTownBankExchangeCpt MToL;
    public ItemTownBankExchangeCpt LToM;

    public override void OpenUI()
    {
        base.OpenUI();
        AnimForInit(SToM.gameObject);
        AnimForInit(MToS.gameObject);
        AnimForInit(MToL.gameObject);
        AnimForInit(LToM.gameObject);
        InitData();
    }

    public void InitData()
    {
        int randomSMRate = 1000;
        int randomMLRate = 10;
        //有一定的几率不会有变化幅度
        Random.InitState(GameCommonInfo.RandomSeed);
        if (Random.Range(0f, 1f) > 0.1f)
        {
            randomSMRate = Random.Range(995, 1005);
            randomMLRate = Random.Range(9, 11);
        }
        //设置汇率
        if (SToM != null)
        {
            SToM.SetData(ItemTownBankExchangeCpt.ExchangeMoneyEnum.SToM, randomSMRate, 1);
        }
        if (MToS != null)
        {
            MToS.SetData(ItemTownBankExchangeCpt.ExchangeMoneyEnum.MToS, 1, randomSMRate);
        }
        if (MToL != null)
        {
            MToL.SetData(ItemTownBankExchangeCpt.ExchangeMoneyEnum.MToL, randomMLRate, 1);
        }
        if (LToM != null)
        {
            LToM.SetData(ItemTownBankExchangeCpt.ExchangeMoneyEnum.LToM, 1, randomMLRate);
        }
    }



    /// <summary>
    /// 初始化动画
    /// </summary>
    /// <param name="obj"></param>
    public void AnimForInit(GameObject obj)
    {
        obj.transform.DOKill();
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From();
    }
}