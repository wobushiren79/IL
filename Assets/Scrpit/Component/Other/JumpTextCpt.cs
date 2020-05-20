using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class JumpTextCpt : BaseMonoBehaviour
{
    public TextMesh tvContent;
    public TextMesh tvContentShadow;


    public void SetData(string content, Color colorContent)
    {
        SetText(content, colorContent);
        AnimForInit();
    }

    /// <summary>
    /// 设置文字
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text, Color colorContent)
    {
        if (tvContent != null)
        {
            tvContent.text = text;
            tvContent.color = colorContent;
        }
        if (tvContentShadow != null)
        {
            tvContentShadow.text = text;
        }
    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit()
    {
        //数字特效
        transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f, 5, 1);
        transform.DOLocalMoveY(1.5f, 2.5f).OnComplete(delegate ()
        {
            Destroy(this);
        });
        if (tvContent != null)
            DOTween.To(() => 1f, alpha => tvContent.color = new Color(tvContent.color.r, tvContent.color.g, tvContent.color.b, alpha), 0f, 1).SetDelay(2.5f);
        if (tvContentShadow != null)
            DOTween.To(() => 1f, alpha => tvContentShadow.color = new Color(tvContentShadow.color.r, tvContentShadow.color.g, tvContentShadow.color.b, alpha), 0f, 1).SetDelay(2.5f);
    }
}