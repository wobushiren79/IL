using UnityEngine;
using UnityEditor;

public class MiniGameAccountMoneyCpt : BaseMonoBehaviour
{
    public SpriteRenderer srMoney;
    public Sprite spMoneyS;
    public Sprite spMoneyM;
    public Sprite spMoneyL;

    public void InitData(MoneyEnum moneyType, int money)
    {
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
        if (money >= 0 && money < 5)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (money >= 5 && money < 10)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (money >= 10)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        transform.eulerAngles =new Vector3(0,0, Random.Range(0, 360));
    }
}