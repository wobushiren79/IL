using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class UIMiniGameBirthEnemy : BaseUIChildComponent<UIMiniGameBirth>
{
    protected float timeForEnemy = 10;

    public void SetData(Vector3 startPosition, Vector3 endPosition)
    {
        RectTransform rtf = (RectTransform)transform;
        rtf.anchoredPosition = startPosition;
        rtf
            .DOAnchorPos(endPosition, timeForEnemy)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}