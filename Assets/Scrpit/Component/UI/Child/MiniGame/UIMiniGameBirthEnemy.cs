using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class UIMiniGameBirthEnemy : BaseUIChildComponent<UIMiniGameBirth>
{
    protected float speedForEnemy = 10;

    public void SetData(Vector3 startPosition, Vector3 endPosition, float speedForEnemy)
    {
        this.speedForEnemy = speedForEnemy;
        RectTransform rtf = (RectTransform)transform;
        rtf.eulerAngles = new Vector3(0, 0, VectorUtil.GetAngle(startPosition, endPosition) - 90);
        rtf.anchoredPosition = startPosition;
        rtf
            .DOAnchorPos(endPosition, speedForEnemy)
            .SetEase(Ease.Linear)
            .OnComplete(() => { DestroyEnemy(); });
    }

    /// <summary>
    /// 删除
    /// </summary>
    public void DestroyEnemy()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0, 0.4f);
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).OnComplete(() => { Destroy(gameObject); });
    }
}