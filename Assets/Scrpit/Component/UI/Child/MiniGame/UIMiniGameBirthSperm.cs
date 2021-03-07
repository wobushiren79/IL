using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class UIMiniGameBirthSperm : BaseUIChildComponent<UIMiniGameBirth>
{
    protected MiniGameBirthSpermBean spermData;

    protected bool isDestroy = false;
    public void InitData(MiniGameBirthSpermBean spermData)
    {
        this.spermData = spermData;

        RectTransform rtfSperm = (RectTransform)transform;
        rtfSperm.position = spermData.positionStart;
        rtfSperm.DOMove(spermData.positionEnd, spermData.timeForSpeed).SetEase(Ease.Linear).OnComplete(() => { });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroy)
            return;
        UIMiniGameBirthEnemy enemy = collision.gameObject.GetComponent<UIMiniGameBirthEnemy>();
        if (enemy)
        {
            isDestroy = true;
            MiniGameHandler.Instance.handlerForBirth.DestroySperm(spermData);
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Error);
            DestroySperm();
            return;
        }
        UIMiniGameBirthEgg egg = collision.gameObject.GetComponent<UIMiniGameBirthEgg>();
        if (egg)
        {
            isDestroy = true;
            egg.TakeSpermSuccess();
            MiniGameHandler.Instance.handlerForBirth.ArriveEgg(spermData);
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Correct);
            DestroySperm();
            return;
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    public void DestroySperm()
    {
        CanvasGroup canvasGroup= GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0, 0.4f);
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).OnComplete(() => { Destroy(gameObject); });
    }

}