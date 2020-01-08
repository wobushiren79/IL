using UnityEngine;
using UnityEditor;

public class MiniGameAccountMoneyCpt : BaseMonoBehaviour
{
    public SpriteRenderer srMoney;
    public Sprite spMoneyS;
    public Sprite spMoneyM;
    public Sprite spMoneyL;

    public MoneyEnum moneyType;
    public int money;

    public void InitData(MoneyEnum moneyType, int money)
    {
        this.moneyType = moneyType;
        this.money = money;
        switch (moneyType)
        {
            case MoneyEnum.L:
                srMoney.sprite = spMoneyL;
                break;
            case MoneyEnum.M:
                srMoney.sprite = spMoneyM;
                break;
            case MoneyEnum.S:
                srMoney.sprite = spMoneyS;
                break;
        }
        float scaleSize = money / 5f;
        transform.localScale = new Vector3(1f + scaleSize, 1f + scaleSize,1f + scaleSize);
        transform.eulerAngles =new Vector3(0,0, Random.Range(0, 360));
    }
}