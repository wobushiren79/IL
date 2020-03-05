using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterInteractiveCpt : BaseMonoBehaviour
{
    public GameObject interactiveObj;
    public TextMesh tvContent;
    public SpriteRenderer srIcon;

    /// <summary>
    /// 展示互动
    /// </summary>
    /// <param name="content"></param>
    public void ShowInteractive(string content)
    {
        interactiveObj.SetActive(true);
        tvContent.text = content;
        AnimForInteractive();
    }

    /// <summary>
    /// 互动动画
    /// </summary>
    public void AnimForInteractive()
    {
        interactiveObj.transform.DOKill();
        interactiveObj.transform.localScale = new Vector3(2, 2, 2);
        interactiveObj.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f).SetEase(Ease.OutBack).From();
    }

    /// <summary>
    /// 关闭互动
    /// </summary>
    public void CloseInteractive()
    {
        interactiveObj.SetActive(false);
    }

}