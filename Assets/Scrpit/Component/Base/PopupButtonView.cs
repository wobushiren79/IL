using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class PopupButtonView<T> : BaseMonoBehaviour, IPointerEnterHandler, IPointerExitHandler where T : PopupShowView
{
    protected T popupShow;
    protected Button thisButton;
    protected bool isActive = true;

    private void Start()
    {
        thisButton = GetComponent<Button>();
        if (thisButton != null)
            thisButton.onClick.AddListener(ButtonClick);
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
        if (!isActive)
            return;
        if (popupShow != null)
            return;
        string popupName = this.GetType().Name.Replace("Button", "");
        popupShow = PopupHandler.Instance.CreatePopup<T>(popupName);
        OpenPopup();
        popupShow.RefreshViewSize();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (popupShow == null)
            return;
        StopAllCoroutines();
        Destroy(popupShow.gameObject);
        popupShow = null;
        ClosePopup();
    }

    public void OnDisable()
    {
        OnPointerExit(null);
    }

    public abstract void OpenPopup();
    public abstract void ClosePopup();
}