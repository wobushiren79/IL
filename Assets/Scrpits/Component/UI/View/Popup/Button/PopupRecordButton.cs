using UnityEngine;
using UnityEditor;
using System;

public class PopupRecordButton : PopupButtonView<PopupRecordShow>
{
    public InnRecordBean innRecordData;

    public override void PopupHide()
    {

    }

    public override void PopupShow()
    {
        popupShow.SetData(innRecordData);
    }

    public void SetData(InnRecordBean innRecordData)
    {
        this.innRecordData = innRecordData;
    }
}