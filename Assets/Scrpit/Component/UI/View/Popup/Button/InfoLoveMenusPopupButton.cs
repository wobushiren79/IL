using UnityEngine;
using UnityEditor;

public class InfoLoveMenusPopupButton : PopupButtonView<InfoLoveMenusPopupShow>
{
    public long idForTeamCustomer;

    public void SetDataForTeamCustomer(long id)
    {
        this.idForTeamCustomer = id;
    }

    public override void ClosePopup()
    {
    }

    public override void OpenPopup()
    {
        popupShow.SetDataForTeamCustomer(idForTeamCustomer);
    }

}