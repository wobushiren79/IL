using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterInteractiveCpt : BaseMonoBehaviour
{
    public GameObject interactiveObj;
    public GameObject objBackground;
    public TextMesh tvContent;
    public TextMesh tvContentShadow;
    public SpriteRenderer srIcon;

    /// <summary>
    /// 展示互动
    /// </summary>
    /// <param name="content"></param>
    public void ShowInteractive(string content)
    {

        //调整大小
        if (content.Length <= 4)
        {
            objBackground.transform.localScale = new Vector3(1, 1, 1) * 1.5f;
        }
        else
        {
            byte[] byte_len = System.Text.Encoding.Default.GetBytes(content);
            objBackground.transform.localScale = new Vector3(1 + (0.04f * (byte_len.Length - 4)), 1, 1) * 1.5f;
        }


        interactiveObj.SetActive(true);
        tvContent.text = content;
        tvContentShadow.text = content;
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