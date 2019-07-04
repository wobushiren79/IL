using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class ItemsSelectionBox : BaseMonoBehaviour
{

    public ICallBack callBack;
    public Button btBack;

    public Button btUse;
    public Button btDiscard;
    public GameObject objContent;

    //屏幕(用来找到鼠标点击的相对位置)
    public RectTransform screenRTF;
    public RectTransform popupRTF;
    //鼠标位置和弹窗偏移量
    public float offsetX = 0;
    public float offsetY = 0;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(Close);
        if (btUse != null)
            btUse.onClick.AddListener(UseItems);
        if (btDiscard != null)
            btDiscard.onClick.AddListener(DiscardItems);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open(int type)
    {
        btUse.gameObject.SetActive(true);
        btDiscard.gameObject.SetActive(true);
        switch (type)
        {
            case 0:
                btUse.gameObject.SetActive(false);
                break;
            case 1:
                break;
        }
        gameObject.SetActive(true);
        objContent.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutBack).From();
        SetLocation();
    }

    public void SetLocation()
    {
        //如果显示Popup 则调整位置为鼠标位置
        if (gameObject.activeSelf)
        {
            Vector2 outPosition;
            //屏幕坐标转换为UI坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out outPosition);
            float moveX = outPosition.x;
            float moveY = outPosition.y;
            //Vector3 newPosition= Vector3.Lerp(transform.localPosition, new Vector3(moveX + offsetX, moveY + offsetY, transform.localPosition.z),0.5f);
            objContent.transform.localPosition = new Vector3(moveX + offsetX, moveY + offsetY, transform.localPosition.z);
        }
    }

    /// <summary>
    /// 使用物品
    /// </summary>
    public void UseItems()
    {
        if (callBack != null)
            callBack.SelectionUse(this);
        Close();
    }

    /// <summary>
    /// 丢弃物品
    /// </summary>
    public void DiscardItems()
    {
        if (callBack != null)
            callBack.SelectionDiscard(this);
        Close();
    }

    public interface ICallBack
    {
        /// <summary>
        /// 选择使用
        /// </summary>
        /// <param name="view"></param>
        void SelectionUse(ItemsSelectionBox view);

        /// <summary>
        /// 选择丢弃
        /// </summary>
        /// <param name="view"></param>
        void SelectionDiscard(ItemsSelectionBox view);
    }
}