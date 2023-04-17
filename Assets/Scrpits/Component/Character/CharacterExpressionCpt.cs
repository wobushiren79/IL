﻿using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections;

public class CharacterExpressionCpt : BaseMonoBehaviour
{
    public enum CharacterExpressionEnum
    {
        Love=1,
        Wordless=2,
        Mad=3,
        Shame=4,
        Surprise=5,
        Fret=6,
        Dead=7,
        Doubt=8
    }

    [Header("表情图标")]
    public Sprite spLove;
    public Sprite spWordlesse;
    public Sprite spMad;
    public Sprite spShame;
    public Sprite spSurprise;
    public Sprite spFret;
    public Sprite spDead;
    public Sprite spDoubt;

    [Header("控件")]
    public SpriteRenderer spExpression;

    public void SetExpression(int expression)
    {
        SetExpression((CharacterExpressionEnum)expression,2);
    }
    public void SetExpression(CharacterExpressionEnum expression)
    {
        SetExpression(expression, 2);
    }
    public void SetExpression(CharacterExpressionEnum expression,float desTime)
    {
        if (spExpression == null)
            return;
        Sprite spIcon = null;
        switch (expression)
        {
            case CharacterExpressionEnum.Love:
                spIcon = spLove;
                break;
            case CharacterExpressionEnum.Wordless:
                spIcon = spWordlesse;
                break;
            case CharacterExpressionEnum.Mad:
                spIcon = spMad;
                break;
            case CharacterExpressionEnum.Shame:
                spIcon = spShame;
                break;
            case CharacterExpressionEnum.Surprise:
                spIcon = spSurprise;
                break;
            case CharacterExpressionEnum.Fret:
                spIcon = spFret;
                break;
            case CharacterExpressionEnum.Dead:
                spIcon = spDead;
                break;
            case CharacterExpressionEnum.Doubt:
                spIcon = spDoubt;
                break;
        }
        if (spIcon == null)
            return;
        StopAllCoroutines();
        spExpression.transform.localScale = new Vector3(1f, 1f,1f);
        spExpression.transform.DOKill();
        spExpression.sprite = spIcon;
        spExpression.gameObject.SetActive(true);
        spExpression.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From().SetEase(Ease.OutBack).OnComplete(delegate ()
        {
            if (desTime == -1)
            {

            }
            else
            {
                if (this != null && spExpression != null)
                {
                    StartCoroutine(TimeDes(desTime));
                }
            }
        });
    }

    public void CancelExpression()
    {
        if (spExpression != null)
        {
            spExpression.gameObject.SetActive(false);
        }
    }

    public IEnumerator TimeDes(float desTime)
    {
        yield return new WaitForSeconds(desTime);
        if (spExpression != null)
        {
            spExpression.gameObject.SetActive(false);
        }
    }
}