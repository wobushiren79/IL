using UnityEngine;
using UnityEditor;

public class PopupLoveMenusButton : PopupButtonView<PopupLoveMenusShow>
{
    public string idForTeamCustomer;

    public void SetDataForTeamCustomer(string id)
    {
        this.idForTeamCustomer = id;
    }

    public override void PopupShow()
    {
        popupShow.SetDataForTeamCustomer(long.Parse(idForTeamCustomer));
    }

    public override void PopupHide()
    {

    }
}