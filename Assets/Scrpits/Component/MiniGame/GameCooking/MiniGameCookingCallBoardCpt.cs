using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class MiniGameCookingCallBoardCpt : BaseMonoBehaviour
{
    public GameObject objContent;
    public TextMesh tvContent;

    /// <summary>
    /// 设置通告板内容
    /// </summary>
    /// <param name="content"></param>
    public void SetCallBoardContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    /// <summary>
    /// 设置通告板状态
    /// </summary>
    /// <param name="isShow"></param>
    public void SetCallBoardStatus(bool isShow)
    {
        if (isShow)
        {
            objContent.transform.DOLocalMoveY(-1f, 3f).SetEase(Ease.OutCubic);
        }
        else
        {
            objContent.transform.DOLocalMoveY(0.72f,3f);
        }
    }
}