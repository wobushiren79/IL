using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToastView : MonoBehaviour
{
    public Image ivIcon;
    public Text tvContent;
    public CanvasGroup cgToast;

    private void Awake()
    {
        cgToast = GetComponent<CanvasGroup>();
    }

    public void SetData(Sprite spIcon,string content,float destoryTime)
    {
        //设置Icon
        SetIcon( spIcon);
        //设置内容
        SetContent( content);
        //定时销毁
        DestroyToast(destoryTime);
    }
   
    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if(ivIcon!= null&& spIcon != null)
        {
            ivIcon.sprite = spIcon;
        }
    }

    /// <summary>
    /// 设置内容
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
        {
            tvContent.text = content;
        }
    }

    /// <summary>
    /// 摧毁Toast
    /// </summary>
    /// <param name="timeDelay"></param>
    private void DestroyToast(float timeDelay)
    {
        if (cgToast != null)
            cgToast.DOFade(0,0.2f).SetDelay(timeDelay);
        transform.DOScale(new Vector3(0, 0), 0.2f).SetDelay(timeDelay).OnComplete(delegate () {
            Destroy(gameObject);
        });
    }
}
