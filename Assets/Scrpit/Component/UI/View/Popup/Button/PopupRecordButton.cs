using UnityEngine;
using UnityEditor;
using System;

public class PopupRecordButton : PopupButtonView<PopupRecordShow>
{
    public InnRecordBean innRecordData;

    public override void ClosePopup()
    {
    
    }

    public override void OpenPopup()
    {
        popupShow.SetData(innRecordData);
    }

    public void SetData(InnRecordBean innRecordData)
    {
        this.innRecordData = innRecordData;
    }
}