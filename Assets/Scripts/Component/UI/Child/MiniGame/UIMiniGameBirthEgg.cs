using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class UIMiniGameBirthEgg : BaseUIView
{

    /// <summary>
    /// 成功接受精子
    /// </summary>
    public void TakeSpermSuccess()
    {
        AnimForSuccess();
    }

    /// <summary>
    /// 成功动画
    /// </summary>
    public void AnimForSuccess()
    {
        transform.DOKill();
        transform.transform.localScale = Vector3.one;
        transform.DOScale(new Vector3(1.2f,1.2f,1.2f),0.5f).SetEase(Ease.OutBack);
    }

}