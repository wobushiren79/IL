using UnityEngine;
using UnityEditor;

public class ItemGameCustomBedCpt : ItemTownCerpenterCpt
{
    public override void OnClickSubmitBuy()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        AudioHandler audioHandler = AudioHandler.Instance;
        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        
        UIGameCustomBed uiGameCustom = (UIGameCustomBed)uiComponent;
        InnBuildManager innBuildManager= uiGameManager.innBuildManager;
        uiGameCustom.SetBedDataByType(buildItemData.GetBuildType(), buildItemData.id);
    }
}