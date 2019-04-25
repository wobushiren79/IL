using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameMain : BaseUIComponent
{
    public Button btBuild;
    public Button btSave;
    public Button btSleep;

    public GameDataManager gameDataManager;
    public InnHandler innHandler;
    public InnWallBuilder innWall;

    public void Start()
    {
        if (btBuild != null)
            btBuild.onClick.AddListener(OpenBuildUI);

        if (btSave != null)
            btSave.onClick.AddListener(SaveData);

        if (btSleep != null)
            btSleep.onClick.AddListener(EndDay);
    }

    public void SaveData()
    {
        gameDataManager.SaveGameData();
    }

    public void OpenBuildUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Build");
    }

    public void EndDay()
    {
        uiManager.OpenUIAndCloseOtherByName("Start");
        innHandler.CloseInn();
    }
}