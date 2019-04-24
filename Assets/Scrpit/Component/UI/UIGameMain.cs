using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameMain : BaseUIComponent
{
    public Button btBuild;
    public Button btSave;

    public GameDataManager gameDataManager;
    public InnWallBuilder innWall;
    public void Start()
    {
        if (btBuild != null)
            btBuild.onClick.AddListener(OpenBuildUI);

        if (btSave != null)
            btSave.onClick.AddListener(SaveData);
    }

    public void SaveData()
    {
        gameDataManager.SaveGameData();
    }

    public void OpenBuildUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Build");
    }
}