using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PickForMoneyDialogView : DialogView
{
    public GameObject objMoneyL;
    public Button btSubMoneyL;
    public Button btAddMoneyL;
    public InputField etMoneyL;

    public GameObject objMoneyM;
    public Button btSubMoneyM;
    public Button btAddMoneyM;
    public InputField etMoneyM;

    public GameObject objMoneyS;
    public Button btSubMoneyS;
    public Button btAddMoneyS;
    public InputField etMoneyS;

   

    //每次增量
    public int cvMoneyL = 1;
    public int cvMoneyM = 1;
    public int cvMoneyS = 1;

    //最大金钱数
    public int maxMoneyL = 1;
    public int maxMoneyM = 1;
    public int maxMoneyS = 99999;

    public int moneyL = 0;
    public int moneyM = 0;
    public int moneyS = 0;


    public override void Awake()
    {
        base.Awake();
        if (etMoneyL)
            etMoneyL.onEndEdit.AddListener(OnEndEditForMoneyL);
        if (etMoneyM)
            etMoneyM.onEndEdit.AddListener(OnEndEditForMoneyM);
        if (etMoneyS)
            etMoneyS.onEndEdit.AddListener(OnEndEditForMoneyS);
    }

    public override void InitData()
    {
        base.InitData();
        if (btSubMoneyL != null)
            btSubMoneyL.onClick.AddListener(SubMoneyForL);
        if (btAddMoneyL != null)
            btAddMoneyL.onClick.AddListener(AddMoneyForL);

        if (btSubMoneyM != null)
            btSubMoneyM.onClick.AddListener(SubMoneyForM);
        if (btAddMoneyM != null)
            btAddMoneyM.onClick.AddListener(AddMoneyForM);

        if (btSubMoneyS != null)
            btSubMoneyS.onClick.AddListener(SubMoneyForS);
        if (btAddMoneyS != null)
            btAddMoneyS.onClick.AddListener(AddMoneyForS);
        RefreshUI();
    }

    public void SubMoneyForL()
    {
        ChangeMoney(MoneyEnum.L, moneyL - cvMoneyL);
    }
    public void AddMoneyForL()
    {
        ChangeMoney(MoneyEnum.L, moneyL + cvMoneyL);
    }
    public void SubMoneyForM()
    {
        ChangeMoney(MoneyEnum.M, moneyM - cvMoneyM);
    }
    public void AddMoneyForM()
    {
        ChangeMoney(MoneyEnum.M, moneyM + cvMoneyM);
    }
    public void SubMoneyForS()
    {
        ChangeMoney(MoneyEnum.S, moneyS - cvMoneyS);
    }
    public void AddMoneyForS()
    {
        ChangeMoney(MoneyEnum.S, moneyS + cvMoneyS);
    }

    public void RefreshUI()
    {
        SetMoney(moneyL, moneyM, moneyS);
    }

    public override void SubmitOnClick()
    {
        if (moneyL == 0 && moneyM == 0 && moneyS == 0)
        {
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1033));
            return;
        }
        base.SubmitOnClick();
    }

    public void GetPickMoney(out int moneyL, out int moneyM, out int moneyS)
    {
        moneyL = this.moneyL;
        moneyM = this.moneyM;
        moneyS = this.moneyS;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="cvMoneyL"></param>
    /// <param name="cvMoneyM"></param>
    /// <param name="cvMoneyS"></param>
    public void SetData(int cvMoneyL, int cvMoneyM, int cvMoneyS)
    {
        this.cvMoneyL = cvMoneyL;
        this.cvMoneyM = cvMoneyM;
        this.cvMoneyS = cvMoneyS;
    }

    /// <summary>
    /// 设置最大金钱修改数
    /// </summary>
    /// <param name="maxMoneyL"></param>
    /// <param name="maxMoneyM"></param>
    /// <param name="maxMoneyS"></param>
    public void SetMaxMoney(int maxMoneyL, int maxMoneyM, int maxMoneyS)
    {
        this.maxMoneyL = maxMoneyL;
        this.maxMoneyM = maxMoneyM;
        this.maxMoneyS = maxMoneyS;
        if (maxMoneyL == 0)
            objMoneyL.SetActive(false);
        else
            objMoneyL.SetActive(true);

        if (maxMoneyM == 0)
            objMoneyM.SetActive(false);
        else
            objMoneyM.SetActive(true);

        if (maxMoneyS == 0)
            objMoneyS.SetActive(false);
        else
            objMoneyS.SetActive(true);
    }

    /// <summary>
    /// 修改金钱
    /// </summary>
    /// <param name="type"></param>
    /// <param name="changeValue"></param>
    public void ChangeMoney(MoneyEnum type, int value)
    {
        switch (type)
        {
            case MoneyEnum.L:
                moneyL = value;
                if (moneyL < 0)
                    moneyL = 0;
                //上限设置
                if (moneyL > maxMoneyL)
                    moneyL = maxMoneyL;
                break;
            case MoneyEnum.M:
                moneyM = value;
                if (moneyM < 0)
                    moneyM = 0;
                //上限设置
                if (moneyM > maxMoneyM)
                    moneyM = maxMoneyM;
                break;
            case MoneyEnum.S:
                moneyS = value;
                if (moneyS < 0)
                    moneyS = 0;
                //上限设置
                if (moneyS > maxMoneyS)
                    moneyS = maxMoneyS;
                break;
        }
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        RefreshUI();
    }



    public void OnEndEditForMoneyL(string value)
    {
        if (int.TryParse(value, out int result))
        {
            ChangeMoney(MoneyEnum.L, result);
        }
        else
        {
            ChangeMoney(MoneyEnum.L, (int)moneyL);
        }

    }
    public void OnEndEditForMoneyM(string value)
    {
        if (int.TryParse(value, out int result))
        {
            ChangeMoney(MoneyEnum.M, result);
        }
        else
        {
            ChangeMoney(MoneyEnum.M, (int)moneyM);
        }
    }

    public void OnEndEditForMoneyS(string value)
    {
        if (int.TryParse(value, out int result))
        {
            ChangeMoney(MoneyEnum.S, result);
        }
        else
        {
            ChangeMoney(MoneyEnum.S, (int)moneyS);
        }
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void SetMoney(long moneyL, long moneyM, long moneyS)
    {
        if (etMoneyL != null)
        {
            etMoneyL.text = moneyL + "";
        }
        if (etMoneyM != null)
        {
            etMoneyM.text = moneyM + "";
        }
        if (etMoneyS != null)
        {
            etMoneyS.text = moneyS + "";
        }
    }
}