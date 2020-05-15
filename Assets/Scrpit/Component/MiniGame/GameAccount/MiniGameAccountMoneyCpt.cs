using UnityEngine;
using UnityEditor;

public class MiniGameAccountMoneyCpt : BaseMonoBehaviour
{
    public ParticleSystem psMoney;
    public SpriteRenderer srMoney;
    public SpriteRenderer srShadow;
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
                srShadow.sprite = spMoneyL;
                break;
            case MoneyEnum.M:
                srMoney.sprite = spMoneyM;
                srShadow.sprite = spMoneyM;
                break;
            case MoneyEnum.S:
                srMoney.sprite = spMoneyS;
                srShadow.sprite = spMoneyS;
                break;
        }
         
        float scaleSize =0.5f +  money * 0.15f;
        if (money >= 10)
        {
            psMoney.gameObject.SetActive(true);
        }
        else
        {
            psMoney.gameObject.SetActive(false);
        }
        transform.localScale = new Vector3( scaleSize, scaleSize, scaleSize);
        transform.eulerAngles =new Vector3(0,0, Random.Range(0, 360));
    }
}