using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class PopupButtonView<T>  : BaseMonoBehaviour, IPointerEnterHandler, IPointerExitHandler  where T : PopupShowView
{
    protected T popupShow;
    private Button mThisButton;
    protected bool isActive = true;
    public virtual void Awake()
    {
        popupShow = FindInChildren<T>(ImportantTypeEnum.Popup);
    }

    private void Start()
    {
        mThisButton = GetComponent<Button>();
        if (mThisButton != null)
            mThisButton.onClick.AddListener(ButtonClick);
    }

    public void SetPopupShowView(T popupShow)
    {
        this.popupShow = popupShow;
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public void ButtonClick()
    {
        OnPointerExit(null);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (popupShow == null || !isActive)
            return;
        popupShow.gameObject.SetActive(true);
        OpenPopup();
        popupShow.RefreshViewSize();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (popupShow == null)
            return;
        StopAllCoroutines();
        popupShow.gameObject.SetActive(false);
        ClosePopup();
    }

    public void OnDisable()
    {
        if (popupShow == null)
            return;
        StopAllCoroutines();
        popupShow.gameObject.SetActive(false);
        ClosePopup();
    }

    public abstract void OpenPopup();
    public abstract void ClosePopup();
}