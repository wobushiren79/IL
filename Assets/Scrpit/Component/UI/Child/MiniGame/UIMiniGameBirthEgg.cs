using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class UIMiniGameBirthEgg : BaseUIChildComponent<UIMiniGameBirth>
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
        transform.DOShakeScale(0.5f,1,5,10);
    }

}