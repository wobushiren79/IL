using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class PopupItemsSelection : BaseMonoBehaviour
{
    public enum SelectionTypeEnum
    {
        Discard,//只有丢弃
        Use,//使用
        UseAndDiscard,//使用和丢弃
        EquipAndDiscardAndTFEquip,//装备和幻化和丢弃
        Unload,//卸除 用于装备界面
        Gift,//赠送
        ReadAndDiscard,//阅读和丢弃
    }

    public ICallBack callBack;
    public Button btBack;

    public Button btUse;
    public Button btDiscard;
    public Button btEquip;
    public Button btTFEquip;
    public Button btUnload;
    public Button btGift;
    public Button btRead;
    public GameObject objContent;

    //屏幕(用来找到鼠标点击的相对位置)
    public RectTransform screenRTF;
    public RectTransform popupRTF;
    //鼠标位置和弹窗偏移量
    public float offsetX = 0;
    public float offsetY = 0;

    protected  AudioHandler audioHandler;
    private void Awake()
    {
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(Close);
        if (btUse != null)
            btUse.onClick.AddListener(UseItems);
        if (btDiscard != null)
            btDiscard.onClick.AddListener(DiscardItems);
        if (btEquip != null)
            btEquip.onClick.AddListener(EquipItems);
        if (btTFEquip != null)
            btTFEquip.onClick.AddListener(TFEquipItems);
        if (btUnload != null)
            btUnload.onClick.AddListener(UnloadItems);
        if (btGift != null)
            btGift.onClick.AddListener(GiftItems);
        if (btRead != null)
            btRead.onClick.AddListener(ReadItems);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }


    public void Close()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        gameObject.SetActive(false);
    }

    public void Open(SelectionTypeEnum type)
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForHighLight);
        btUse.gameObject.SetActive(false);
        btDiscard.gameObject.SetActive(false);
        btEquip.gameObject.SetActive(false);
        btUnload.gameObject.SetActive(false);
        btGift.gameObject.SetActive(false);
        btRead.gameObject.SetActive(false);
        btTFEquip.gameObject.SetActive(false);

        switch (type)
        {
            case SelectionTypeEnum.Discard:
                btDiscard.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.Use:
                btUse.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.UseAndDiscard:
                btUse.gameObject.SetActive(true);
                btDiscard.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.EquipAndDiscardAndTFEquip:
                btEquip.gameObject.SetActive(true);
                btDiscard.gameObject.SetActive(true);
                btTFEquip.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.Unload:
                btUnload.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.Gift:
                btGift.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.ReadAndDiscard:
                btRead.gameObject.SetActive(true);
                btDiscard.gameObject.SetActive(true);
                break;
        }
        gameObject.SetActive(true);
        objContent.transform.localScale = Vector3.one;
        objContent.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutBack).From();
        SetLocation();
    }

    public void SetLocation()
    {
        //如果显示Popup 则调整位置为鼠标位置
        if (gameObject.activeSelf)
        {
            //屏幕坐标转换为UI坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out Vector2 outPosition);
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

    /// <summary>
    /// 装备
    /// </summary>
    public void EquipItems()
    {
        if (callBack != null)
            callBack.SelectionEquip(this);
        Close();
    }

    /// <summary>
    /// 幻化
    /// </summary>
    public void TFEquipItems()
    {
        if (callBack != null)
            callBack.SelectionTFEquip(this);
        Close();
    }

    /// <summary>
    /// 卸下
    /// </summary>
    public void UnloadItems()
    {
        if (callBack != null)
            callBack.SelectionUnload(this);
        Close();
    }

    /// <summary>
    /// 赠送
    /// </summary>
    public void GiftItems()
    {
        if (callBack != null)
            callBack.SelectionGift(this);
        Close();
    }

    /// <summary>
    /// 阅读
    /// </summary>
    public void ReadItems()
    {
        if (callBack != null)
            callBack.SelectionRead(this);
        Close();
    }

    public interface ICallBack
    {
        /// <summary>
        /// 选择使用
        /// </summary>
        /// <param name="view"></param>
        void SelectionUse(PopupItemsSelection view);

        /// <summary>
        /// 选择丢弃
        /// </summary>
        /// <param name="view"></param>
        void SelectionDiscard(PopupItemsSelection view);

        /// <summary>
        /// 选择装备
        /// </summary>
        /// <param name="view"></param>
        void SelectionEquip(PopupItemsSelection view);

        /// <summary>
        /// 选择幻化装备
        /// </summary>
        /// <param name="view"></param>
        void SelectionTFEquip(PopupItemsSelection view);

        /// <summary>
        /// 选择卸下
        /// </summary>
        /// <param name="view"></param>
        void SelectionUnload(PopupItemsSelection view);

        /// <summary>
        /// 选择赠送
        /// </summary>
        /// <param name="view"></param>
        void SelectionGift(PopupItemsSelection view);

        /// <summary>
        /// 选择阅读
        /// </summary>
        /// <param name="view"></param>
        void SelectionRead(PopupItemsSelection view);
    }
}