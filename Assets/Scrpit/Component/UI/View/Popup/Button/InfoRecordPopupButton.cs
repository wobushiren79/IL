using UnityEngine;
using UnityEditor;
using System;

public class InfoRecordPopupButton : PopupButtonView
{
    public InnRecordBean innRecordData;

    public override void ClosePopup()
    {
    
    }

    public override void OpenPopup()
    {
        ((InfoRecordPopupShow)popupShow).SetData(innRecordData);
    }

    public void SetData(InnRecordBean innRecordData)
    {
        this.innRecordData = innRecordData;
    }
}