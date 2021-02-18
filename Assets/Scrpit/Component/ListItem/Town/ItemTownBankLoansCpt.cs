using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemTownBankLoansCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public UserLoansBean loansData;

    public Text tvMoneyS;
    public Text tvRate;
    public Text tvDays;
    public Text tvMoneyForDay;
    public Button btSubmit;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OnClickForSubmit);
    }

    public void SetData(UserLoansBean loansData)
    {
        this.loansData = loansData;
        SetLoansMoney(loansData.moneyS);
        SetLoansRate(loansData.loansRate);
        SetLoansDays(loansData.loansDays);
        SetMoneyForDay(loansData.moneySForDay);

    }

    public void SetLoansMoney(long moneys)
    {
        if (tvMoneyS != null)
            tvMoneyS.text = moneys + "";
    }

    public void SetLoansRate(float rate)
    {
        if (tvRate != null)
            tvRate.text = rate * 100 + "%";
    }

    public void SetLoansDays(int day)
    {
        if (tvDays != null)
            tvDays.text = day + TextHandler.Instance.manager.GetTextById(31);
    }

    public void SetMoneyForDay(long moneys)
    {
        if (tvMoneyForDay != null)
            tvMoneyForDay.text = moneys + "";
    }

    public void OnClickForSubmit()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (gameData.listLoans.Count >= gameData.loansNumberLimit)
        {
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1091));
            return;
        }

        DialogBean dialogData = new DialogBean
        {
            content = string.Format(TextHandler.Instance.manager.GetTextById(3091), tvMoneyS.text, tvDays.text)
        };
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);

    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (gameData.AddLoans(loansData))
        {
            gameData.AddMoney(0, 0, loansData.moneyS);
            ToastHandler.Instance.ToastHint(string.Format(TextHandler.Instance.manager.GetTextById(1092), tvMoneyS.text));
        }
        else
        {
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1091));
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}