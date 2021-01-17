using UnityEngine;
using UnityEditor;

public class ItemGameCustomBedCpt : ItemTownCerpenterCpt
{
    public override void OnClickSubmitBuy()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);     
        UIGameCustomBed uiGameCustom = (UIGameCustomBed)uiComponent;
        uiGameCustom.SetBedDataByType(buildItemData.GetBuildType(), buildItemData.id);
    }
}