using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class UIMiniGameBirthSperm : BaseUIChildComponent<UIMiniGameBirth>
{
    protected MiniGameBirthSpermBean spermData;

    public void InitData(MiniGameBirthSpermBean spermData)
    {
        this.spermData = spermData;

        RectTransform rtfSperm = (RectTransform)transform;
        rtfSperm.position = spermData.positionStart;
        rtfSperm.DOMove(spermData.positionEnd, 10).SetEase(Ease.Linear).OnComplete(() => { });
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        UIMiniGameBirthEnemy enemy = collision.gameObject.GetComponent<UIMiniGameBirthEnemy>();
        if (enemy)
        {
            DestroyImmediate(gameObject);
            return;
        }
        UIMiniGameBirthEgg egg = collision.gameObject.GetComponent<UIMiniGameBirthEgg>();
        if (egg)
        {
            DestroyImmediate(gameObject);
            return;
        }
    }
}