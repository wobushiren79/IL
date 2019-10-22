using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameTest : BaseUIComponent
{
    public InputField etStoryId;
    public Button btStoryCreate;

    public InputField etItemId;
    public InputField etItemNumber;
    public Button btItemCreate;

    public InputField etBuildItemId;
    public InputField etBuildItemNumber;
    public Button btBuildItemCreate;

    private void Start()
    {
        if (btStoryCreate != null)
            btStoryCreate.onClick.AddListener(CreateStory);
        if (btItemCreate != null)
            btItemCreate.onClick.AddListener(AddItem);
        if (btBuildItemCreate != null)
            btBuildItemCreate.onClick.AddListener(AddBuildItem);
    }

    /// <summary>
    /// 创建故事
    /// </summary>
    public void CreateStory()
    {
        if (long.TryParse(etStoryId.text,out long storyId))
        {
            EventHandler eventHandler= GetUIMananger<UIGameManager>().eventHandler;
            eventHandler.EventTriggerForStory(storyId);
        } 
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    public void AddItem()
    {
        if (long.TryParse(etItemId.text, out long itemId))
        {

            if (long.TryParse(etItemNumber.text, out long itemNumber))
            {
                UIGameManager uiGameManager= GetUIMananger<UIGameManager>();
                uiGameManager.gameDataManager.gameData.ChangeItemsNumber(itemId, itemNumber);
            }
            else
            {
                LogUtil.LogError("道具数量输入错误");
            } 
        }
        else
        {
            LogUtil.LogError("道具ID输入错误");
        }
    }


    /// <summary>
    /// 添加道具
    /// </summary>
    public void AddBuildItem()
    {
        if (long.TryParse(etBuildItemId.text, out long itemId))
        {

            if (long.TryParse(etBuildItemNumber.text, out long itemNumber))
            {
                UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
                uiGameManager.gameDataManager.gameData.ChangeBuildNumber(itemId, itemNumber);
            }
            else
            {
                LogUtil.LogError("建筑数量输入错误");
            }
        }
        else
        {
            LogUtil.LogError("建筑ID输入错误");
        }
    }
}