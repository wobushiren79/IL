using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class PopupButtonView : BaseMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PopupShowView popupShow;
    private Button mThisButton;

    public void SetPopupShowView(PopupShowView popupShow)
    {
        this.popupShow = popupShow;
    }

    private void Start()
    {
        mThisButton = GetComponent<Button>();
        if (mThisButton != null)
            mThisButton.onClick.AddListener(ButtonClick);
    }

    public void ButtonClick()
    {
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (popupShow != null)
            popupShow.gameObject.SetActive(true);
        OpenPopup();
        popupShow.RefreshViewSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (popupShow != null)
            popupShow.gameObject.SetActive(false);
        ClosePopup();
    }

    private void OnDisable()
    {
        if (popupShow != null)
            popupShow.gameObject.SetActive(false);
        ClosePopup();
    }

    public abstract void OpenPopup();
    public abstract void ClosePopup();
}