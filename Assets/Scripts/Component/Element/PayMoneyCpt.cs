using UnityEngine;
using UnityEditor;
using DG.Tweening;
using TMPro;

public class PayMoneyCpt : BaseMonoBehaviour
{
    public SpriteRenderer iconMoney;
    public TextMeshPro tvContent;

    public void SetData(Vector3 startPosition, long moneyL, long moneyM, long moneyS)
    {
        tvContent.text = ("+" + moneyS);
        tvContent.DOFade(0, 2).SetDelay(2);
        iconMoney.DOFade(0, 2).SetDelay(2);
        transform.DOMoveY(startPosition.y + 1f, 4).OnComplete(delegate ()
        {
            if(gameObject!=null)
                Destroy(gameObject);
        });
        transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f, 5, 1);
    }

}