using UnityEngine;
using UnityEditor;
using System;
using DG.Tweening;

public class PopupShowView : BaseMonoBehaviour
{
    //屏幕(用来找到鼠标点击的相对位置)
    public RectTransform screenRTF;
    public RectTransform popupRTF;

    //鼠标位置和弹窗偏移量
    public float offsetX = 0;
    public float offsetY = 0;

    public virtual void Update()
    {
        if (screenRTF == null)
            return;
        //如果显示Popup 则调整位置为鼠标位置
        if (gameObject.activeSelf)
        {
            Vector2 outPosition;
            //屏幕坐标转换为UI坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out outPosition);
            float moveX = outPosition.x;
            float moveY = outPosition.y;
            //Vector3 newPosition= Vector3.Lerp(transform.localPosition, new Vector3(moveX + offsetX, moveY + offsetY, transform.localPosition.z),0.5f);
            transform.localPosition = new Vector3(moveX + offsetX, moveY + offsetY, transform.localPosition.z);
        }
    }

    public void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.DOScale(new Vector3(0, 0, 0), 0.3f).From();
    }

    public void OnDisable()
    {
        if (popupRTF != null)
        {
            popupRTF.anchoredPosition = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// 刷新控件大小
    /// </summary>
    public void RefreshViewSize()
    {
        GameUtil.RefreshRectViewHight(popupRTF, false);
    }
}