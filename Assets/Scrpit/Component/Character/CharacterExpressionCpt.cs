using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections;

public class CharacterExpressionCpt : BaseMonoBehaviour
{
    public enum CharacterExpressionEnum
    {
        Love,
    }
    [Header("表情图标")]
    public Sprite spLove;

    [Header("控件")]
    public SpriteRenderer spExpression;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetExpression(CharacterExpressionEnum.Love);
        }
       
    }

    public void SetExpression(CharacterExpressionEnum expression)
    {
        if (spExpression == null)
            return;
        Sprite spIcon = null;
        switch (expression)
        {
            case CharacterExpressionEnum.Love:
                spIcon = spLove;
                break;
        }
        if (spIcon == null)
            return;
        StopAllCoroutines();
        spExpression.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        spExpression.transform.DOKill();
        spExpression.sprite = spIcon;
        spExpression.gameObject.SetActive(true);
        spExpression.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From().SetEase(Ease.OutBack).OnComplete(delegate ()
        {
            StartCoroutine(TimeDes());
        });
    }

    public IEnumerator TimeDes()
    {
        yield return new WaitForSeconds(2);
        if (spExpression != null)
        {
            spExpression.gameObject.SetActive(false);
        }
    }
}