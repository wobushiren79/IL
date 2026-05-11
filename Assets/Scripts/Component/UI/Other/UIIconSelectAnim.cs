using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIIconSelectAnim : BaseMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator selectAnim;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectAnim.SetInteger("Status", 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectAnim.SetInteger("Status", 0);
    }
}
