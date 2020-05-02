using UnityEngine;
using UnityEditor;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class PopupShowView : BaseMonoBehaviour
{
    //屏幕(用来找到鼠标点击的相对位置)
    protected RectTransform screenRTF;
    protected LayoutGroup popuplayoutGroup;
    public RectTransform popupRTF;

    //鼠标位置和弹窗偏移量
    public float offsetX = 0;
    public float offsetY = 0;

    public virtual void Awake()
    {
        screenRTF = (RectTransform)transform.parent.transform;
        popuplayoutGroup = GetComponent<LayoutGroup>();
    }

    public virtual void Update()
    {
        if (screenRTF == null)
            return;
        //如果显示Popup 则调整位置为鼠标位置
        InitPosition();
    }

    public void OnEnable()
    {
        InitPosition();
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

    
    public void InitPosition()
    {
        if (gameObject.activeSelf)
        {

            //屏幕坐标转换为UI坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out Vector2 outPosition);
            float moveX = outPosition.x;
            float moveY = outPosition.y;

            transform.localPosition = new Vector3(moveX + offsetX, moveY + offsetY, transform.localPosition.z);

            //判断鼠标在屏幕的左右
            if (Input.mousePosition.x <= (Screen.width / 2))
            {
                //左
                popupRTF.pivot = new Vector2(0, 0.5f);
                if (popuplayoutGroup != null)
                    popuplayoutGroup.childAlignment = TextAnchor.UpperLeft;
            }
            else
            {
                //右
                popupRTF.pivot = new Vector2(1, 0.5f);
                if (popuplayoutGroup != null)
                    popuplayoutGroup.childAlignment = TextAnchor.UpperRight;
            }
            //Vector3 newPosition= Vector3.Lerp(transform.localPosition, new Vector3(moveX + offsetX, moveY + offsetY, transform.localPosition.z),0.5f);
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